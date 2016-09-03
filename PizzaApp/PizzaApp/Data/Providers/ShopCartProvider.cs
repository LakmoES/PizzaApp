using Newtonsoft.Json.Linq;
using PizzaApp.Data.Persistence;
using PizzaApp.Data.Providers.ProviderHelpers;
using PizzaApp.Data.ServerConsts.ServerUrlsControllers;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaApp.Data.Providers
{
    public class ShopCartProvider
    {
        public static async Task<bool> AddProduct(DBConnection dbc, int productID, int amount)
        {
            string url = ShopCartUrlsCollection.AddProduct;
            
            var values = new Dictionary<string, string>
            {
                { "productID", productID.ToString()},
                { "amount", amount.ToString() }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            return content != null && content.Equals("\"ok\"");
        }
        public static async Task<bool> EditProduct(DBConnection dbc, int productID, int amount)
        {
            string url = ShopCartUrlsCollection.EditProduct;
            
            var values = new Dictionary<string, string>
            {
                { "productID", productID.ToString()},
                { "amount", amount.ToString() }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            return content != null && content.Equals("\"ok\"");
        }

        public static async Task<IEnumerable<ShopCartProduct>> Show(DBConnection dbc, string promocode)
        {
            string url = ShopCartUrlsCollection.Show;
            var values = new Dictionary<string, string>();
            if (promocode != null)
                values.Add("promocode", promocode);

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return null;
            JArray jArray;
            try
            {
                jArray = JArray.Parse(content);
            }
            catch { return null; }
            var shopCartProductList = jArray.ToObject<List<ShopCartProduct>>();

            return shopCartProductList;
        }
        public static async Task<int> ProductExists(DBConnection dbc, int productID)
        {
            string url = ShopCartUrlsCollection.ProductExists;
            var values = new Dictionary<string, string>
            {
                { "productID", productID.ToString()}
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return 0;

            int count;
            return !int.TryParse(content, out count) ? 0 : count;
        }
        public static async Task<int?> MakeOrder(DBConnection dbc, int? addressID, string promocode)
        {
            string url = ShopCartUrlsCollection.MakeOrder;

            var values = new Dictionary<string, string>();
            if (addressID != null)
                values.Add("addressID", addressID.ToString());
            if (!String.IsNullOrEmpty(promocode))
                values.Add("promocode", promocode);

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return null;
            return ParseOrderNo(content);
        }

        public static async Task<int?> OrderProduct(DBConnection dbc, int? addressID, string promocode, int productID, int amountOfProduct)
        {
            string url = ShopCartUrlsCollection.OrderProduct;

            var values = new Dictionary<string, string>
            {
                { "productID", productID.ToString() },
                { "amount", amountOfProduct.ToString()}
            };
            if (addressID != null)
                values.Add("addressID", addressID.ToString());
            if (!string.IsNullOrEmpty(promocode))
                values.Add("promocode", promocode);

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return null;
            return ParseOrderNo(content);
        }
        public static async Task<IEnumerable<ServerError>> Clear(DBConnection dbc)
        {
            string url = ShopCartUrlsCollection.Clear;
            var values = new Dictionary<string, string>();

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return new List<ServerError> { new ServerError { error = "Не удалось связаться с сервером." } };
            if (content.Equals("\"ok\""))
                return null;
            return new List<ServerError> { new ServerError { error = "Неизвестная ошибка." } };
        }
        public static async Task<bool> RemoveProduct(DBConnection dbc, int productID)
        {
            string url = ShopCartUrlsCollection.RemoveProduct;
            var values = new Dictionary<string, string>
            {
                { "productID", productID.ToString() }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            return content != null && content.Equals("\"ok\"");
        }

        private static int? ParseOrderNo(string content)
        {
            var regex = new System.Text.RegularExpressions.Regex("{\"orderNO\":[0-9]+}");
            var match = regex.Match(content);
            if (match.Success)
                return Convert.ToInt32(match.Value.Substring(11, match.Length - 11 - 1));
            return null;
        }
    }
}
