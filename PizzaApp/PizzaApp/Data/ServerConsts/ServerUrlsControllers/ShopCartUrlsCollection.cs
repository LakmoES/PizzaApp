using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzaApp.Data.ServerConsts.ServerUrlsControllers
{
    public class ShopCartUrlsCollection
    {
        private static BaseUrlsController bc = new BaseUrlsController("ShopCart");
        public static string AddProduct => bc.GetFullUrl("AddProduct");
        public static string EditProduct => bc.GetFullUrl("EditProduct");
        public static string Show => bc.GetFullUrl("Show");
        public static string ProductExists => bc.GetFullUrl("ProductExists");
        public static string MakeOrder => bc.GetFullUrl("MakeOrder");
        public static string Clear => bc.GetFullUrl("Clear");
        public static string RemoveProduct => bc.GetFullUrl("RemoveProduct");
        public static string OrderProduct => bc.GetFullUrl("OrderProduct");
    }
}
