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
        private DBConnection dbc;
        private ListView listView;
        private ActivityIndicator activityIndicator;
        private List<Product> products;
        public ProductsPage(DBConnection dbc)
        {
            Title = "Товары";
            Icon = "Leads.png";

            this.dbc = dbc;
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

            activityIndicator = new ActivityIndicator { IsVisible = false, IsRunning = false };

            Content = new StackLayout
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    label,
                    activityIndicator,
                    listView
                }
            };

            DeactivateControls();
            listView.BeginRefresh();
        }
        private void DeactivateControls()
        {
            listView.IsEnabled = false;
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
        }
        private void ActivateControls()
        {
            listView.IsEnabled = true;
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }
        private async void ListRefreshing(object sender, EventArgs e)
        {
            await FillList();
            (sender as ListView).IsRefreshing = false;
            ActivateControls();
        }
        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            DeactivateControls();

            var itemSourceList = (sender as ListView).ItemsSource.Cast<dynamic>().ToList();
            int index = itemSourceList.FindIndex(a => a.id == (e.SelectedItem as dynamic).id);
            
            Product p = products.ElementAt(index);
            await Navigation.PushAsync(new CurrentProductPage(dbc, p, await ShopCartProvider.ProductExists(dbc, p.id)));

            (sender as ListView).SelectedItem = null;
            ActivateControls();
        }
        private async Task FillList()
        {
            products = await ProductProvider.GetProductPage(1, 10) as List<Product>;
            if (products == null)
            {
                await DisplayAlert("Warning", "Connection problem", "OK");
                return;
            }
            listView.ItemsSource = products.Select(a => new { id = a.id, title = a.title, subtitle = a.cost.ToString() + " грн", image = ImageSource.FromFile("icon.png") });
        }
	}
	
}