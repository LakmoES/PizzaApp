using PizzaApp.Data.Persistence;
using PizzaApp.Data.Providers;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PizzaApp.Pages
{
    public partial class ShopCartPage : ContentPage
    {
        private DBConnection dbc;
        private ObservableCollection<ShopCartProduct> products;
        public ShopCartPage(DBConnection dbc)
        {
            Title = "Корзина";
            InitializeComponent();
            this.dbc = dbc;

            this.buttonMakeOrder.Clicked += ButtonMakeOrder_Clicked;
            this.buttonClearShopCart.Clicked += ButtonClearShopCart_Clicked;
            this.buttonUsePromocode.Clicked += ButtonUsePromocode_Clicked;

            this.listViewProducts.Refreshing += ListViewProducts_Refreshing;
            this.listViewProducts.BeginRefresh();
        }
        public void BeginRefresh()
        {
            listViewProducts.BeginRefresh();
        }
        private void ButtonUsePromocode_Clicked(object sender, EventArgs e)
        {
            UsePromocode();
        }
        private void UsePromocode()
        {
            listViewProducts.BeginRefresh();
        }

        private void DeactivateControls()
        {
            this.buttonMakeOrder.IsEnabled = false;
            this.buttonClearShopCart.IsEnabled = false;
            this.buttonUsePromocode.IsEnabled = false;

            this.listViewProducts.IsEnabled = false;
        }
        private void ActivateControls()
        {
            this.buttonMakeOrder.IsEnabled = true;
            this.buttonClearShopCart.IsEnabled = true;
            this.buttonUsePromocode.IsEnabled = true;

            this.listViewProducts.IsEnabled = true;
        }

        private async void ButtonClearShopCart_Clicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert(null, "Действительно хотите удалить все товары из корзины?", "Да", "Нет");
            if (!answer)
                return;

            DeactivateControls();
            await ClearShopCart();
        }
        private async Task ClearShopCart()
        {
            var errors = await ShopCartProvider.Clear(dbc);
            if (errors != null)
                await DisplayAlert("Ошибка", String.Join(Environment.NewLine, errors.Select(x => x.error)), "OK");
            await UpdateShopCart();
            ActivateControls();
        }
        private async void ButtonMakeOrder_Clicked(object sender, EventArgs e)
        {
            await GoToMakingOrder();
        }

        private async Task GoToMakingOrder()
        {
            DeactivateControls();
            await Navigation.PushAsync(new MakingOrderPage(dbc));
            ActivateControls();
        }

        private async void ListViewProducts_Refreshing(object sender, EventArgs e)
        {
            DeactivateControls();
            await UpdateShopCart();
        }
        private async Task UpdateShopCart()
        {
            string promocode = null;
            if (!String.IsNullOrWhiteSpace(this.entryPromocode.Text))
                promocode = this.entryPromocode.Text;
            var products = await ShopCartProvider.Show(dbc, promocode);
            if (products == null)
            {
                this.labelTotal.Text = String.Empty;
                listViewProducts.ItemsSource = null;
                this.products = null;
                await DisplayAlert(null, "Произошла ошибка", "OK");
                listViewProducts.IsRefreshing = false;
                ActivateControls();
                return;
            }
            this.products = new ObservableCollection<ShopCartProduct>(products);

            listViewProducts.ItemsSource = this.products;
            this.labelTotal.Text = String.Format("В корзине товаров на {0} грн", products.Sum(x => x.resultPrice));

            listViewProducts.IsRefreshing = false;
            ActivateControls();
        }

        public async void OnEdit(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            int productID = Convert.ToInt32(mi.CommandParameter);
            await Navigation.PushAsync(new ShopCartProductEdit(dbc, products.Where(x => x.productid == productID).FirstOrDefault(), this));
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Подтверждение", "Действительно хотите удалить?", "Да", "Нет");
            if (!answer)
                return;
            DeactivateControls();
            var mi = ((MenuItem)sender);
            int productID = Convert.ToInt32(mi.CommandParameter);
            bool removeResult = await ShopCartProvider.RemoveProduct(dbc, productID);
            if (removeResult)
            {
                for (int i = 0; i < products.Count; ++i)
                    if (products.ElementAt(i).productid == productID)
                    {
                        products.RemoveAt(i);
                        break;
                    }
            }
            else
                await DisplayAlert("Ошибка", "Не удалось удалить товар.", "OK");
            ActivateControls();
        }
    }
}
