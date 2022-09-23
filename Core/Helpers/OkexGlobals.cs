using System.Globalization;

namespace SharpCryptoExchange.Okx.Helpers
{
    public static class OkxGlobals
    {
        // Local Settings
        public readonly static CultureInfo OkxCultureInfo = new CultureInfo("en-US");
        public readonly static string OkxDatetimeFormat = "yyyy-MM-ddTHH:mm:ss.fffZ";
    }
}
