namespace PizzaApp.Data.ServerConsts.ServerUrlsControllers
{
    public class UserUrlsCollection
    {
        private static BaseUrlsController bc = new BaseUrlsController("User");
        public static string GetInfo => bc.GetFullUrl("GetInfo");
        public static string Edit => bc.GetFullUrl("Edit");
        public static string GetTelList => bc.GetFullUrl("GetTelList");
        public static string RemoveTel => bc.GetFullUrl("RemoveTel");
        public static string AddTel => bc.GetFullUrl("AddTel");
        public static string EditTel => bc.GetFullUrl("EditTel");
        public static string GetAddressList => bc.GetFullUrl("GetAddressList");
        public static string RemoveAddress => bc.GetFullUrl("RemoveAddress");
        public static string AddAddress => bc.GetFullUrl("AddAddress");
        public static string EditAddress => bc.GetFullUrl("EditAddress");
    }
}
