using System;
using Xamarin.Forms;
using System.Collections.Generic;
using PizzaApp.Data.Persistence;
using System.IO;
using PizzaApp.Data;
using System.Threading.Tasks;

namespace PizzaApp.Pages
{

	public class OpportunitiesPage : ContentPage
	{
        private DBConnection dbc;
		public OpportunitiesPage (DBConnection dbc)
		{
			Title = "Opportunities";
			Icon = "Opportunities.png";

            this.dbc = dbc;

            User user = null;
            string userInfo;
            try
            {
                user = dbc.GetUser();
                userInfo = user == null ? "empty table" : user.username;
            }
            catch(Exception ex) { userInfo = ex.ToString(); }

            Content = new StackLayout
            {
                Children = { new Label { Text = dbc.db.DatabasePath + Environment.NewLine + userInfo } }
            };
        }
	}
	
}