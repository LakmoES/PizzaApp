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
        Label label = new Label { Text = "������� Pizza", HorizontalOptions = LayoutOptions.Center };
        Button button = new Button { Text = "������� �� ���� �������" };
        ListView listViewProducts;
        private DBConnection dbc;
        public MainPage (DBConnection dbc)
		{
			Title = "�������";
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
            (sender as ListView).IsRefreshing = false;
        }
        private async Task FillProductList()
        {
            var products = await ProductProvider.GetProductPage(1, 3);
            if (listViewProducts == null)
                throw new NullReferenceException("listview is null");
            listViewProducts.ItemsSource = products.Select(a => new { title = a.title, subtitle = a.cost.ToString() + " ���", image = ImageSource.FromFile("icon.png") });
        }

        public void bClicked(object sender, EventArgs e)
        {
            button.IsEnabled = false;

            Navigation.PushAsync(new ProductsPage(dbc));
                        
            button.IsEnabled = true;
        }
    }
	
}