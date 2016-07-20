using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using Xamarin.Forms;

namespace PizzaApp.Data.Persistence
{
    public class DBConnection
    {
        public SQLiteConnection db { private set; get; }
        public DBConnection()
        {
            db = DependencyService.Get<IDBPlatform>().GetConnection();
        }
        public User GetUser()
        {
            var user = db.ExecuteScalar<User>("SELECT * FROM User");
            return user;
        }
        public async Task DBG(string message)
        {
            string x = await Requests.GetAsync("http://localhost:37146/DBG/Write?message=" + message);
            if (x != "ok")
                throw new Exception("");
        }
    }
}
