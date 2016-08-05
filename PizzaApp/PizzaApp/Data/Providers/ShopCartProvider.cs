using Newtonsoft.Json.Linq;
using PizzaApp.Data.Persistence;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.Providers
{
    public class ShopCartProvider
    {
        public static async Task<bool> AddProduct(DBConnection dbc, int productID, int amount)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/ShopCart/AddProduct";
            var token = dbc.GetToken();
            if (token == null)
                return false;
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                    dbc.SaveToken(t);
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash },
                { "productID", productID.ToString()},
                { "amount", amount.ToString() }
            };

            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
            if (content == null)
                return false;
            if (content.Equals("\"ok\""))
                return true;
            else
                return false;
        }
        public static async Task<bool> EditProduct(DBConnection dbc, int productID, int amount)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/ShopCart/EditProduct";
            var token = dbc.GetToken();
            if (token == null)
                return false;
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                    dbc.SaveToken(t);
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash },
                { "productID", productID.ToString()},
                { "amount", amount.ToString() }
            };

            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
            if (content == null)
                return false;
            if (content.Equals("\"ok\""))
                return true;
            else
                return false;
        }

        public static async Task<IEnumerable<ShopCartProduct>> Show(DBConnection dbc, string promocode)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/ShopCart/Show";
            var token = dbc.GetToken();
            if (token == null)
                return null;
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                    dbc.SaveToken(t);
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash }
            };
            if (promocode != null)
                values.Add("promocode", promocode);

            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
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
            string url = "http://lakmoes-001-site1.etempurl.com/ShopCart/ProductExists";
            var token = dbc.GetToken();
            if (token == null)
                return 0;
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                    dbc.SaveToken(t);
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash },
                { "productID", productID.ToString()}
            };

            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
            if (content == null)
                return 0;

            int count;
            if (!Int32.TryParse(content, out count))
                return 0;
            else
                return count;
        }
        public static async Task<int?> MakeOrder(DBConnection dbc, int? addressID, string promocode)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/ShopCart/MakeOrder";
            var token = dbc.GetToken();
            if (token == null)
                return null;
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                    dbc.SaveToken(t);
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash }
            };
            if (addressID != null)
                values.Add("addressID", addressID.ToString());
            if (!String.IsNullOrEmpty(promocode))
                values.Add("promocode", promocode);

            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
            if (content == null)
                return null;
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex("{\"orderNO\":[0-9]+}");
            var match = regex.Match(content);
            if (match.Success)
            {
                return Convert.ToInt32(match.Value.Substring(11, match.Length - 11 - 1));
            }
            else
                return null;
        }
        public static async Task<IEnumerable<ServerError>> Clear(DBConnection dbc)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/ShopCart/Clear";
            var token = dbc.GetToken();
            if (token == null)
                return new List<ServerError> { new ServerError { error = "Возможно, вы не вошли в профиль." } };
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                    dbc.SaveToken(t);
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash }
            };

            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
            if (content == null)
                return new List<ServerError> { new ServerError { error = "Не удалось связаться с сервером." } };
            if (content.Equals("\"ok\""))
                return null;
            else
                return new List<ServerError> { new ServerError { error = "Неизвестная ошибка." } };
        }
        public static async Task<bool> RemoveProduct(DBConnection dbc, int productID)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/ShopCart/RemoveProduct";
            var token = dbc.GetToken();
            if (token == null)
                return false;
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                    dbc.SaveToken(t);
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash },
                { "productID", productID.ToString() }
            };

            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
            if (content == null)
                return false;
            if (content.Equals("\"ok\""))
                return true;
            else
                return false;
        }
    }
}
