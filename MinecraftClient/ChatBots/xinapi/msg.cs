using System;
using System.Threading;
using System.Text.Json;
using MinecraftClient.Scripting;

namespace MinecraftClient.ChatBots
{
    /// <summary>
    /// Example of message receiving.
    /// </summary>

    public class TestBot : ChatBot
    {
        private Timer timer;
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
            // 创建定时器，每5秒执行一次
            timer = new Timer(Playerlist, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
            // 异步执行网络请求
        }

        private void Playerlist()
        {
            var data = new
            {
                list = GetOnlinePlayers()
            };
            string jsonContent = JsonSerializer.Serialize(data);
            await SendGetRequest("https://127.0.0.1/upload/player/list",jsonContent);
        }
        
    }
}
