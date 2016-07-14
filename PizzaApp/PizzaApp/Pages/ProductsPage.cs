using System;
using Xamarin.Forms;
using System.Collections.Generic;
using PizzaApp.Pages.ProductsPageItems;
using PizzaApp.Data;
using System.Linq;
using Android.Graphics;
using System.Threading.Tasks;

namespace PizzaApp.Pages
{

	public class ProductsPage : ContentPage
	{
        ListView listView;
        public ProductsPage()
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
            var products = await ServerProcessor.GetProducts(1, 10);
            if (listView == null)
                throw new NullReferenceException("listview is null");
            listView.ItemsSource = products.Select(a => new { title = a.title, subtitle = a.cost.ToString(), image = ImageSource.FromFile("icon.png") });
        }
	}
	
}