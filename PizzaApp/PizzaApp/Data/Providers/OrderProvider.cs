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
    public class OrderProvider
    {
        public static async Task<IEnumerable<Order>> GetPage(DBConnection dbc, int page, int pageSize)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/Order/GetPage";
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
                {
                    dbc.SaveToken(t);
                    token = t;
                }
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash },
                { "page", page.ToString() },
                { "pageSize", pageSize.ToString() }
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
                        values["token"] = token.token_hash;
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
            var ordersList = jArray.ToObject<List<Order>>();

            return ordersList;
        }
        public static async Task<int?> GetPageCount(DBConnection dbc, int pageSize)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/Order/Pages";
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
                {
                    dbc.SaveToken(t);
                    token = t;
                }
            }

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash },
                { "pageSize", pageSize.ToString() }
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
                        values["token"] = token.token_hash;
                        content = await Requests.PostAsync(url, values);
                    }
                }
                catch { }
            }
            if (content == null)
                return null;
            int count;
            if (!Int32.TryParse(content, out count))
                return null;

            return count;
        }
    }
}
