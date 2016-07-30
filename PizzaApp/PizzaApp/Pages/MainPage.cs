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
        Label label = new Label { Text = "Магазин Pizza", HorizontalOptions = LayoutOptions.Center };
        Button button = new Button { Text = "Перейти ко всем товарам" };
        ListView listViewProducts;
        private DBConnection dbc;
        public MainPage (DBConnection dbc)
		{
			Title = "Главная";
			Icon = "Contracts.png";

            this.dbc = dbc;
            button.Clicked += bClicked;

            listViewProducts = new ListView();
            listViewProducts.IsPullToRefreshEnabled = true;
            listViewProducts.ItemTemplate = new DataTemplate(typeof(ProductCell));
            listViewProducts.Refreshing += ListRefreshing;

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
            var products = await ProductProvider.GetProductPage(1, 3);
            if (products == null)
            {
                await DisplayAlert("Warning", "Connection problem", "OK");
                return;
            }
            listViewProducts.ItemsSource = products.Select(a => new { title = a.title, subtitle = a.cost.ToString() + " грн", image = ImageSource.FromFile("icon.png") });
        }
        private async Task UpdateCategories()
        {
            var categories = await ProductProvider.GetCategories();
            dbc.SaveCategoryList(categories.Select(c => new ProductCategory { id = c.id, title = c.title }).ToList());
        }

        public void bClicked(object sender, EventArgs e)
        {
            button.IsEnabled = false;

            Navigation.PushAsync(new ProductsPage(dbc));
                        
            button.IsEnabled = true;
        }
    }
	
}