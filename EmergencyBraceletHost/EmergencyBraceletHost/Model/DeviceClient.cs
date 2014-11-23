using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EmergencyBraceletHost.Model
{
    public class DeviceClient
    {
        private readonly string _deviceUrl = "https://api.spark.io/v1/devices/{0}/{1}?access_token={2}";

        public async Task<string> Get(string deviceID, string accessToken, string path)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(_deviceUrl, deviceID, path, accessToken);
                var json = await client.GetStringAsync(url);
                var obj = JsonConvert.DeserializeObject<DeviceResponse>(json);
                return obj.Result;
            }
        }

        public async Task Post(string deviceID, string accessToken, string path)
        {
            using (var client = new HttpClient())
            {
                var url = string.Format(_deviceUrl, deviceID, path, accessToken);
                await client.PostAsync(url, new StringContent(string.Empty));
            }
        }
    }
}
