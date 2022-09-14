using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class MaintenanceSystemConverter : BaseConverter<OkxMaintenanceSystem>
    {
        public MaintenanceSystemConverter() : this(true) { }
        public MaintenanceSystemConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxMaintenanceSystem, string>> Mapping => new List<KeyValuePair<OkxMaintenanceSystem, string>>
        {
            new KeyValuePair<OkxMaintenanceSystem, string>(OkxMaintenanceSystem.Classic, "classic"),
            new KeyValuePair<OkxMaintenanceSystem, string>(OkxMaintenanceSystem.Unified, "unified"),
        };
    }
}