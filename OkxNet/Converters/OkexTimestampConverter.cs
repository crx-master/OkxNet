using Newtonsoft.Json;
using System;

namespace OkxNet.Converters
{
    /// <summary>
    /// Converter from UNIX milliseconds timestamp to DateTimeOffset (UTC)
    /// </summary>
    internal class OkxTimestampConverter : JsonConverter
    {
        /// <inheritdoc />
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime);
        }

        /// <inheritdoc />
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return null;

            if (reader.Value is string s && string.IsNullOrWhiteSpace(s))
                return null;

            var t = long.Parse(reader.Value.ToString());
            return DateTimeOffset.FromUnixTimeMilliseconds(t);
            //return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(t);
        }

        /// <inheritdoc />
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteValue((DateTimeOffset?)null);
            else
                //writer.WriteValue((long)Math.Round(((DateTime)value - new DateTime(1970, 1, 1)).TotalMilliseconds));
                writer.WriteValue(((DateTimeOffset)value).ToUnixTimeMilliseconds());
        }
    }
}
