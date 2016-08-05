using System;
using Xamarin.Forms;
using System.Collections.Generic;
using PizzaApp.Data.Persistence;

namespace PizzaApp.Pages.MenuPageItems
{

	public class MenuListView : ListView
	{
		public MenuListView (DBConnection dbc)
		{
			List<MenuItem> data = new MenuListData (dbc);

			ItemsSource = data;
			VerticalOptions = LayoutOptions.FillAndExpand;
			BackgroundColor = Color.Transparent;
			SeparatorVisibility = SeparatorVisibility.None;

			var cell = new DataTemplate (typeof(MenuCell));
			cell.SetBinding (MenuCell.TextProperty, "Title");
			cell.SetBinding (MenuCell.ImageSourceProperty, "IconSource");

			ItemTemplate = cell;
		}
	}
}