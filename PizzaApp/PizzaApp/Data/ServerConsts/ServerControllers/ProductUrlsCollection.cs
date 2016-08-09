using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace PizzaApp.Data.ServerConsts.ServerControllers
{
    public class ProductUrlsCollection
    {
        private static BaseController bc = new BaseController("Product");
        public static string GetPage { get { return bc.GetFullUrl("GetPage"); } }
        public static string GetByName { get { return bc.GetFullUrl("GetByName"); } }
        public static string Pages { get { return bc.GetFullUrl("Pages"); } }
        public static string CategoryList { get { return bc.GetFullUrl("GetCategoryList"); } }
    }
}
