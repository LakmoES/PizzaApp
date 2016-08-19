using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.Persistence.Settings
{
    public class ServerURL
    {
        [PrimaryKey]
        public string url { get; set; }
    }
}
