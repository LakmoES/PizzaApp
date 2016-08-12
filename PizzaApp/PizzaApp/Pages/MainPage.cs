using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using PizzaApp.Data.ServerEntities;
using Newtonsoft.Json.Linq;
using System.Linq;
using PizzaApp.Data;
using PizzaApp.Pages.ProductsPageItems;
using PizzaApp.Data.Providers;
using PizzaApp.Data.Persistence;

namespace PizzaApp.Pages
{

	public class MainPage : ContentPage
	{
        private ActivityIndicator activityIndicator = new ActivityIndicator { IsRunning = false, IsVisible = false };
        private Label label = new Label { Text = "Магазин Pizza", HorizontalOptions = LayoutOptions.Center };
        private Button button = new Button { Text = "Перейти ко всем товарам" };
        private ListView listViewProducts;
        private IEnumerable<Product> products;
        private DBConnection dbc;
        public MainPage (DBConnection dbc)
		{
			Title = "Главная";
			Icon = "Contracts.png";

            this.dbc = dbc;
            button.Clicked += buttonClicked;

            listViewProducts = new ListView();
            listViewProducts.IsPullToRefreshEnabled = true;
            listViewProducts.ItemTemplate = new DataTemplate(typeof(ProductCell));
            listViewProducts.Refreshing += ListRefreshing;
            listViewProducts.ItemSelected += ListItemSelected;

            Content = new StackLayout
            {
                Children = { label, activityIndicator, listViewProducts, button }
            };

            DeactivateControls();
            listViewProducts.BeginRefresh();
        }
        private void DeactivateControls()
        {
            listViewProducts.IsEnabled = false;
            button.IsEnabled = false;
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
        }
        private void ActivateControls()
        {
            listViewProducts.IsEnabled = true;
            button.IsEnabled = true;
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }

        private async void ListRefreshing(object sender, EventArgs e)
        {
            if (await FillProductList())
                await UpdateCategories();
            (sender as ListView).IsRefreshing = false;
            ActivateControls();
        }
        private async Task<bool> FillProductList()
        {
            products = await ProductProvider.GetProductPage(1, 3) as List<Product>;
            if (products == null)
            {
                await DisplayAlert("Warning", "Connection problem", "OK");
                return false;
            }
            listViewProducts.ItemsSource = products.Select(a => new { id = a.id, title = a.title, subtitle = a.cost.ToString() + " грн", image = ImageSource.FromFile("icon.png") });
            return true;
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
        private async Task UpdateCategories()
        {
            var categories = await ProductProvider.GetCategories();
            if(categories == null)
            {
                await DisplayAlert("Warning", "Connection problem", "OK");
                return;
            }
            dbc.SaveCategoryList(categories.Select(c => new ProductCategory { id = c.id, title = c.title }).ToList());
        }

        public void buttonClicked(object sender, EventArgs e)
        {
            DeactivateControls();

            Navigation.PushAsync(new ProductsPage(dbc));

            ActivateControls();
        }
    }
	
}