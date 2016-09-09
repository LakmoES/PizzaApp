using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PizzaApp.Data.Persistence;
using PizzaApp.Data.Providers.ProviderHelpers;
using PizzaApp.Data.ServerConsts.ServerUrlsControllers;

namespace PizzaApp.Data.Providers
{
    public static class ProductProvider
    {
        private static async Task<IEnumerable<T>> GetArrayFromServer<T>(string url)
        {
            string content = await Requests.GetAsync(url);
            if (content == null)
                return null;
            JArray jArray;
            try
            {
                jArray = JArray.Parse(content);
            }
            catch
            {
                return null;
            }
            var array = jArray.ToObject<List<T>>();

            return array;
        }

        public static async Task<IEnumerable<Product>> GetProductPage(int page, int pageSize, int category = -1,
            char? orderBy = null, bool? desc = null)
        {
            var url = ProductUrlsCollection.GetPage;
            var values = string.Format("?page={0}&pageSize={1}", page, pageSize);
            if (category != -1) values += string.Format("&category={0}", category);
            if (orderBy != null) values += string.Format("&orderBy={0}", orderBy);
            if (desc != null) values += string.Format("&desc={0}", (bool) desc ? 1 : 0);

            return await GetArrayFromServer<Product>(url + values);
        }

        public static async Task<IEnumerable<Product>> GetProductPageByName(string name, int page, int pageSize)
        {
            var url = ProductUrlsCollection.GetByName;
            var values = string.Format("?name={0}&page={1}&pageSize={2}", name, page, pageSize);
            return await GetArrayFromServer<Product>(url + values);
        }

        public static async Task<int?> GetProductPageCount(int pageSize, int category = -1)
        {
            var url = ProductUrlsCollection.Pages;
            var values = string.Format("?pageSize={0}", pageSize);
            if (category != -1) values += string.Format("&category={0}", category);

            string content = await Requests.GetAsync(url + values);
            if (content == null)
                return null;
            int count;
            if (!Int32.TryParse(content, out count))
                return null;

            return count;
        }

        public static async Task<int?> GetProductPageByNameCount(string name, int pageSize)
        {
            var url = ProductUrlsCollection.PagesByName;
            var values = string.Format("?pageSize={0}", pageSize);
            if (name != null) values += string.Format("&name={0}", name);

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
            return await GetArrayFromServer<Category>(url);
        }

        public static async Task<string> GetCategoryName(int category)
        {
            var url = ProductUrlsCollection.CategoryList;
            var values = String.Format("?category={0}", category);
            string content = await Requests.GetAsync(url + values);
            if (content == null)
                return null;
            var categoryName = new {name = ""};
            var jEntity = categoryName;
            try
            {
                jEntity = JsonConvert.DeserializeAnonymousType(content, categoryName);
            }
            catch
            {
                return null;
            }

            return jEntity.name;
        }

        public static async Task<ProductImageUrls> GetProductImage(int productID)
        {
            string url = ProductUrlsCollection.GetImagesUrl;
            var values = string.Format("?productID={0}", productID);

            string content = await Requests.GetAsync(url + values);
            if (content == null)
                return null;
            ProductImageUrls productImage;
            try
            {
                productImage = JsonConvert.DeserializeObject<ProductImageUrls>(content);
            }
            catch
            {
                return null;
            }

            return productImage;
        }
    }
}
