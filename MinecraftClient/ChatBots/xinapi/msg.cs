using MinecraftClient.Scripting;

namespace MinecraftClient.ChatBots
{
    /// <summary>
    /// Example of message receiving.
    /// </summary>

    public class TestBot : ChatBot
    {
        // 发送POST请求
        private async Task<string> SendPostRequest(string url, string jsonContent)
        {
            try
            {
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return responseBody;
            }
            catch (HttpRequestException e)
            {
                LogToConsole($"请求错误: {e.Message}");
                return null;
            }
        }
        
        public override void Initialize()
        {
            // 异步执行网络请求
            Task.Run(async () =>
            {
                string response = await SendGetRequest("https://api.example.com/data");
                if (response != null)
                {
                    LogToConsole($"收到响应: {response}");
                }
            });
        }
        
    }
}
