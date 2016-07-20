using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.Persistence
{
    public class Token
    {
        [PrimaryKey]
        public string token_hash { get; set; }
        public DateTime createTime { get; set; }
        public DateTime expTime { get; set; }
    }
}
