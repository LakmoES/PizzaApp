using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PizzaApp.Data.Persistence;
using PizzaApp.Data.Providers.ProviderHelpers;
using PizzaApp.Data.ServerConsts.ServerUrlsControllers;
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
            string url = UserUrlsCollection.GetInfo;
            var values = new Dictionary<string, string>();

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
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
        public static async Task<IEnumerable<ServerError>> Edit(DBConnection dbc, string password, string email, string name, string surname)
        {
            string url = UserUrlsCollection.Edit;
            var values = new Dictionary<string, string>();

            if (!String.IsNullOrEmpty(email))
                values.Add("email", email);
            if (!String.IsNullOrEmpty(name))
                values.Add("name", name);
            if (password != null)
                if (password.Length > 0)
                    values.Add("password", password);
            if (surname != null)
                if (surname.Length > 0)
                    values.Add("surname", surname);

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
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

        public static async Task<IEnumerable<TelNumber>> GetTelList(DBConnection dbc)
        {
            string url = UserUrlsCollection.GetTelList;
            string content = await TokenProviderHelper.TryGetContentGet(dbc, url);
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
            string url = UserUrlsCollection.RemoveTel;
            var values = new Dictionary<string, string>
            {
                { "telID", telID.ToString() }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return false;
            if (content.Equals("\"ok\""))
                return true;
            else
                return false;
        }
        public static async Task<bool> AddTel(DBConnection dbc, string tel)
        {
            string url = UserUrlsCollection.AddTel;
            var values = new Dictionary<string, string>
            {
                { "tel", tel }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return false;
            if (content.Equals("\"ok\""))
                return true;
            else
                return false;
        }
        public static async Task<bool> EditTel(DBConnection dbc, int telID, string tel)
        {
            string url = UserUrlsCollection.EditTel;
            var values = new Dictionary<string, string>
            {
                { "telID", telID.ToString() },
                { "tel", tel }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return false;
            if (content.Equals("\"ok\""))
                return true;
            else
                return false;
        }

        public static async Task<IEnumerable<Address>> GetAddressList(DBConnection dbc)
        {
            string url = UserUrlsCollection.GetAddressList;
            string content = await TokenProviderHelper.TryGetContentGet(dbc, url);
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
            string url = UserUrlsCollection.RemoveAddress;
            var values = new Dictionary<string, string>
            {
                { "addressID", addressID.ToString() }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return false;
            if (content.Equals("\"ok\""))
                return true;
            else
                return false;
        }
        public static async Task<bool> AddAddress(DBConnection dbc, string address)
        {
            string url = UserUrlsCollection.AddAddress;
            var values = new Dictionary<string, string>
            {
                { "address", address }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return false;
            if (content.Equals("\"ok\""))
                return true;
            else
                return false;
        }
        public static async Task<bool> EditAddress(DBConnection dbc, int addressID, string address)
        {
            string url = UserUrlsCollection.EditAddress;
            var values = new Dictionary<string, string>
            {
                { "addressID", addressID.ToString() },
                { "address", address }
            };

            string content = await TokenProviderHelper.TryGetContentPost(dbc, url, values);
            if (content == null)
                return false;
            if (content.Equals("\"ok\""))
                return true;
            else
                return false;
        }
    }
}
