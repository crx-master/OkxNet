#nullable enable
using CryptoExchange.Net.Objects;

namespace OkxNet.Objects.Core
{
    public class OkxSocketClientOptions : BaseSocketClientOptions
    {
        public bool DemoTradingService { get; set; } = false;

        public static OkxSocketClientOptions Default { get; set; } = new OkxSocketClientOptions()
        {
            SocketSubscriptionsCombineTarget = 100,
            MaxSocketConnections = 50
        };

        public new OkxApiCredentials? ApiCredentials
        {
            get
            {
                return (OkxApiCredentials?)base.ApiCredentials;
            }

            set => base.ApiCredentials = value;
        }

        private OkxSocketApiClientOptions _unifiedStreamsOptions = new OkxSocketApiClientOptions();
        public OkxSocketApiClientOptions UnifiedStreamsOptions
        {
            get => _unifiedStreamsOptions;
            set => _unifiedStreamsOptions = new OkxSocketApiClientOptions(_unifiedStreamsOptions, value);
        }

        public OkxSocketClientOptions() : this(Default)
        {
        }

        internal OkxSocketClientOptions(OkxSocketClientOptions baseOn) : base(baseOn)
        {
            if (baseOn == null)
                return;

            ApiCredentials = baseOn.ApiCredentials?.Copy() as OkxApiCredentials;
            _unifiedStreamsOptions = new OkxSocketApiClientOptions(baseOn.UnifiedStreamsOptions, null);
        }
    }

    public class OkxSocketApiClientOptions : ApiClientOptions
    {
        public new OkxApiCredentials? ApiCredentials
        {
            get => (OkxApiCredentials?)base.ApiCredentials;
            set => base.ApiCredentials = value;
        }

        public OkxSocketApiClientOptions()
        {
        }

        internal OkxSocketApiClientOptions(OkxSocketApiClientOptions baseOn, OkxSocketApiClientOptions? newValues) : base(baseOn, newValues)
        {
            ApiCredentials = (OkxApiCredentials?)newValues?.ApiCredentials?.Copy() ?? (OkxApiCredentials?)baseOn.ApiCredentials?.Copy();
        }
    }

}
