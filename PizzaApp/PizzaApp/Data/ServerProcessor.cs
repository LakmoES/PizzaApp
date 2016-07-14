using Newtonsoft.Json.Linq;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data
{
    public static class ServerProcessor
    {
        public static async Task<List<Product>> GetProducts(int page, int pageSize)
        {
            //var values = new Dictionary<string, string>
            //{
            //    { "page", page.ToString() },
            //    { "pageSize", pageSize.ToString() }
            //};
            var values = String.Format("?page={0}&pageSize={1}", page, pageSize);

            string content = await ServerProcessor.GetAsync("http://lakmoes-001-site1.etempurl.com/Product/GetPage" + values);
            if (content == null)
                return null;
            var jArray = JArray.Parse(content);
            var productList = jArray.ToObject<List<Product>>();

            return productList;
        }
        private static async Task<String> PostAsync(string uri, Dictionary<string, string> data)
        {
            var parameters = new FormUrlEncodedContent(data);

            var httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMilliseconds(7000);
            var response = await httpClient.PostAsync(uri, parameters);

            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
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
