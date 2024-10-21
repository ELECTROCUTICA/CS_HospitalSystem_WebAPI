using System.Text.Json;
using System.Text;

namespace HospitalSystem_WebAPI_dotnet6.Utils {
    public class ChatGPTAPI {

        private const string URL = "https://api.chatanywhere.tech/v1/chat/completions";
        private const string API_KEY = "sk-0FipaHgKpAuhVS0YV45vLDV6fsTDJ5k6ABxbFu7LXKLlLJNm";

        public async static Task<string> SendRequestToChatGPT(string message) { 
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {API_KEY}");

            var requestBody = new {
                model = "gpt-4o-mini",
                messages = new[] {
                    new { role = "system", content = "现在你是一名医院中的导诊员，指导病人应该去看什么科室，以及提供一些医疗建议"},
                    new { role = "user", content = message}
                }
            };

            var json = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

            try {
                var response = await client.PostAsync(URL, json);
                var responseString = await response.Content.ReadAsStringAsync();

                JsonDocument doc = JsonDocument.Parse(responseString);
                string? content = doc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                return content;
            }
            catch (Exception ex) {
                Console.WriteLine(ex.StackTrace);
            }

            return "发生错误";
        }

    }
}
