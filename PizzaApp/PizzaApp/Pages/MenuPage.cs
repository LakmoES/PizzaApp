using System;
using Xamarin.Forms;
using System.Collections.Generic;
using PizzaApp.Pages.MenuPageItems;
using PizzaApp.Data.Persistence;

namespace PizzaApp.Pages
{
	public class MenuPage : ContentPage
	{
        public ListView Menu { get; set; }

		public MenuPage (DBConnection dbc)
		{
			Icon = "settings.png";
			Title = "Меню"; // The Title property must be set.
			BackgroundColor = Color.FromHex ("333333");

			Menu = new MenuListView (dbc);

			var menuLabel = new ContentView {
				Padding = new Thickness (10, 36, 0, 5),
				Content = new Label {
					TextColor = Color.FromHex ("AAAAAA"),
					Text = "Меню", 
				}
			};

			var layout = new StackLayout { 
				Spacing = 0, 
				VerticalOptions = LayoutOptions.FillAndExpand
			};
			layout.Children.Add (menuLabel);
			layout.Children.Add (Menu);

			Content = layout;
		}
	}
}