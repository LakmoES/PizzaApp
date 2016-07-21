using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data
{
    public static class Requests
    {
        public static async Task<String> PostAsync(string uri, Dictionary<string, string> data)
        {
            var parameters = new FormUrlEncodedContent(data);
            string content = null;
            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(7000);
                var response = await httpClient.PostAsync(uri, parameters);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
            }
            catch { return null; }

            return await Task.Run(() => content);
        }

        public static async Task<String> GetAsync(string uri)
        {
            string content = null;
            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(7000);
                content = await httpClient.GetStringAsync(uri);
            }
            catch { }
            return await Task.Run(() => content);
        }
    }
}
