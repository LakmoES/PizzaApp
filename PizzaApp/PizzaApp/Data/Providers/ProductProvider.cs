using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PizzaApp.Data.ServerConsts.ServerControllers;

namespace PizzaApp.Data.Providers
{
    public static class ProductProvider
    {
        public static async Task<IEnumerable<Product>> GetProductPage(int page, int pageSize, int category = -1)
        {
            var url = ProductUrlsCollection.GetPage;
            var values = String.Format("?page={0}&pageSize={1}", page, pageSize);
            if (category != -1) values += String.Format("&category={0}", category);

            string content = await Requests.GetAsync(url + values);
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
        public static async Task<IEnumerable<Product>> GetProductPageByName(string name, int page, int pageSize)
        {
            var url = ProductUrlsCollection.GetByName;
            var values = String.Format("?name={0}&page={1}&pageSize={2}", name, page, pageSize);
            string content = await Requests.GetAsync(url + values);
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
        public static async Task<int?> GetProductPageCount(int pageSize, int category = -1)
        {
            var url = ProductUrlsCollection.Pages;
            var values = String.Format("?pageSize={0}",pageSize);
            if (category != -1) values += String.Format("&category={0}", category);

            string content = await Requests.GetAsync(url + values);
            if (content == null)
                return null;
            int count;
            if (!Int32.TryParse(content, out count))
                return null;

            return count;
        }
        public static async Task<IEnumerable<Category>> GetCategories()
        {
            string url = ProductUrlsCollection.CategoryList;
            string content = await Requests.GetAsync(url);
            if (content == null)
                return null;
            JArray jArray;
            try
            {
                jArray = JArray.Parse(content);
            }
            catch { return null; }
            var categoryList = jArray.ToObject<List<Category>>();

            return categoryList;
        }
        public static async Task<string> GetCategoryName(int category)
        {
            var url = ProductUrlsCollection.CategoryList;
            var values = String.Format("?category={0}", category);
            string content = await Requests.GetAsync(url + values);
            if (content == null)
                return null;
            var categoryName = new { name = "" };
            var jEntity = categoryName;
            try
            {
                jEntity = JsonConvert.DeserializeAnonymousType(content, categoryName);
            }
            catch { return null; }

            return jEntity.name;
        }
    }
}
