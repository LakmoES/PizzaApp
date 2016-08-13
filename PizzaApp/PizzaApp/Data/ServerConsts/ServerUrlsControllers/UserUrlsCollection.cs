namespace PizzaApp.Data.ServerConsts.ServerUrlsControllers
{
    public class UserUrlsCollection
    {
        private static BaseUrlsController bc = new BaseUrlsController("User");
        public static string GetInfo { get { return bc.GetFullUrl("GetInfo"); } }
        public static string Edit { get { return bc.GetFullUrl("Edit"); } }
        public static string GetTelList { get { return bc.GetFullUrl("GetTelList"); } }
        public static string RemoveTel { get { return bc.GetFullUrl("RemoveTel"); } }
        public static string AddTel { get { return bc.GetFullUrl("AddTel"); } }
        public static string EditTel { get { return bc.GetFullUrl("EditTel"); } }
        public static string GetAddressList { get { return bc.GetFullUrl("GetAddressList"); } }
        public static string RemoveAddress { get { return bc.GetFullUrl("RemoveAddress"); } }
        public static string AddAddress { get { return bc.GetFullUrl("AddAddress"); } }
        public static string EditAddress { get { return bc.GetFullUrl("EditAddress"); } }
    }
}
