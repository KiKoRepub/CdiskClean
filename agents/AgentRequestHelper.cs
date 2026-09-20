using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CdiskClean.agents
{
    public class AgentRequestHelper
    {

        private static string BASE_URL = "https://api.cdiskclean.com/agents";

        private static string AgentName = "default"; // 默认的 Agent 名称，可以根据需要修改

        public static async Task<T> sendAgentRequest<T>(string prompt, object? data = null, string? returnFormat = null)
        {
            var request = new AgentRequest<object>(data, prompt, returnFormat);
            // 这里可以添加发送请求的逻辑，例如调用 API 或其他处理
            // 假设我们有一个方法 SendRequestAsync 来发送请求并获取响应
            // var response = await SendRequestAsync(request);
            // return response;
            throw new NotImplementedException("SendAgentRequest method is not implemented yet.");
        }

        public static async Task<AgentUsage> getAgentUsage()
        {
            throw new NotImplementedException("GetAgentUsage method is not implemented yet.");
        }


    }
}
