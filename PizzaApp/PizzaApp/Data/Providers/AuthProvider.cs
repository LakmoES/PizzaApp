using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PizzaApp.Data.Persistence;
using PizzaApp.Data.ServerConsts.ServerControllers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PizzaApp.Data.Providers
{
    public static class AuthProvider
    {
        public static async Task<Token> Login(string username, string password)
        {
            var url = AuthUrlsCollection.Login;
            var values = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password }
            };

            string content = await Requests.PostAsync(url, values);
            if (content == null)
                return null;
            var token = new { token_hash = "", lifetime = 0 };
            var jEntity = token;
            try
            {
                jEntity = JsonConvert.DeserializeAnonymousType(content, token);
            }
            catch { return null; }

            var createTime = DateTime.Now;
            var expTime = createTime.AddMinutes(jEntity.lifetime);

            return new Token { token_hash = jEntity.token_hash, createTime = createTime, expTime = expTime };
        }
        public static async Task<Token> RenewToken(string token_hash)
        {
            var url = AuthUrlsCollection.GetNewToken;
            var values = new Dictionary<string, string>
            {
                { "token", token_hash }
            };

            string content = await Requests.PostAsync(url, values);
            if (content == null)
                return null;

            var token = new { token_hash = "", lifetime = 0 };
            var jEntity = token;
            try
            {
                jEntity = JsonConvert.DeserializeAnonymousType(content, token);
            }
            catch { return null; }

            var createTime = DateTime.Now;
            var expTime = createTime.AddMinutes(jEntity.lifetime);

            return new Token { token_hash = jEntity.token_hash, createTime = createTime, expTime = expTime };
        }
        public static async Task<String> Logout(string token_hash)
        {
            var url = AuthUrlsCollection.Logout;
            var values = new Dictionary<string, string>
            {
                { "token", token_hash }
            };
            string content = await Requests.PostAsync(url, values);
            return content;
        }
        public static async Task<IEnumerable<ServerError>> Register(string username, string password, string email, string name, string surname)
        {
            var url = AuthUrlsCollection.Register;
            var values = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
                { "email", email },
                { "name", name }
            };
            if (surname != null)
                if (surname.Length > 0)
                    values.Add("surname", surname);

            string content = await Requests.PostAsync(url, values);
            List<ServerError> errorList = null;
            if(content != null)
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
