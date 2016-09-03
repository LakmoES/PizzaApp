namespace PizzaApp.Data.ServerConsts.ServerUrlsControllers
{
    public class AuthUrlsCollection
    {
        private static BaseUrlsController bc = new BaseUrlsController("Auth");
        public static string Login => bc.GetFullUrl("Login");
        public static string GetNewToken => bc.GetFullUrl("GetNewToken");
        public static string Logout => bc.GetFullUrl("Logout");
        public static string Register => bc.GetFullUrl("RegisterUser");
        public static string NewGuest => bc.GetFullUrl("NewGuest");
        public static string NoMoreGuest => bc.GetFullUrl("NoMoreGuest");
    }
}
