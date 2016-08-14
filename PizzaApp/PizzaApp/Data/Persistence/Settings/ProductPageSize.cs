using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.Persistence.Settings
{
    public class ProductPageSize
    {
        [PrimaryKey]
        public int pageSize { get; set; }
    }
}
