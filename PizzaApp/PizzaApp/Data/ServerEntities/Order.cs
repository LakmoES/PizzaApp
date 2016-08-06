using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.ServerEntities
{
    public class Order
    {
        public int id { get; set; }
        public string status { get; set; }
        public string promocode { get; set; }
        public int? delivery { get; set; }
        public Decimal cost { get; set; }
        public DateTime date { get; set; }
    }
}
