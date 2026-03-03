using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Qiniu.Storage;

namespace Qiniu.Util
{
    internal static class QiniuJson
    {
        public static T? Deserialize<T>(string? json, JsonTypeInfo<T> typeInfo)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonSerializer.Deserialize(json, typeInfo);
        }

        public static string Serialize<T>(T value, JsonTypeInfo<T> typeInfo)
        {
            ArgumentNullException.ThrowIfNull(value);
            return JsonSerializer.Serialize(value, typeInfo);
        }
    }

    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonSerializable(typeof(ResumeInfo))]
    [JsonSerializable(typeof(ResumeContext))]
    [JsonSerializable(typeof(ZoneInfo))]
    [JsonSerializable(typeof(PutPolicy))]
    [JsonSerializable(typeof(FileInfo))]
    [JsonSerializable(typeof(Dictionary<string, string>))]
    [JsonSerializable(typeof(Dictionary<string, object>))]
    [JsonSerializable(typeof(Dictionary<string, object>[]))]
    internal partial class QiniuJsonSerializerContext : JsonSerializerContext
    {
    }
}
