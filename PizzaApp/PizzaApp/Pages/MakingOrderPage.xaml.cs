﻿using PizzaApp.Data.Persistence;
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
    public partial class MakingOrderPage : ContentPage
    {
        private readonly bool AllowOrderWithoutAddress = true;

        private DBConnection dbc;
        private IEnumerable<Address> addresses;
        private ShopCartPage parent;
        private string promocode;
        public MakingOrderPage(DBConnection dbc, IEnumerable<Address> addresses, Decimal totalPrice, ShopCartPage parent, string promocode = null)
        {
            InitializeComponent();
            Title = "Сделать заказ";
            this.dbc = dbc;
            this.addresses = addresses;
            this.parent = parent;
            this.promocode = promocode;

            this.labelTotalPrice.Text = String.Format("Заказ на сумму {0} грн", totalPrice);
            foreach (var address in addresses)
                this.pickerDeliveryAddress.Items.Add(address.address);

            buttonSubmit.Clicked += ButtonSubmit_Clicked;
        }

        private async void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            await MakeOrder();
        }
        private async Task MakeOrder()
        {
            if (!AllowOrderWithoutAddress && (pickerDeliveryAddress.Items.Count <= 0 || pickerDeliveryAddress.SelectedIndex < 0))
                await DisplayAlert("Ошибка", "Не выбран адрес доставки.", "OK");
            else
            {
                int? addressID = pickerDeliveryAddress.SelectedIndex == -1 ? null : addresses.ElementAt(pickerDeliveryAddress.SelectedIndex).id as int?;
                var result = await ShopCartProvider.MakeOrder(dbc, addressID, promocode);
                if (result == null)
                    await DisplayAlert("Ошибка", "Не удалось совершить заказ.", "OK");
                else
                {
                    await DisplayAlert(null, String.Format("Заказ №{0} отправлен на обработку. С вами свяжутся в ближайшее время.", result), "OK");
                    await parent.UpdateShopCart();
                    await Navigation.PopAsync();
                }
            }
                
            ActivateControls();
        }

        private void DeactivateControls()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            pickerDeliveryAddress.IsEnabled = false;
            buttonSubmit.IsEnabled = false;
        }
        private void ActivateControls()
        {
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            pickerDeliveryAddress.IsEnabled = true;
            buttonSubmit.IsEnabled = true;
        }
    }
}
