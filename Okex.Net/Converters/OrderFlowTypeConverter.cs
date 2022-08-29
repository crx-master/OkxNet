﻿using CryptoExchange.Net.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
{
    internal class OrderFlowTypeConverter : BaseConverter<OkxOrderFlowType>
    {
        public OrderFlowTypeConverter() : this(true) { }
        public OrderFlowTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxOrderFlowType, string>> Mapping => new List<KeyValuePair<OkxOrderFlowType, string>>
        {
            new KeyValuePair<OkxOrderFlowType, string>(OkxOrderFlowType.Taker, "T"),
            new KeyValuePair<OkxOrderFlowType, string>(OkxOrderFlowType.Maker, "M"),
        };
    }
}