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
        public void WriteLog(string text)
        {
            var filename = "errlog.txt";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.GetFullPath(Path.Combine(libraryPath, filename));

            if (!File.Exists(path))
                File.Create(path);
            File.AppendAllText(path, text);
        }
        public string ReadAllLogs()
        {
            var filename = "errlog.txt";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.GetFullPath(Path.Combine(libraryPath, filename));

            if (!File.Exists(path))
                return "";
            return File.ReadAllText(path);
        }
        public void ClearLogFile()
        {
            var filename = "errlog.txt";
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
            var libraryPath = Path.Combine(documentsPath, "..", "Library");
            var path = Path.GetFullPath(Path.Combine(libraryPath, filename));

            if (File.Exists(path))
                File.Delete(path);
        }
    }
}
