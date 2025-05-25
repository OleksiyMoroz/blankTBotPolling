using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace src.Services
{
    public class ApiService
    {
        private readonly HttpClient _client;
        public ApiService(IHttpClientFactory httpClientFactory) {
            _client = httpClientFactory.CreateClient("api");
        }

        public async Task<string> GetApiResponse(string p) {
            try {
                var response = await _client.GetAsync(_client.BaseAddress + p);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch {
                return string.Empty;
            }
        }
    }
}