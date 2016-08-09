using PizzaApp.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.Providers.ProviderHelpers
{
    public class TokenProviderHelper
    {
        private static async Task<Token> TryGetToken(DBConnection dbc)
        {
            var token = dbc.GetToken();
            if (token == null)
                return null;
            DateTime now = DateTime.Now;
            if (token.expTime.Date == now.Date &&
                token.expTime.TimeOfDay.Hours == now.TimeOfDay.Hours &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes > 0 &&
                token.expTime.TimeOfDay.Minutes - now.TimeOfDay.Minutes <= 5)
            {
                Token t = await AuthProvider.RenewToken(token.token_hash);
                if (t != null)
                {
                    dbc.SaveToken(t);
                    token = t;
                }
            }
            return token;
        }
        private static async Task<Token> TryReLogin(DBConnection dbc)
        {
            var user = dbc.GetUser();
            var token = await AuthProvider.Login(user.username, user.password);
            if (token != null)
            {
                dbc.SaveToken(token);
                return token;
            }
            return null;
        }
        public static async Task<string> TryGetContentPost(DBConnection dbc, string url, Dictionary<string, string> values)
        {
            var token = await TryGetToken(dbc);
            if (token == null)
                return null;
            values.Add("token", token.token_hash);
            string content = await Requests.PostAsync(url, values);
            if (content != null && content.Equals("\"wrong token\""))
            {
                token = await TryReLogin(dbc);
                if (token != null)
                {
                    values["token"] = token.token_hash;
                    content = await Requests.PostAsync(url, values);
                }
            }
            return content;
        }
        public static async Task<string> TryGetContentGet(DBConnection dbc, string url)
        {
            var token = await TryGetToken(dbc);
            if (token == null)
                return null;

            if (url.IndexOf('?') == -1)
                url += "?token=";
            else
                url += "&token=";

            string content = await Requests.GetAsync(url);
            if (content != null && content.Equals("\"wrong token\""))
            {
                token = await TryReLogin(dbc);
                if (token != null)
                {
                    dbc.SaveToken(token);
                    content = await Requests.GetAsync(url + token.token_hash);
                }
            }
            return content;
        }
    }
}
