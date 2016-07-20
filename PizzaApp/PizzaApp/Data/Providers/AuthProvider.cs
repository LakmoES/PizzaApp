using Newtonsoft.Json.Linq;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.Providers
{
    class AuthProvider
    {
        public static async Task<List<Product>> GetProductPage(int page, int pageSize, int category = -1)
        {
            //var values = new Dictionary<string, string>
            //{
            //    { "page", page.ToString() },
            //    { "pageSize", pageSize.ToString() }
            //};
            var values = String.Format("?page={0}&pageSize={1}", page, pageSize);
            if (category != -1) values += String.Format("&category={0}", category);

            string content = await Requests.GetAsync("http://lakmoes-001-site1.etempurl.com/Product/GetPage" + values);
            if (content == null)
                return null;
            JArray jArray;
            try
            {
                jArray = JArray.Parse(content);
            }
            catch { return null; }
            var productList = jArray.ToObject<List<Product>>();

            return productList;
        }
    }
}
