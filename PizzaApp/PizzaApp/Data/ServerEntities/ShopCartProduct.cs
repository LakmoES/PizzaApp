using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.ServerEntities
{
    public class ShopCartProduct
    {
        //"productid","title","measure","category","cost","available","advertising","amount","resultPrice"
        public int productid { get; set; }
        public string title { get; set; }
        public string measure { get; set; }
        public int category { get; set; }
        public Decimal cost { get; set; }
        public int available { get; set; }
        public int advertising { get; set; }
        public int amount { get; set; }
        public Decimal resultPrice { get; set; }
    }
}
