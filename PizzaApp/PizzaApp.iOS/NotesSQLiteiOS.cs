using PizzaApp.Data.Persistence;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;

[assembly: Dependency(typeof(PizzaApp.iOS.NotesSQLiteiOS))]
namespace PizzaApp.iOS
{
    public class NotesSQLiteiOS : IDBPlatform
    {
        public SQLite.Net.SQLiteConnection GetConnection()
        {
            var filename = "pizzadb.db3";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.GetFullPath(Path.Combine(libraryPath, filename));

            var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
            var connection = new SQLite.Net.SQLiteConnection(platform, path);
            return connection;
        }
    }
}
