using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyMessagerClient
{
    public class AuthService
    {
        private readonly HttpClient _httpClient;
        private const string AuthApiUrl = "https://localhost:7013/api/auth/";

        public AuthService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> LoginAsync(string username, string password)
        {
            var request = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync($"{AuthApiUrl}login", request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Authorization error");

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result.Token;
        }

        public async Task<string> RegisterAsync(string username, string password)
        {
            var request = new { Username = username, Password = password };
            var response = await _httpClient.PostAsJsonAsync($"{AuthApiUrl}register", request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Registration error");

            var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
            return result.Token;
        }

        private class AuthResponse
        {
            public string Token { get; set; }
        }
    }
}