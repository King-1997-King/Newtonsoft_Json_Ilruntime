using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Newtonsoft.Json.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class EnumerableVectorConverter<T>: JsonConverter
    {
        private static readonly VectorConverter VectorConverter = new VectorConverter();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="value"></param>
        /// <param name="serializer"></param>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
                writer.WriteNull();

            T[] src = (value as IEnumerable<T>)?.ToArray();

            if (src == null)
            {
                writer.WriteNull();
                return;
            }

            writer.WriteStartArray();

            for (var i = 0; i < src.Length; i++)
            {
                VectorConverter.WriteJson(writer, src[i], serializer);
            }
            writer.WriteEndArray();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(IEnumerable<Vector2>).IsAssignableFrom(objectType)
                            || typeof(IEnumerable<Vector3>).IsAssignableFrom(objectType)
                            || typeof(IEnumerable<Vector4>).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            var result = new List<T>();

            var obj = JObject.Load(reader);

            for (var i = 0; i < obj.Count; i++)
                result.Add(JsonConvert.DeserializeObject<T>(obj[i].ToString()));

            return result;
        }

        public override bool CanRead
        {
            get { return true; }
        }
    }
}
