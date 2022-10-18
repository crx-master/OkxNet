using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpCryptoExchange.Authentication;
using SharpCryptoExchange.Logging;
using SharpCryptoExchange.Objects;
using SharpCryptoExchange.Okx.Helpers;
using SharpCryptoExchange.Okx.Models.Core;
using SharpCryptoExchange.Sockets;
using System;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SharpCryptoExchange.Okx
{
    public partial class OkxSocketClient : BaseSocketClient
    {
        #region Properties
        public bool Connected
        {
            get
            {
                return SocketConnections.Values.Where(c => c.Connected).Any();
            }
        }
        #endregion

        #region Internal Fields
        internal OkxSocketClientOptions Options { get; }
        internal OkxSocketClientUnifiedSocket UnifiedSocket { get; }
        internal OkxApiCredentials Credentials { get; private set; }
        internal bool IsAuthendicated { get; private set; }
        #endregion

        #region Constructor/Destructor
        public OkxSocketClient() : this(OkxSocketClientOptions.Default)
        {
        }

        public OkxSocketClient(OkxSocketClientOptions options) : base("OKX WebSocket API", options)
        {
            SetDataInterpreter(DecompressData, null);

            //TODO: those periodic PING is not working properly
            /*
             * 2022/08/29 12:24:02:608 | Error | OKX WebSocket API | Deserialize JsonReaderException: Unexpected character encountered while parsing value: p. Path '', line 0, position 0., Path: , LineNumber: 0, LinePosition: 0. Data: pong
             * 2022/08/29 12:24:02:610 | Warning | OKX WebSocket API | Socket 1 Message not handled: pong
             */
            //SendPeriodic("Ping", TimeSpan.FromSeconds(5), con => "ping");
            Credentials = options.ApiCredentials;
            UnifiedSocket = AddApiClient(new OkxSocketClientUnifiedSocket(Logger, this, options));
        }
        #endregion

        #region Common Methods
        public static void SetDefaultOptions(OkxSocketClientOptions options)
        {
            OkxSocketClientOptions.Default = options;
        }

        public virtual void SetApiCredentials(OkxApiCredentials credentials)
        {
            Credentials = credentials;
            UnifiedSocket.SetApiCredentials(credentials);
        }
        public virtual void SetApiCredentials(string apiKey, string apiSecret, string passPhrase)
        {
            var credentials = new OkxApiCredentials(apiKey, apiSecret, passPhrase);
            Credentials = credentials;
            UnifiedSocket.SetApiCredentials(credentials);
        }
        #endregion

        //public virtual CallResult<OkxSocketPingPong> Ping() => PingAsync().Result;
        //public virtual async Task<CallResult<OkxSocketPingPong>> PingAsync()
        //{
        //    if (!Connected) throw new ApplicationException("Not connected!");
        //    foreach(var connection in socketConnections.Values)
        //    {
        //        var pit = DateTime.UtcNow;
        //        var sw = Stopwatch.StartNew();
        //        var response = await QueryAndWaitAsync<string>(connection, "ping").ConfigureAwait(true);
        //        var pot = DateTime.UtcNow;
        //        sw.Stop();
        //        var result = new OkxSocketPingPong { PingTime = pit, PongTime = pot, Latency = sw.Elapsed, PongMessage = response.Data };
        //        if (response != null && response.Error != null)
        //            return new CallResult<OkxSocketPingPong>(result);
        //        else
        //            return new CallResult<OkxSocketPingPong>(response.Error);
        //    }
        //    throw new ApplicationException("No connections to ping!");
        //}
        protected static string DecompressData(byte[] byteData)
        {
            using var decompressedStream = new MemoryStream();
            using var compressedStream = new MemoryStream(byteData);
            using var deflateStream = new DeflateStream(compressedStream, CompressionMode.Decompress);
            deflateStream.CopyTo(decompressedStream);
            decompressedStream.Position = 0;

            using var streamReader = new StreamReader(decompressedStream);
            /** /
            var response = streamReader.ReadToEnd();
            return response;
            /**/

            return streamReader.ReadToEnd();
        }
        internal virtual Task<CallResult<T>> UnifiedQueryAsync<T>(object request, bool authenticated)
        {
            return QueryAsync<T>(UnifiedSocket, request, authenticated);
        }
        internal virtual Task<CallResult<UpdateSubscription>> UnifiedSubscribeAsync<T>(object request, string identifier, bool authenticated, Action<DataEvent<T>> dataHandler, CancellationToken ct)
        {
            return SubscribeAsync(UnifiedSocket, request, identifier, authenticated, dataHandler, ct);
        }
        protected override bool HandleQueryResponse<T>(SocketConnection socketConnection, object request, JToken data, out CallResult<T> callResult)
        {
            return OkxHandleQueryResponse<T>(socketConnection, request, data, out callResult);
        }


        protected virtual bool OkxHandleQueryResponse<T>(SocketConnection s, object request, JToken data, out CallResult<T> callResult)
        {
            callResult = null;

            // Ping Request
            if (request.ToString() == "ping" && data.ToString() == "pong")
            {
                return true;
            }

            // Check for Error
            if (data is JObject && data["event"] != null && (string)data["event"]! == "error" && data["code"] != null && data["msg"] != null)
            {
                LogHelper.LogWarningMessage(Logger, "Query failed: " + (string)data["msg"]!);
                callResult = new CallResult<T>(new ServerError($"{(string)data["code"]!}, {(string)data["msg"]!}"));
                return true;
            }

            // Login Request
            if (data is JObject && data["event"] != null && (string)data["event"]! == "login")
            {
                var desResult = Deserialize<T>(data);
                if (!desResult)
                {
                    LogHelper.LogWarningMessage(Logger, $"Failed to deserialize data: {desResult.Error}. Data: {data}");
                    return false;
                }

                callResult = new CallResult<T>(desResult.Data);
                return true;
            }

            return false;
        }
        protected override bool HandleSubscriptionResponse(SocketConnection socketConnection, SocketSubscription subscription, object request, JToken data, out CallResult<object> callResult)
        {
            return OkxHandleSubscriptionResponse(socketConnection, subscription, request, data, out callResult);
        }
        protected virtual bool OkxHandleSubscriptionResponse(SocketConnection s, SocketSubscription subscription, object request, JToken data, out CallResult<object> callResult)
        {
            callResult = null;

            // Ping-Pong
            var json = data.ToString();
            if (json == "pong")
                return false;

            // Check for Error
            // 30040: {0} Channel : {1} doesn't exist
            if (data.HasValues && data["event"] != null && (string)data["event"]! == "error" && data["errorCode"] != null && (string)data["errorCode"]! == "30040")
            {
                LogHelper.LogWarningMessage(Logger, $"Subscription failed: {data["message"]}");
                callResult = new CallResult<object>(new ServerError($"{(string)data["errorCode"]!}, {(string)data["message"]!}"));
                return true;
            }

            // Check for Success
            if (data.HasValues && data["event"] != null && (string)data["event"]! == "subscribe" && data["arg"]["channel"] != null)
            {
                if (request is OkxSocketRequest socRequest)
                {
                    if (socRequest.Arguments.FirstOrDefault().Channel == (string)data["arg"]["channel"]!)
                    {
                        LogHelper.LogDebugMessage(Logger, "Subscription completed");
                        callResult = new CallResult<object>(true);
                        return true;
                    }
                }
            }

            return false;
        }
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        {
            return OkxMessageMatchesHandler(socketConnection, message, request);
        }
        protected virtual bool OkxMessageMatchesHandler(SocketConnection s, JToken message, object request)
        {
            // Ping Request
            if (request.ToString() == "ping" && message.ToString() == "pong")
                return false;

            // Check Point
            if (message.Type != JTokenType.Object)
                return false;

            // Socket Request
            if (request is OkxSocketRequest socketRequest)
            {
                // Check for Error
                if (message is JObject && message["event"] != null && (string)message["event"]! == "error" && message["code"] != null && message["msg"] != null)
                    return false;

                // Check for Channel
                if (socketRequest.Operation != OkxSocketOperation.Subscribe || message["arg"]["channel"] == null)
                    return false;

                // Compare Request and Response Arguments
                var reqArg = socketRequest.Arguments.FirstOrDefault();
                var resArg = JsonConvert.DeserializeObject<OkxSocketRequestArgument>(message["arg"].ToString());

                // Check Data
                var data = message["data"];
                if (data?.HasValues ?? false)
                {
                    if (reqArg.Channel == resArg.Channel &&
                        reqArg.Underlying == resArg.Underlying &&
                        reqArg.InstrumentId == resArg.InstrumentId &&
                        reqArg.InstrumentType == resArg.InstrumentType)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            return OkxMessageMatchesHandler(socketConnection, message, identifier);
        }
        protected virtual bool OkxMessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            return true;
        }
        protected override async Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection socketConnection)
        {
            return await OkxAuthenticateSocket(socketConnection);
        }
        protected virtual async Task<CallResult<bool>> OkxAuthenticateSocket(SocketConnection s)
        {
            // Check Point
            //if (s.Authenticated)
            //    return new CallResult<bool>(true, null);

            // Check Point
            if (Credentials == null || Credentials.Key == null || Credentials.Secret == null || Credentials.PassPhrase == null)
                return new CallResult<bool>(new NoApiCredentialsError());

            // Get Credentials
            var key = Credentials.Key.GetString();
            var secret = Credentials.Secret.GetString();
            var passphrase = Credentials.PassPhrase.GetString();

            // Check Point
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(passphrase))
                return new CallResult<bool>(new NoApiCredentialsError());

            // Timestamp
            var timestamp = (DateTime.UtcNow.ToUnixTimeMilliSeconds() / 1000.0m).ToString(CultureInfo.InvariantCulture);

            // Signature
            var signtext = timestamp + "GET" + "/users/self/verify";
            var hmacEncryptor = new HMACSHA256(Encoding.ASCII.GetBytes(secret));
            var signature = OkxAuthenticationProvider.Base64Encode(hmacEncryptor.ComputeHash(Encoding.UTF8.GetBytes(signtext)));
            var request = new OkxSocketAuthRequest(OkxSocketOperation.Login, new OkxSocketAuthRequestArgument
            {
                ApiKey = key,
                Passphrase = passphrase,
                Timestamp = timestamp,
                Signature = signature,
            });

            // Try to Login
            var result = new CallResult<bool>(new ServerError("No response from server"));
            await s.SendAndWaitAsync(request, TimeSpan.FromSeconds(10), data =>
            {
                if ((string)data["event"] != "login")
                    return false;

                var authResponse = Deserialize<OkxSocketResponse>(data);
                if (!authResponse)
                {
                    LogHelper.LogWarningMessage(Logger, "Authorization failed: " + authResponse.Error);
                    result = new CallResult<bool>(authResponse.Error);
                    return true;
                }
                if (!authResponse.Data.Success)
                {
                    LogHelper.LogWarningMessage(Logger, "Authorization failed: " + authResponse.Error.Message);
                    result = new CallResult<bool>(new ServerError(authResponse.Error.Code.Value, authResponse.Error.Message));
                    return true;
                }

                LogHelper.LogDebugMessage(Logger, "Authorization completed");
                result = new CallResult<bool>(true);
                IsAuthendicated = true;
                return true;
            });

            return result;
        }
        protected override Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription subscriptionToUnsub)
        {
            return OkxUnsubscribeAsync(connection, subscriptionToUnsub);
        }
        protected virtual async Task<bool> OkxUnsubscribeAsync(SocketConnection connection, SocketSubscription subscriptionToUnsub)
        {
            if (subscriptionToUnsub == null || subscriptionToUnsub.Request == null)
                return false;

            var request = new OkxSocketRequest(OkxSocketOperation.Unsubscribe, ((OkxSocketRequest)subscriptionToUnsub.Request).Arguments);
            await connection.SendAndWaitAsync(request, TimeSpan.FromSeconds(10), data =>
            {
                if (data.Type != JTokenType.Object)
                    return false;

                if ((string)data["event"] == "unsubscribe")
                {
                    return (string)data["arg"]["channel"] == request.Arguments.FirstOrDefault().Channel;
                }

                return false;
            });
            return false;
        }
        protected override async Task<CallResult<SocketConnection>> GetSocketConnection(SocketApiClient apiClient, string address, bool authenticated)
        {
            address = authenticated
                ? "wss://ws.okx.com:8443/ws/v5/private"
                : "wss://ws.okx.com:8443/ws/v5/public";

            if (((OkxSocketClientOptions)ClientOptions).DemoTradingService)
            {
                address = authenticated
                    ? "wss://wspap.okx.com:8443/ws/v5/private?brokerId=9999"
                    : "wss://wspap.okx.com:8443/ws/v5/public?brokerId=9999";
            }
            return await base.GetSocketConnection(apiClient, address, authenticated);
        }
    }

    public class OkxSocketClientUnifiedSocket : SocketApiClient
    {
        #region Internal Fields
        internal readonly OkxSocketClient _baseClient;
        internal readonly ILogger _logger;
        #endregion

        internal OkxSocketClientUnifiedSocket(ILogger logger, OkxSocketClient baseClient, OkxSocketClientOptions options) : base(options, options.UnifiedStreamsOptions)
        {
            _baseClient = baseClient;
            _logger = logger;
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new OkxAuthenticationProvider((OkxApiCredentials)credentials);
    }

}