using System;
using System.IO;
using CryptoExchange.Net.Authentication;
using System.Security;
using System.Text;
using CryptoExchange.Net;
using Newtonsoft.Json.Linq;

namespace OkxNet.Objects.Core
{
    public class OkxApiCredentials : ApiCredentials
    {
        public SecureString PassPhrase { get; }

        public OkxApiCredentials(string apiKey, string apiSecret, string apiPassPhrase) : base(apiKey, apiSecret)
        {
            PassPhrase = apiPassPhrase.ToSecureString();
        }

        public override ApiCredentials Copy()
        {
            return new OkxApiCredentials(Key!.GetString(), Secret!.GetString(), PassPhrase!.GetString());
        }
    }
}
