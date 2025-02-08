using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ollama.Core.Services
{
    public class ApiService
    {
        private OllamaService _service;
        public async Task<string> SendPostRequest(object data)
        {
            _service = new OllamaService();

            var ipAddress = _service.GetOllamaIpAddress();

            string url = $"http://{ipAddress}:11434/api/chat";

            using (HttpClient client = new HttpClient())
            {
                string json = JsonConvert.SerializeObject(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await client.PostAsync(url, content);

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
