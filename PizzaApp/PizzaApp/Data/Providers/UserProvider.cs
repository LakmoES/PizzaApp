using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PizzaApp.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.Providers
{
    public static class UserProvider
    {
        public static async Task<User> GetInfo(DBConnection dbc)
        {
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
                { "token",  token.token_hash }
            };
            
            string content = await Requests.PostAsync("http://lakmoes-001-site1.etempurl.com/User/GetInfo", values);
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
                        content = await Requests.PostAsync("http://lakmoes-001-site1.etempurl.com/User/Edit", values);
                    }
                }
                catch { }
            }
            if (content == null)
                return null;
            var userInfo = new { username = "", name = "", surname = "", email = "", guest = 0 };
            var jEntity = userInfo;
            try
            {
                jEntity = JsonConvert.DeserializeAnonymousType(content, userInfo);
            }
            catch { return null; }

            return new User { username = jEntity.username, name = jEntity.name, surname = jEntity.surname, email = jEntity.email, guest = jEntity.guest };
        }
        public static async Task<List<ServerError>> Edit(DBConnection dbc, string password, string email, string name, string surname)
        {
            var token = dbc.GetToken();
            if (token == null)
                return new List<ServerError> { new ServerError { error = "Вы не залогинены" } };

            var values = new Dictionary<string, string>
            {
                { "token", token.token_hash },
                { "email", email },
                { "name", name }
            };
            if (password != null)
                if (password.Length > 0)
                    values.Add("password", password);
            if (surname != null)
                if (surname.Length > 0)
                    values.Add("surname", surname);

            string content = await Requests.PostAsync("http://lakmoes-001-site1.etempurl.com/User/Edit", values);
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
                        content = await Requests.PostAsync("http://lakmoes-001-site1.etempurl.com/User/Edit", values);
                    }
                }
                catch { }
            }
            List<ServerError> errorList = null;
            if (content != null)
            {
                JArray jArray;
                try
                {
                    jArray = JArray.Parse(content);
                }
                catch { return null; }
                try
                {
                    errorList = jArray.ToObject<List<ServerError>>();
                }
                catch { return new List<ServerError> { { new ServerError { error = "Failed to parse Json" } }, { new ServerError { error = content } } }; }
            }
            return errorList;
        }
    }
}
