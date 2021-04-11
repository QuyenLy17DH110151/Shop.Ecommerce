using Ecommerce.ViewModel.System.Users;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.AdminApp.Service
{
    public class UserAPIClient : IUserAPIClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserAPIClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> Authenticate(LoginRequest request)
        {
            var json = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
            var client = _httpClientFactory.CreateClient();
            client.BaseAddress=new Uri("https://localhost:5001");
            var respone = await client.PostAsync("/api/Users/Authenticate",httpContent);
            var token = await respone.Content.ReadAsStringAsync();
            return token;
        }
    }
}
