using SharpCryptoExchange.Converters;
using SharpCryptoExchange.Okx.Enums;
using System.Collections.Generic;

namespace SharpCryptoExchange.Okx.Converters
{
    internal class TransferTypeConverter : BaseConverter<OkxTransferType>
    {
        public TransferTypeConverter() : this(true) { }
        public TransferTypeConverter(bool quotes) : base(quotes) { }

        protected override List<KeyValuePair<OkxTransferType, string>> Mapping => new List<KeyValuePair<OkxTransferType, string>>
        {
            new KeyValuePair<OkxTransferType, string>(OkxTransferType.TransferWithinAccount, "0"),
            new KeyValuePair<OkxTransferType, string>(OkxTransferType.MasterAccountToSubAccount, "1"),
            new KeyValuePair<OkxTransferType, string>(OkxTransferType.SubAccountToMasterAccount, "2"),
        };
    }
}