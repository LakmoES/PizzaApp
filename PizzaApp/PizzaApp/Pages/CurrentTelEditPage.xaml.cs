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
    public partial class CurrentTelEditPage : ContentPage
    {
        private DBConnection dbc;
        private TelNumber telNumber;
        private AccountTelEditPage parentPage;
        public CurrentTelEditPage(DBConnection dbc,TelNumber telNumber, AccountTelEditPage parentPage)
        {
            InitializeComponent();
            Title = "Изменение номера";
            this.dbc = dbc;
            this.telNumber = telNumber;
            this.parentPage = parentPage;
            this.entryTelNumber.Text = telNumber.number;
            this.buttonSubmit.Clicked += ButtonSubmit_Clicked;
        }
        private void DeactivateControls()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            entryTelNumber.IsEnabled = false;
            buttonSubmit.IsEnabled = false;
        }
        private void ActivateControls()
        {
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            entryTelNumber.IsEnabled = true;
            buttonSubmit.IsEnabled = true;
        }
        private async void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            if (!String.IsNullOrWhiteSpace(this.entryTelNumber.Text))
                if (!await UserProvider.EditTel(dbc, telNumber.id, this.entryTelNumber.Text))
                    await DisplayAlert("Ошибка", "Не удалось изменить номер.", "OK");
                else
                {
                    await parentPage.GetTelListFromServer();
                    await Navigation.PopAsync();
                }
            else
                await DisplayAlert(null, "Укажите номер.", "OK");
            ActivateControls();
        }
    }
}
