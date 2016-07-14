using System;
using Xamarin.Forms;
using System.Linq;
using PizzaApp.Pages.MenuPageItems;

namespace PizzaApp.Pages
{
	public class RootPage : MasterDetailPage
	{
		MenuPage menuPage;

		public RootPage ()
		{
			menuPage = new MenuPage ();

			menuPage.Menu.ItemSelected += (sender, e) => NavigateTo (e.SelectedItem as MenuPageItems.MenuItem);

			Master = menuPage;
			Detail = new NavigationPage (new MainPage ());
		}

		void NavigateTo (MenuPageItems.MenuItem menu)
		{
			if (menu == null)
				return;
			
			Page displayPage = (Page)Activator.CreateInstance (menu.TargetType);

			Detail = new NavigationPage (displayPage);

			menuPage.Menu.SelectedItem = null;
			IsPresented = false;
		}
	}
}