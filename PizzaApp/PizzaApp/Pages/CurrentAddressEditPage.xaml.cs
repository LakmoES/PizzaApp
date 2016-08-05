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
    public partial class CurrentAddressEditPage : ContentPage
    {
        private DBConnection dbc;
        private Address address;
        private AccountAddressEditPage parentPage;
        public CurrentAddressEditPage(DBConnection dbc, Address address, AccountAddressEditPage parentPage)
        {
            InitializeComponent();
            Title = "Изменение адреса доставки";
            this.dbc = dbc;
            this.address = address;
            this.parentPage = parentPage;
            this.entryAddress.Text = address.address;
            this.buttonSubmit.Clicked += ButtonSubmit_Clicked; ;
        }
        private void DeactivateControls()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;
            
            entryAddress.IsEnabled = false;
            buttonSubmit.IsEnabled = false;
        }
        private void ActivateControls()
        {
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            entryAddress.IsEnabled = true;
            buttonSubmit.IsEnabled = true;
        }
        private async void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            if (!String.IsNullOrWhiteSpace(this.entryAddress.Text))
                if (!await UserProvider.EditAddress(dbc, address.id, this.entryAddress.Text))
                    await DisplayAlert("Ошибка", "Не удалось изменить адрес.", "OK");
                else
                {
                    await parentPage.GetAddressListFromServer();
                    await Navigation.PopAsync();
                }
            else
                await DisplayAlert(null, "Укажите адрес.", "OK");
            ActivateControls();
        }
    }
}
