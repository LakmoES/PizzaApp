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
        private static readonly int timeout = 9000;
        public static async Task<String> PostAsync(string uri, Dictionary<string, string> data)
        {
            var parameters = new FormUrlEncodedContent(data);
            string content = null;
            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
                var response = await httpClient.PostAsync(uri, parameters);
                response.EnsureSuccessStatusCode();
                content = await response.Content.ReadAsStringAsync();
            }
            catch { return null; }

            return content;
        }

        public static async Task<String> GetAsync(string uri)
        {
            string content = null;
            try
            {
                var httpClient = new HttpClient();
                httpClient.Timeout = TimeSpan.FromMilliseconds(timeout);
                content = await httpClient.GetStringAsync(uri);
            }
            catch { }
            return content;
        }
    }
}
