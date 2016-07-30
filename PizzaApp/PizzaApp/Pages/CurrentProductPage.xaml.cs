using PizzaApp.Data.Persistence;
using PizzaApp.Data.Providers;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PizzaApp.Pages
{
    public partial class CurrentProductPage : ContentPage
    {
        private DBConnection dbc;
        private Product product;
        public CurrentProductPage(DBConnection dbc, Product product, int alreadyExists)
        {
            InitializeComponent();
            this.dbc = dbc;
            this.product = product;

            if (this.product.available == 0)
                this.buttonAddToCart.IsEnabled = false;
            if (alreadyExists <= 0)
                this.labelAlreadyExists.Text = "";
            else
                this.labelAlreadyExists.Text = String.Format("Уже есть в корзине: {0}", alreadyExists);

            this.Title = product.title;
            this.labelCategory.Text = dbc.GetCategoryTitle(product.category);
            this.labelAvailable.Text = product.available == 1 ? "В наличии" : "Нет в наличии";
            this.labelAdvertising.Text = product.advertising == 1 ? "Акция!" : "";
            this.labelCostPerAmount.Text = String.Format("{0} грн за {1}", product.cost, product.measure);

            this.stepperBuyAmount.ValueChanged += StepperBuyAmount_ValueChanged;
            this.buttonAddToCart.Clicked += ButtonAddToCart_Clicked;
        }

        private async void ButtonAddToCart_Clicked(object sender, EventArgs e)
        {
            this.buttonAddToCart.IsEnabled = false;
            await AddToCart(Convert.ToInt32(this.labelBuyAmount.Text));
        }
        private async Task AddToCart(int amount)
        {
            bool result = await ShopCartProvider.AddProduct(dbc, product.id, amount);
            if (!result)
                await DisplayAlert("Ошибка", "Не удалось добавить товар в корзину.", "OK");
            else
            {
                this.stepperBuyAmount.Value = 1;
                this.labelAlreadyExists.Text = String.Format("Уже есть в корзине: {0}", await AlreadyAmount());
            }

            this.buttonAddToCart.IsEnabled = true;
        }
        private async Task<int> AlreadyAmount()
        {
            return await ShopCartProvider.ProductExists(dbc, product.id);
        }

        private void StepperBuyAmount_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.labelBuyAmount.Text = e.NewValue.ToString();
        }
    }
}
