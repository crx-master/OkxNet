using CryptoExchangeNet.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
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