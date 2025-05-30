using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MyMessagerClient
{
    public class MessageService
    {
        private readonly HttpClient _httpClient;
        private const string ApiUrl = "https://localhost:7013/api/messages";

        public MessageService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Message>> GetMessagesAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Message>>(ApiUrl)
                    ?? new List<Message>();
            }
            catch
            {
                return new List<Message>();
            }
        }

        public async Task<bool> SendMessageAsync(string message, string token)
        {
            try
            {
                var request = new
                {
                    Message = message,
                    Token = token
                };

                var response = await _httpClient.PostAsJsonAsync(ApiUrl, request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }

    public class Message
    {
        public Guid Id { get; set; }
        public string Text { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}