using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class DeliveryExerciseHistoryTypeConverter : BaseConverter<OkxDeliveryExerciseHistoryType>
    {
        public DeliveryExerciseHistoryTypeConverter() : this(true) { }
        public DeliveryExerciseHistoryTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxDeliveryExerciseHistoryType, string>> Mapping => new List<KeyValuePair<OkxDeliveryExerciseHistoryType, string>>
        {
            new KeyValuePair<OkxDeliveryExerciseHistoryType, string>(OkxDeliveryExerciseHistoryType.Delivery, "delivery"),
            new KeyValuePair<OkxDeliveryExerciseHistoryType, string>(OkxDeliveryExerciseHistoryType.Exercised, "exercised"),
            new KeyValuePair<OkxDeliveryExerciseHistoryType, string>(OkxDeliveryExerciseHistoryType.ExpiredOtm, "expired_otm"),
        };
    }
}