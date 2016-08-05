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
    public partial class ShopCartProductEdit : ContentPage
    {
        private DBConnection dbc;
        private ShopCartPage parent;
        private ShopCartProduct product;
        public ShopCartProductEdit(DBConnection dbc, ShopCartProduct product, ShopCartPage parent)
        {
            InitializeComponent();
            Title = product.title + ", количество";
            if (product == null)
                throw new NullReferenceException("ShopCartProductEdit product is null");
            this.dbc = dbc;
            this.product = product;
            this.parent = parent;

            this.labelAmount.Text = product.amount.ToString();
            this.stepperAmount.Value = product.amount;
            this.buttonSubmit.Clicked += ButtonSubmit_Clicked;
            this.stepperAmount.ValueChanged += StepperAmount_ValueChanged;
        }
        private void DeactivateControls()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            this.buttonSubmit.IsEnabled = false;
            this.stepperAmount.IsEnabled = false;
        }
        private void ActivateControls()
        {
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            this.buttonSubmit.IsEnabled = true;
            this.stepperAmount.IsEnabled = true;
        }

        private void StepperAmount_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.labelAmount.Text = e.NewValue.ToString();
        }

        private async void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            await Submit();
        }
        private async Task Submit()
        {
            if (!await ShopCartProvider.EditProduct(dbc, product.productid, Convert.ToInt32(this.stepperAmount.Value)))
            {
                await DisplayAlert(null, "Не удалось изменить количество.", "OK");
                ActivateControls();
            }
            else
            {
                parent.BeginRefresh();
                await Navigation.PopAsync();
            }
        }
    }
}
