using System;
using Xamarin.Forms;
using System.Linq;
using PizzaApp.Pages.MenuPageItems;
using PizzaApp.Data.Persistence;

namespace PizzaApp.Pages
{
	public class RootPage : MasterDetailPage
	{
		MenuPage menuPage;
        private DBConnection dbc;
		public RootPage (DBConnection dbc)
		{
			menuPage = new MenuPage (dbc);
            this.dbc = dbc;

			menuPage.Menu.ItemSelected += (sender, e) => NavigateTo (e.SelectedItem as MenuPageItems.MenuItem);

			Master = menuPage;
			Detail = new NavigationPage (new MainPage (dbc));
		}

		void NavigateTo (MenuPageItems.MenuItem menu)
		{
			if (menu == null)
				return;
			
			Page displayPage = (Page)Activator.CreateInstance (menu.TargetType, dbc);

			Detail = new NavigationPage (displayPage);

			menuPage.Menu.SelectedItem = null;
			IsPresented = false;
		}
	}
}