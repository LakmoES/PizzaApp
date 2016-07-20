using System;
using Xamarin.Forms;
using System.Collections.Generic;
using PizzaApp.Pages.ProductsPageItems;
using PizzaApp.Data;
using System.Linq;
using Android.Graphics;
using System.Threading.Tasks;
using PizzaApp.Data.Providers;
using PizzaApp.Data.Persistence;

namespace PizzaApp.Pages
{

	public class ProductsPage : ContentPage
	{
        ListView listView;
        public ProductsPage(DBConnection dbc)
        {
            Title = "Товары";
            Icon = "Leads.png";

            var label = new Label
            {
                Text = "Наши товары",
                HorizontalOptions = LayoutOptions.Center
            };
            listView = new ListView();
            listView.IsPullToRefreshEnabled = true;
            listView.ItemTemplate = new DataTemplate(typeof(ProductCell));
            listView.Refreshing += ListRefreshing;

            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    label,
                    listView
                }
            };
            listView.BeginRefresh();
        }
        private async void ListRefreshing(object sender, EventArgs e)
        {
            await FillList();
            (sender as ListView).IsRefreshing = false;
        }
        private async Task FillList()
        {
            var products = await ProductProvider.GetProductPage(1, 10);
            if (listView == null)
                throw new NullReferenceException("listview is null");
            listView.ItemsSource = products.Select(a => new { title = a.title, subtitle = a.cost.ToString() + " грн", image = ImageSource.FromFile("icon.png") });
        }
	}
	
}