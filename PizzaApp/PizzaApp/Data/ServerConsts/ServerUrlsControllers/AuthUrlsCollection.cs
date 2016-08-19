﻿namespace PizzaApp.Data.ServerConsts.ServerUrlsControllers
{
    public class AuthUrlsCollection
    {
        private static BaseUrlsController bc = new BaseUrlsController("Auth");
        public static string Login { get { return bc.GetFullUrl("Login"); } }
        public static string GetNewToken { get { return bc.GetFullUrl("GetNewToken"); } }
        public static string Logout { get { return bc.GetFullUrl("Logout"); } }
        public static string Register { get { return bc.GetFullUrl("RegisterUser"); } }
        public static string NewGuest { get { return bc.GetFullUrl("NewGuest"); } }
        public static string NoMoreGuest { get { return bc.GetFullUrl("NoMoreGuest"); } }
    }
}