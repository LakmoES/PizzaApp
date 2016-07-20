using Newtonsoft.Json;
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
    }
}
