using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthService.MVC.SyncServices
{
    public interface IApiService
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, object data);
        Task<HttpResponseMessage> PostFormDataAsync(string url, MultipartFormDataContent formData);
        Task<HttpResponseMessage> PutAsync(string url, object data);
        Task<HttpResponseMessage> PutFormDataAsync(string url, MultipartFormDataContent formData);
        Task<HttpResponseMessage> DeleteAsync(string url);
    }
}
