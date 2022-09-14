using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class ApiPermissionConverter : BaseConverter<OkxApiPermission>
    {
        public ApiPermissionConverter() : this(true) { }
        public ApiPermissionConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxApiPermission, string>> Mapping => new List<KeyValuePair<OkxApiPermission, string>>
        {
            new KeyValuePair<OkxApiPermission, string>(OkxApiPermission.ReadOnly, "read_only"),
            new KeyValuePair<OkxApiPermission, string>(OkxApiPermission.Trade, "trade"),
        };
    }
}