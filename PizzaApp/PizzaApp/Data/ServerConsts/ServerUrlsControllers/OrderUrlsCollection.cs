namespace PizzaApp.Data.ServerConsts.ServerUrlsControllers
{
    public class OrderUrlsCollection
    {
        private static BaseUrlsController bc = new BaseUrlsController("Order");
        public static string GetPage { get { return bc.GetFullUrl("GetPage"); } }
        public static string Pages { get { return bc.GetFullUrl("Pages"); } }
    }
}
