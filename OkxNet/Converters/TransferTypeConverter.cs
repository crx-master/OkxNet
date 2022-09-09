using CryptoExchangeNet.Converters;
using OkxNet.Enums;
using System.Collections.Generic;

namespace OkxNet.Converters
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