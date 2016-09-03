namespace PizzaApp.Data.ServerConsts.ServerUrlsControllers
{
    public class OrderUrlsCollection
    {
        private static BaseUrlsController bc = new BaseUrlsController("Order");
        public static string GetPage => bc.GetFullUrl("GetPage");
        public static string Pages => bc.GetFullUrl("Pages");
    }
}
