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
		public OpportunitiesPage ()
		{
			Title = "Opportunities";
			Icon = "Opportunities.png";

            DBConnection dbc = new DBConnection();
            
            //User user = dbc.GetUser();

            Content = new StackLayout
            {
                Children = { new Label { Text = dbc.db.DatabasePath } }
            };
        }
	}
	
}