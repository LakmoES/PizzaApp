using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PizzaApp.Pages;

using Xamarin.Forms;
using PizzaApp.Data.Persistence;

namespace PizzaApp
{
    public class App : Application
    {
        public App()
        {
            DBConnection dbc = new DBConnection();

            MainPage = new RootPage(dbc);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
