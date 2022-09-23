using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharpCryptoExchange.Okx.Attributes
{
    public static class JsonExtensions
    {
        public static TJToken RemoveFromLowestPossibleParent<TJToken>(this TJToken node) where TJToken : JToken
        {
            if (node == null)
                return null;
            var contained = node.AncestorsAndSelf().Where(t => t.Parent is JContainer && t.Parent.Type != JTokenType.Property).FirstOrDefault();
            if (contained != null)
                contained.Remove();
            // Also detach the node from its immediate containing property -- Remove() does not do this even though it seems like it should
            if (node.Parent is JProperty property)
                property.Value = null;
            return node;
        }

        public static IList<JToken> AsList(this IList<JToken> container) { return container; }
    }

    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
    public class TypedDataAttribute : Attribute
    {
    }

    public class TypedDataConverter<TObject> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(TObject).IsAssignableFrom(objectType);
        }

        private static JsonProperty GetExtensionJsonProperty(JsonObjectContract contract)
        {
            try
            {
                return contract.Properties.Where(p => p.AttributeProvider.GetAttributes(typeof(TypedDataAttribute), false).Any()).Single();
            }
            catch (InvalidOperationException ex)
            {
                throw new JsonSerializationException($"Exactly one property with JsonTypedExtensionDataAttribute is required for type {contract.UnderlyingType}", ex);
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;
            var jObj = JObject.Load(reader);
            var contract = (JsonObjectContract)serializer.ContractResolver.ResolveContract(objectType);
            var extensionJsonProperty = GetExtensionJsonProperty(contract);

            var extensionJProperty = (JProperty)null;
            for (int i = jObj.Count - 1; i >= 0; i--)
            {
                var property = (JProperty)jObj.AsList()[i];
                if (contract.Properties.GetClosestMatchProperty(property.Name) == null)
                {
                    if (extensionJProperty == null)
                    {
                        extensionJProperty = new JProperty(extensionJsonProperty.PropertyName, new JObject());
                        jObj.Add(extensionJProperty);
                    }
                    ((JObject)extensionJProperty.Value).Add(property.RemoveFromLowestPossibleParent());
                }
            }

            var value = existingValue ?? contract.DefaultCreator();
            using (var subReader = jObj.CreateReader())
                serializer.Populate(subReader, value);
            return value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var contract = (JsonObjectContract)serializer.ContractResolver.ResolveContract(value.GetType());
            var extensionJsonProperty = GetExtensionJsonProperty(contract);

            JObject jObj;
            using (new PushValue<bool>(true, () => Disabled, (canWrite) => Disabled = canWrite))
            {
                jObj = JObject.FromObject(value, serializer);
            }

            var extensionValue = (jObj[extensionJsonProperty.PropertyName] as JObject).RemoveFromLowestPossibleParent();
            if (extensionValue != null)
            {
                for (int i = extensionValue.Count - 1; i >= 0; i--)
                {
                    var property = (JProperty)extensionValue.AsList()[i];
                    jObj.Add(property.RemoveFromLowestPossibleParent());
                }
            }

            jObj.WriteTo(writer);
        }

        [ThreadStatic]
        private static bool _disabled;

        // Disables the converter in a thread-safe manner.
        private static bool Disabled { get { return _disabled; } set { _disabled = value; } }

        public override bool CanWrite { get { return !Disabled; } }

        public override bool CanRead { get { return !Disabled; } }
    }

    public struct PushValue<T> : IDisposable
    {
        private readonly Action<T> _setValue;
        private readonly T oldValue;

        public PushValue(T value, Func<T> getValue, Action<T> setValue)
        {
            if (getValue == null) throw new ArgumentNullException(nameof(getValue));
            _setValue = setValue ?? throw new ArgumentNullException(nameof(setValue));
            oldValue = getValue();
            setValue(value);
        }

        #region IDisposable Members

        // By using a disposable struct we avoid the overhead of allocating and freeing an instance of a finalizable class.
        public void Dispose()
        {
            _setValue?.Invoke(oldValue);
        }

        #endregion
    }
}
