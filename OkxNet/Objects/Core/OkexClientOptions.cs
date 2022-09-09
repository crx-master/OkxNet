using CryptoExchangeNet.Interfaces;
using CryptoExchangeNet.Objects;
using System.Collections.Generic;
using System;

namespace OkxNet.Objects.Core
{
    public class OkxClientOptions : BaseRestClientOptions
    {
        public bool DemoTradingService { get; set; } = false;
        public bool SignPublicRequests { get; set; } = false;

        public static OkxClientOptions Default { get; set; } = new OkxClientOptions();


        private OkxRestApiClientOptions _unifiedApiOptions = new OkxRestApiClientOptions(OkxApiAddresses.Default.UnifiedAddress)
        {
            RateLimiters = new List<IRateLimiter>
            {
                new RateLimiter()
                .AddPartialEndpointLimit("/api/v5/trade/order", 60, TimeSpan.FromSeconds(2), null, true, true)
            }
        };

        public new OkxApiCredentials ApiCredentials
        {
            get => (OkxApiCredentials)base.ApiCredentials;
            set => base.ApiCredentials = value;
        }

        public OkxRestApiClientOptions UnifiedApiOptions
        {
            get => _unifiedApiOptions;
            set => _unifiedApiOptions = new OkxRestApiClientOptions(_unifiedApiOptions, value);
        }

        public OkxClientOptions() : this(Default)
        {
        }

        internal OkxClientOptions(OkxClientOptions baseOn) : base(baseOn)
        {
            if (baseOn == null)
                return;

            DemoTradingService = baseOn.DemoTradingService;
            SignPublicRequests = baseOn.SignPublicRequests;

            ApiCredentials = (OkxApiCredentials)baseOn.ApiCredentials?.Copy();
            _unifiedApiOptions = new OkxRestApiClientOptions(baseOn.UnifiedApiOptions, null);
        }
    }

    public class OkxRestApiClientOptions : RestApiClientOptions
    {
        public new OkxApiCredentials ApiCredentials
        {
            get => (OkxApiCredentials)base.ApiCredentials;
            set => base.ApiCredentials = value;
        }

        public OkxRestApiClientOptions()
        {
        }

        internal OkxRestApiClientOptions(string baseAddress) : base(baseAddress)
        {
        }

        internal OkxRestApiClientOptions(OkxRestApiClientOptions baseOn, OkxRestApiClientOptions newValues) : base(baseOn, newValues)
        {
            ApiCredentials = (OkxApiCredentials)newValues?.ApiCredentials?.Copy() ?? (OkxApiCredentials)baseOn.ApiCredentials?.Copy();
        }
    }

}
