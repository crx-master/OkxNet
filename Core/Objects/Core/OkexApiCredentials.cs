using SharpCryptoExchange.Authentication;
using System.Security;

namespace SharpCryptoExchange.Okx.Objects.Core
{
    public class OkxApiCredentials : ApiCredentials
    {
        public SecureString PassPhrase { get; }

        public OkxApiCredentials(string apiKey, string apiSecret, string apiPassPhrase) : base(apiKey, apiSecret)
        {
            PassPhrase = apiPassPhrase.ToSecureString();
        }

        public override ApiCredentials Copy()
        {
            return new OkxApiCredentials(Key!.GetString(), Secret!.GetString(), PassPhrase!.GetString());
        }
    }
}
