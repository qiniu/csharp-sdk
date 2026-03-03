using System;
using System.Text.Json;

namespace Qiniu.Util
{
    internal static class QiniuJson
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        public static T? Deserialize<T>(string? json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonSerializer.Deserialize<T>(json, SerializerOptions);
        }

        public static string Serialize<T>(T value)
        {
            ArgumentNullException.ThrowIfNull(value);
            return JsonSerializer.Serialize(value, SerializerOptions);
        }
    }
}
