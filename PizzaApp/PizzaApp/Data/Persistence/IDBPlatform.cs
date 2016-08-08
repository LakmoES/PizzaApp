using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;

namespace PizzaApp.Data.Persistence
{
    public interface IDBPlatform
    {
        SQLiteConnection GetConnection();
        void WriteLog(string text);
        string ReadAllLogs();
        void ClearLogFile();
    }
}
