using Newtonsoft.Json.Linq;
using PizzaApp.Data.Persistence;
using PizzaApp.Data.ServerConsts.ServerControllers;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PizzaApp.Data.Providers.ProviderHelpers;

namespace PizzaApp.Data.Providers
{
    public class OrderProvider
    {
        
        public static async Task<IEnumerable<Order>> GetPage(DBConnection dbc, int page, int pageSize)
        {
            string url = OrderUrlsCollection.GetPage;

            var values = new Dictionary<string, string>
            {
                { "page", page.ToString() },
                { "pageSize", pageSize.ToString() }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return null;
            JArray jArray;
            try
            {
                jArray = JArray.Parse(content);
            }
            catch { return null; }
            var ordersList = jArray.ToObject<List<Order>>();

            return ordersList;
        }
        public static async Task<int?> GetPageCount(DBConnection dbc, int pageSize)
        {
            string url = OrderUrlsCollection.Pages;

            var values = new Dictionary<string, string>
            {
                { "pageSize", pageSize.ToString() }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return null;
            int count;
            if (!Int32.TryParse(content, out count))
                return null;

            return count;
        }
    }
}
