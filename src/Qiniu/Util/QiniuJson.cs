using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Qiniu.CDN;
using Qiniu.Storage;

namespace Qiniu.Util
{
    internal static class QiniuJson
    {
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };

        internal static readonly QiniuJsonSerializerContext SerializerContext = new(SerializerOptions);

        public static T? Deserialize<T>(string? json, JsonTypeInfo<T> typeInfo)
        {
            ArgumentNullException.ThrowIfNull(typeInfo);

            if (string.IsNullOrWhiteSpace(json))
            {
                return default;
            }

            return JsonSerializer.Deserialize(json, typeInfo);
        }

        public static string Serialize<T>(T value, JsonTypeInfo<T> typeInfo)
        {
            ArgumentNullException.ThrowIfNull(value);
            ArgumentNullException.ThrowIfNull(typeInfo);
            return JsonSerializer.Serialize(value, typeInfo);
        }
    }

    [JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true, DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonSerializable(typeof(ResumeInfo))]
    [JsonSerializable(typeof(ResumeContext))]
    [JsonSerializable(typeof(ZoneInfo))]
    [JsonSerializable(typeof(PutPolicy))]
    [JsonSerializable(typeof(FileInfo))]
    [JsonSerializable(typeof(BandwidthInfo))]
    [JsonSerializable(typeof(BandwidthRequest))]
    [JsonSerializable(typeof(FluxInfo))]
    [JsonSerializable(typeof(FluxRequest))]
    [JsonSerializable(typeof(LogListInfo))]
    [JsonSerializable(typeof(LogListRequest))]
    [JsonSerializable(typeof(PrefetchInfo))]
    [JsonSerializable(typeof(PrefetchRequest))]
    [JsonSerializable(typeof(RefreshInfo))]
    [JsonSerializable(typeof(RefreshRequest))]
    [JsonSerializable(typeof(BatchInfo))]
    [JsonSerializable(typeof(List<BatchInfo>))]
    [JsonSerializable(typeof(BucketInfo))]
    [JsonSerializable(typeof(FetchInfo))]
    [JsonSerializable(typeof(ListInfo))]
    [JsonSerializable(typeof(PfopInfo))]
    [JsonSerializable(typeof(List<string>))]
    [JsonSerializable(typeof(Dictionary<string, string>))]
    [JsonSerializable(typeof(Dictionary<string, object>))]
    [JsonSerializable(typeof(Dictionary<string, object>[]))]
    internal partial class QiniuJsonSerializerContext : JsonSerializerContext
    {
    }
}
