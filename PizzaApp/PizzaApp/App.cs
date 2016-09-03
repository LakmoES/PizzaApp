using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PizzaApp.Pages;

using Xamarin.Forms;
using PizzaApp.Data.Persistence;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PizzaApp.Data.ServerEntities;
using Newtonsoft.Json.Converters;
using PizzaApp.Data.ServerConsts;

namespace PizzaApp
{
    public class App : Application
    {
        public App()
        {
            DBConnection dbc = new DBConnection();

            ServerAddress.Url = dbc.GetServerURL() ?? "http://asp-lakmoes.tk";

            DependencyService.Get<IDBPlatform>().ClearLogFile();

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
