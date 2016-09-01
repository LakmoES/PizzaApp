using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PizzaApp.Data.Persistence;
using System.IO;

namespace PizzaAppMVVM.Persistence
{
    public class SQLiteDroid : IDBPlatform
    {
        private static string Path(string filename)
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = System.IO.Path.Combine(documentsPath, filename);
            return path;
        }


        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var path = Path("pizzadb.db3");

            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var connection = new SQLite.Net.SQLiteConnection(platform, path);
            return connection;
        }

        public void WriteLog(string text)
        {
            var path = Path("errlog.txt");

            if (!File.Exists(path))
                File.Create(path);
            File.AppendAllText(path, text);
        }
        public string ReadAllLogs()
        {
            var path = Path("errlog.txt");

            if (!File.Exists(path))
                return "";
            return File.ReadAllText(path);
        }
        public void ClearLogFile()
        {
            var path = Path("errlog.txt");

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}