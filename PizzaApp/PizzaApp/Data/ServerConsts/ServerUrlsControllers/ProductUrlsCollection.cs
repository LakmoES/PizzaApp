using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace PizzaApp.Data.ServerConsts.ServerUrlsControllers
{
    public class ProductUrlsCollection
    {
        private static BaseUrlsController bc = new BaseUrlsController("Product");
        public static string GetPage => bc.GetFullUrl("GetPage");
        public static string GetByName => bc.GetFullUrl("GetByName");
        public static string Pages => bc.GetFullUrl("Pages");
        public static string CategoryList => bc.GetFullUrl("GetCategoryList");
        public static string PagesByName => bc.GetFullUrl("PagesByName");
        public static string GetImagesUrl => bc.GetFullUrl("GetImagesUrl");
    }
}
