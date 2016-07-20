using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PizzaApp.Data.Persistence;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;

[assembly: Dependency(typeof(PizzaApp.Droid.NotesSQLiteDroid))]
namespace PizzaApp.Droid
{
    public class NotesSQLiteDroid : IDBPlatform
    {
        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var filename = "pizzadb.db3";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, filename);

            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
            var connection = new SQLite.Net.SQLiteConnection(platform, path);
            return connection;
        }
    }
}