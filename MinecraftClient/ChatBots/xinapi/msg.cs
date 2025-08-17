using System;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;
using MinecraftClient.Scripting;

namespace MinecraftClient.ChatBots
{
    /// <summary>
    /// Example of message receiving.
    /// </summary>
    public class UploadBot : ChatBot
    {
        private Timer timer;
        private HttpClient httpClient = new HttpClient();
        
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
            timer = new Timer(async (object stateInfo) => await Playerlist(), null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private async Task Playerlist()
        {
            try
            {
                var data = new
                {
                    list = GetOnlinePlayers()
                };
                string jsonContent = JsonSerializer.Serialize(data);
                await SendPostRequest("https://127.0.0.1/upload/player/list", jsonContent);
            }
            catch (Exception e)
            {
                LogToConsole($"执行错误: {e.Message}");
            }
        }
        
        public override void OnUnload()
        {
            // 清理资源
            timer?.Dispose();
            httpClient?.Dispose();
        }
    }
}