using SQLite;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.Persistence
{
    [Table("User")]
    public class User
    {
        [PrimaryKey]
        public string username { get; set; }
        public string password { get; set; }
    }
}
