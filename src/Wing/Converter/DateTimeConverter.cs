using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Wing.Converter
{
    public class DateTimeConverter : DateTimeConverterBase
    {
        private readonly DateTime greenwichMeanTime;

        public DateTimeConverter()
        {
            greenwichMeanTime = TimeZoneInfo.ConvertTime(new DateTime(1970, 1, 1), TimeZoneInfo.Local);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(DateTime) || objectType == typeof(DateTime?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                return null;
            }

            if (CanConvert(objectType))
            {
                if (string.IsNullOrEmpty(reader.Value?.ToString()))
                {
                    return reader.Value;
                }

                if (reader.Value is string || reader.TokenType == JsonToken.Date)
                {
                    if (DateTime.TryParse(reader.Value.ToString(), out DateTime dt))
                    {
                        return dt;
                    }

                    return reader.Value;
                }

                var ratio = reader.Value.ToString().Length == 10 ? 10000000 : 10000;
                return new DateTime(greenwichMeanTime.Ticks + (Convert.ToInt64(reader.Value) * ratio)).ToLocalTime();
            }

            return reader.Value;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            long val;
            if (value.GetType() == typeof(DateTime))
            {
                DateTime dt = Convert.ToDateTime(value);
                val = (dt.ToUniversalTime().Ticks - greenwichMeanTime.Ticks) / 10000000;
            }
            else
            {
                val = Convert.ToInt64(value);
            }

            writer.WriteValue(val);
        }
    }
}
