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
            Token token = null;
            string userInfo = String.Empty, tokenInfo = String.Empty;
            try
            {
                user = dbc.GetUser();
                userInfo = user == null ? "empty user" : "username:pass -> " + user.username + ":" + user.password;
                token = dbc.GetToken();
                tokenInfo = token == null ? "empty token" : "token: " + token.token_hash;
            }
            catch(Exception ex) { userInfo = ex.ToString(); }

            Content = new StackLayout
            {
                Children = { new Label { Text = "DB path: " + dbc.db.DatabasePath + Environment.NewLine + userInfo + Environment.NewLine + tokenInfo, Style = Device.Styles.SubtitleStyle } }
            };
        }
	}
	
}