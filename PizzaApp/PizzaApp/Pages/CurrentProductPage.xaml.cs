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
            {
                this.buttonAddToCart.IsEnabled = false;
                this.buttonBuyProduct.IsEnabled = false;
            }
            this.labelAlreadyExists.Text = alreadyExists <= 0 ? "" : string.Format("Уже есть в корзине: {0}", alreadyExists);

            this.Title = product.title;
            this.labelCategory.Text = dbc.GetCategoryTitle(product.category);
            this.labelAvailable.Text = product.available == 1 ? "В наличии" : "Нет в наличии";
            this.labelAdvertising.Text = product.advertising == 1 ? "Акция!" : "";
            this.labelCostPerAmount.Text = string.Format("{0} грн за {1}", product.cost, product.measure);

            this.stepperBuyAmount.ValueChanged += StepperBuyAmount_ValueChanged;
            this.buttonAddToCart.Clicked += ButtonAddToCart_Clicked;
            this.buttonBuyProduct.Clicked += ButtonBuyProduct_Clicked;
        }

        private async void ButtonBuyProduct_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            if (dbc.GetUser() == null)
            {
                bool result = await DisplayAlert("Вход не выполнен", "Вы хотите войти в свой профиль или создать гостевой?", "Да", "Нет");
                if (result)
                    await Navigation.PushAsync(new AccountPage(dbc));
                else
                {
                    ActivateControls();
                    return;
                }
            }
            else
                await GoToMakingOrder();
            ActivateControls();
        }
        private async Task GoToMakingOrder()
        {
            var addresses = await UserProvider.GetAddressList(dbc);
            if (addresses == null)
                await DisplayAlert("Ошибка", "Не удалось связаться с сервером.", "OK");
            else
                await Navigation.PushAsync(
                    new MakingOrderPage(
                        dbc,
                        addresses,
                        product.cost * Convert.ToDecimal(this.stepperBuyAmount.Value),
                        productID: product.id,
                        amountOfProduct: Convert.ToInt32(this.stepperBuyAmount.Value)
                        )
                );
            ActivateControls();
        }

        private void DeactivateControls()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            stepperBuyAmount.IsEnabled = false;
            buttonAddToCart.IsEnabled = false;
            buttonBuyProduct.IsEnabled = false;
        }
        private void ActivateControls()
        {
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            stepperBuyAmount.IsEnabled = true;
            buttonAddToCart.IsEnabled = true;
            buttonBuyProduct.IsEnabled = true;
        }
        private async void ButtonAddToCart_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            if (dbc.GetUser() == null)
            {
                bool result = await DisplayAlert("Вход не выполнен", "Вы хотите войти в свой профиль или создать гостевой?", "Да", "Нет");
                if (result)
                    await Navigation.PushAsync(new AccountPage(dbc));
                else
                {
                    ActivateControls();
                    return;
                }
            }
            else
                await AddToCart(Convert.ToInt32(this.labelBuyAmount.Text));
            ActivateControls();
        }
        private async Task AddToCart(int amount)
        {
            bool result = await ShopCartProvider.AddProduct(dbc, product.id, amount);
            if (!result)
                await DisplayAlert("Ошибка", "Не удалось добавить товар в корзину.", "OK");
            else
            {
                this.stepperBuyAmount.Value = 1;
                this.labelAlreadyExists.Text = $"Уже есть в корзине: {await AlreadyAmount()}";
            }

            ActivateControls();
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
