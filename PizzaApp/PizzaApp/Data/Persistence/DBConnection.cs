using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using Xamarin.Forms;
using PizzaApp.Data.Persistence.Settings;

namespace PizzaApp.Data.Persistence
{
    public class DBConnection
    {
        public SQLiteConnection db { private set; get; }
        private object locker;
        public DBConnection()
        {
            locker = new object();
            db = DependencyService.Get<IDBPlatform>().GetConnection();
            db.CreateTable<User>();
            db.CreateTable<Token>();
            db.CreateTable<ProductCategory>();
            db.CreateTable<ProductPageSize>();
            db.CreateTable<ServerURL>();
        }
        public User GetUser()
        {
            lock (locker)
            {
                var user = from u in db.Table<User>() select u;
                return user.FirstOrDefault();
                //var user = db.ExecuteScalar<User>("SELECT * FROM User");
                //return user;
            }
        }
        public Token GetToken()
        {
            lock (locker)
            {
                var token = from t in db.Table<Token>() select t;
                return token.FirstOrDefault();
            }
        }
        public string GetServerURL()
        {
            lock (locker)
            {
                var serverAddress = from sa in db.Table<ServerURL>() select sa;
                var foundServerAddress = serverAddress.FirstOrDefault();
                if (foundServerAddress != null)
                    return foundServerAddress.url;
                return null;
            }
        }
        public void SaveUser(User user)
        {
            lock(locker)
            {
                db.Execute("DELETE FROM USER");
                db.Insert(user);
            }
        }
        public void SaveToken(Token token)
        {
            lock(locker)
            {
                db.Execute("DELETE FROM TOKEN");
                db.Insert(token);
            }
        }
        public void SaveServerURL(string url)
        {
            lock (locker)
            {
                db.Execute("DELETE FROM SERVERURL");
                db.Insert(new ServerURL { url = url });
            }
        }
        public void RemoveUser()
        {
            lock (locker)
            {
                db.Execute("DELETE FROM USER");
            }
        }
        public void RemoveToken()
        {
            lock (locker)
            {
                db.Execute("DELETE FROM TOKEN");
            }
        }
        public void SaveCategoryList(List<ProductCategory> categories)
        {
            lock (locker)
            {
                db.Execute("DELETE FROM PRODUCTCATEGORY");
                db.InsertAll(categories);
            }
        }
        public string GetCategoryTitle(int id)
        {
            lock (locker)
            {
                var categoryTitle = from c in db.Table<ProductCategory>() where c.id == id select c.title;
                return categoryTitle.FirstOrDefault();
            }
        }
        public IEnumerable<ProductCategory> GetCategoryList()
        {
            lock(locker)
            {
                return from c in db.Table<ProductCategory>() select c;
            }
        }
        public void SaveProductPageSize(int pageSize)
        {
            lock (locker)
            {
                db.Execute("DELETE FROM PRODUCTPAGESIZE");
                db.Insert(new ProductPageSize { pageSize = pageSize });
            }
        }
        public int? GetProductPageSize()
        {
            lock (locker)
            {
                var pageSize = from ps in db.Table<ProductPageSize>() select ps;
                var foundPageSize = pageSize.FirstOrDefault();
                if (foundPageSize != null)
                    return foundPageSize.pageSize;
                return null;
            }
        }
    }
}
