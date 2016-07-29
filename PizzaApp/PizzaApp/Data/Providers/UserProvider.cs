using Newtonsoft.Json;
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
    public static class UserProvider
    {
        public static async Task<User> GetInfo(DBConnection dbc)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/User/GetInfo";
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
            string url = "http://lakmoes-001-site1.etempurl.com/User/Edit";
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

        public static async Task<List<TelNumber>> GetTelList(DBConnection dbc)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/User/GetTelList?token=";
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

            string content = await Requests.GetAsync(url + token.token_hash);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.GetAsync(url + token.token_hash);
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
            var telList = jArray.ToObject<List<TelNumber>>();

            return telList;
        }
        public static async Task<bool> RemoveTel(DBConnection dbc, int telID)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/User/RemoveTel";
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
                { "telID", telID.ToString() }
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
        public static async Task<bool> AddTel(DBConnection dbc, string tel)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/User/AddTel";
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
                { "tel", tel }
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
        public static async Task<bool> EditTel(DBConnection dbc, int telID, string tel)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/User/EditTel";
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
                { "telID", telID.ToString() },
                { "tel", tel }
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

        public static async Task<List<Address>> GetAddressList(DBConnection dbc)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/User/GetAddressList?token=";
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

            string content = await Requests.GetAsync(url + token.token_hash);
            if (content != null && content.Equals("\"wrong token\""))
            {
                try
                {
                    var user = dbc.GetUser();
                    token = await AuthProvider.Login(user.username, user.password);
                    if (token != null)
                    {
                        dbc.SaveToken(token);
                        content = await Requests.GetAsync(url + token.token_hash);
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
            var addressList = jArray.ToObject<List<Address>>();

            return addressList;
        }
        public static async Task<bool> RemoveAddress(DBConnection dbc, int addressID)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/User/RemoveAddress";
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
                { "addressID", addressID.ToString() }
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
        public static async Task<bool> AddAddress(DBConnection dbc, string address)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/User/AddAddress";
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
                { "address", address }
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
        public static async Task<bool> EditAddress(DBConnection dbc, int addressID, string address)
        {
            string url = "http://lakmoes-001-site1.etempurl.com/User/EditAddress";
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
                { "addressID", addressID.ToString() },
                { "address", address }
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
