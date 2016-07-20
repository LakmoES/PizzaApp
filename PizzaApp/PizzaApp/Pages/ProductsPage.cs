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
using PizzaApp.Data.ServerEntities;

namespace PizzaApp.Pages
{

	public class ProductsPage : ContentPage
	{
        private ListView listView;
        private List<Product> products;
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
            listView.ItemSelected += ListItemSelected;

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
        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            (sender as ListView).IsEnabled = false;

            var itemSourceList = (sender as ListView).ItemsSource.Cast<dynamic>().ToList();
            int index = itemSourceList.FindIndex(a => a.id == (e.SelectedItem as dynamic).id);
            
            Product p = products.ElementAt(index);
            await Navigation.PushAsync(new CurrentProductPage(p));

            (sender as ListView).SelectedItem = null;
            (sender as ListView).IsEnabled = true;
        }
        private async Task FillList()
        {
            products = await ProductProvider.GetProductPage(1, 10);
            if (listView == null)
                throw new NullReferenceException("listview is null");
            listView.ItemsSource = products.Select(a => new { id = a.id, title = a.title, subtitle = a.cost.ToString() + " грн", image = ImageSource.FromFile("icon.png") });
        }
	}
	
}