using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.Persistence
{
    [Table("ProductCategory")]
    public class ProductCategory
    {
        [PrimaryKey]
        public int id { get; set; }
        public string title { get; set; }
    }
}
