using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.IO;

namespace Niceify.Services
{
    public class AiService
    {
        private readonly string _apiKey;
        private readonly string _model;
        private readonly string _provider;

        public AiService()
        {
            var config = JsonSerializer.Deserialize<AiConfig>(
                File.ReadAllText("apiSettings.json"),
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            _apiKey = config.ApiKey;
            _model = config.Model;
            _provider = config.Provider;
        }

        public async Task<string> RewriteTextAsync(string input, string tone, string customPrompt)
        {
            var prompt = string.IsNullOrWhiteSpace(customPrompt)
                ? $"Rewrite the following in a {tone} tone: {input}"
                : customPrompt;

            var requestBody = new
            {
                model = _model,
                messages = new[] {
                    new { role = "system", content = "You are a helpful tone-adjusting assistant." },
                    new { role = "user", content = prompt }
                }
            };

            var http = new HttpClient();
            http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
            if (_provider == "openrouter")
                http.DefaultRequestHeaders.Add("referer", "https://niceify.local");

            var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
            var response = await http.PostAsync("https://openrouter.ai/api/v1/chat/completions", content);
            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            return doc.RootElement
                .GetProperty("choices")[0]
                .GetProperty("message")
                .GetProperty("content")
                .GetString() ?? "[Error]";
        }

        private class AiConfig
        {
            public string Provider { get; set; }
            public string ApiKey { get; set; }
            public string Model { get; set; }
        }
    }
}
