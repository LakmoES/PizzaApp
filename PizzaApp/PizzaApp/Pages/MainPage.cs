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
                Children = { label, listViewProducts, button }
            };

            listViewProducts.BeginRefresh();
        }

        private async void ListRefreshing(object sender, EventArgs e)
        {
            await FillProductList();
            await UpdateCategories();
            (sender as ListView).IsRefreshing = false;
        }
        private async Task FillProductList()
        {
            products = await ProductProvider.GetProductPage(1, 3) as List<Product>;
            if (products == null)
            {
                await DisplayAlert("Warning", "Connection problem", "OK");
                return;
            }
            listViewProducts.ItemsSource = products.Select(a => new { id = a.id, title = a.title, subtitle = a.cost.ToString() + " грн", image = ImageSource.FromFile("icon.png") });
        }
        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            (sender as ListView).IsEnabled = false;

            var itemSourceList = (sender as ListView).ItemsSource.Cast<dynamic>().ToList();
            int index = itemSourceList.FindIndex(a => a.id == (e.SelectedItem as dynamic).id);

            Product p = products.ElementAt(index);
            await Navigation.PushAsync(new CurrentProductPage(dbc, p, await ShopCartProvider.ProductExists(dbc, p.id)));

            (sender as ListView).SelectedItem = null;
            (sender as ListView).IsEnabled = true;
        }
        private async Task UpdateCategories()
        {
            var categories = await ProductProvider.GetCategories();
            dbc.SaveCategoryList(categories.Select(c => new ProductCategory { id = c.id, title = c.title }).ToList());
        }

        public void buttonClicked(object sender, EventArgs e)
        {
            button.IsEnabled = false;

            Navigation.PushAsync(new ProductsPage(dbc));
                        
            button.IsEnabled = true;
        }
    }
	
}