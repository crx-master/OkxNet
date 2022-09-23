using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class ApiPermissionConverter : BaseConverter<OkxApiPermissions>
    {
        public ApiPermissionConverter() : this(true) { }
        public ApiPermissionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxApiPermissions, string>> Mapping => new List<KeyValuePair<OkxApiPermissions, string>>
        {
            new KeyValuePair<OkxApiPermissions, string>(OkxApiPermissions.ReadOnly, "read_only"),
            new KeyValuePair<OkxApiPermissions, string>(OkxApiPermissions.Trade, "trade"),
        };
    }
}