using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CdiskClean.agents
{
    internal class AgentRequest<T>
    {
        /// <summary>
        /// 业务数据
        /// </summary>
        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        /// <summary>
        /// 提示词
        /// </summary>
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; } = DefaultPrompt;

        /// <summary>
        /// 返回格式（JSON Schema 字符串）
        /// </summary>
        [JsonPropertyName("returnFormat")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ReturnFormat { get; set; }

        /// <summary>
        /// 序列化时忽略，仅用于本地默认值
        /// </summary>
        [JsonIgnore]
        public const string DefaultPrompt =
            "请严格按照 ReturnFormat 指定的格式返回数据，需要处理的数据在 Data 字段中。";

        public AgentRequest() { }

        [JsonConstructor]
        public AgentRequest(T? data, string? prompt = null, string? returnFormat = null)
        {
            Data = data;
            Prompt = string.IsNullOrWhiteSpace(prompt) ? DefaultPrompt : prompt;
            ReturnFormat = returnFormat;
        }
    }
}
