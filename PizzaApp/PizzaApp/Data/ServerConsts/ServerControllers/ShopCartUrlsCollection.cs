using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.ServerConsts.ServerControllers
{
    public class ShopCartUrlsCollection
    {
        private static BaseController bc = new BaseController("ShopCart");
        public static string AddProduct { get { return bc.GetFullUrl("AddProduct"); } }
        public static string EditProduct { get { return bc.GetFullUrl("EditProduct"); } }
        public static string Show { get { return bc.GetFullUrl("Show"); } }
        public static string ProductExists { get { return bc.GetFullUrl("ProductExists"); } }
        public static string MakeOrder { get { return bc.GetFullUrl("MakeOrder"); } }
        public static string Clear { get { return bc.GetFullUrl("Clear"); } }
        public static string RemoveProduct { get { return bc.GetFullUrl("RemoveProduct"); } }
    }
}
