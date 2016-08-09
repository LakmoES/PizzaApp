namespace PizzaApp.Data.ServerConsts.ServerControllers
{
    public class OrderUrlsCollection
    {
        private static BaseController bc = new BaseController("Order");
        public static string GetPage { get { return bc.GetFullUrl("GetPage"); } }
        public static string Pages { get { return bc.GetFullUrl("Pages"); } }
    }
}
