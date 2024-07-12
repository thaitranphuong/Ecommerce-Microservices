using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuthService.MVC.SyncServices
{
    public class ApiService : IApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly HttpClient client; 

        public ApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;

            client = _httpClientFactory.CreateClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            var response = await client.GetAsync($"{_configuration["Host"]}{url}");
            return response;
        }

        public async Task<HttpResponseMessage> PostAsync(string url, object data)
        {
            var jsonData = new StringContent(
                 JsonConvert.SerializeObject(data),
                 Encoding.UTF8,
                 "application/json");

            var response = await client.PostAsync($"{_configuration["Host"]}{url}", jsonData);
            return response;
        }

        public async Task<HttpResponseMessage> PostFormDataAsync(string url, MultipartFormDataContent formData)
        {
            var response = await client.PostAsync($"{_configuration["Host"]}{url}", formData);
            return response;
        }

        public async Task<HttpResponseMessage> PutAsync(string url, object data)
        {
            var jsonData = new StringContent(
                JsonConvert.SerializeObject(data),
                Encoding.UTF8,
                "application/json");

            var response = await client.PutAsync($"{_configuration["Host"]}{url}", jsonData);
            return response;
        }

        public async Task<HttpResponseMessage> PutFormDataAsync(string url, MultipartFormDataContent formData)
        {
            var response = await client.PutAsync($"{_configuration["Host"]}{url}", formData);
            return response;
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            var response = await client.DeleteAsync($"{_configuration["Host"]}{url}");
            return response;
        }
    }
}
