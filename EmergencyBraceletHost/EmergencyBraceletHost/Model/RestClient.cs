using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyBraceletHost.Model
{
    public class RestClient
    {
        private readonly string _apiUrl = "http://machack7api.cloudapp.net/api/";

        public async Task<T> Get<T>(string path) where T : class
        {
            using (var client = new HttpClient())
            {
                var url = _apiUrl + path;
                var json = await client.GetStringAsync(url);
                var obj = JsonConvert.DeserializeObject<T>(json);
                return obj;
            }
        }

        public async Task Post<T>(string path, T data) where T : class
        {
            using (var client = new HttpClient())
            {
                var json = JsonConvert.SerializeObject(data);
                var url = _apiUrl + path;
                await client.PostAsync(url, new StringContent(json));
            }
        }
    }
}
