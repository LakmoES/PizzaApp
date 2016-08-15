using PizzaApp.Data.Persistence;
using PizzaApp.Data.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PizzaApp.Pages
{
    public partial class AccountEditPage : ContentPage
    {
        private DBConnection dbc;
        private AccountPage parentPage;
        public AccountEditPage(DBConnection dbc, User user, AccountPage parentPage)
        {
            InitializeComponent();
            Title = "Редактирование профиля";

            this.dbc = dbc;
            FillFields(user);
            this.parentPage = parentPage;

            this.buttonSubmit.Clicked += ButtonSubmit_Clicked;
            this.buttonTelEdit.Clicked += ButtonTelEdit_Clicked;
            this.buttonAddressEdit.Clicked += ButtonAddressEdit_Clicked;
        }
        private void DeactivateControls()
        {
            entryUsername.IsEnabled = false;
            entryPassword.IsEnabled = false;
            entryEmail.IsEnabled = false;
            entryName.IsEnabled = false;
            entrySurname.IsEnabled = false;
            buttonSubmit.IsEnabled = false;
            buttonTelEdit.IsEnabled = false;
            buttonAddressEdit.IsEnabled = false;

            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
        }
        private void ActivateControls()
        {
            entryPassword.IsEnabled = true;
            entryEmail.IsEnabled = true;
            entryName.IsEnabled = true;
            entrySurname.IsEnabled = true;
            buttonSubmit.IsEnabled = true;
            buttonTelEdit.IsEnabled = true;
            buttonAddressEdit.IsEnabled = true;

            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }
        private void FillFields(User user)
        {
            if (user.guest == 1)
                entryPassword.IsVisible = false;
            this.entryUsername.Text = user.username;
            this.entryEmail.Text = user.email ?? "";
            this.entryName.Text = user.name ?? "";
            this.entrySurname.Text = user.surname ?? "";
        }
        private async void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            await Edit();
        }
        private async Task Edit()
        {
            string password = null;
            if (this.entryPassword.Text.Length > 0)
                password = this.entryPassword.Text;

            var errorList = await UserProvider.Edit(dbc, password, this.entryEmail.Text, this.entryName.Text, this.entrySurname.Text);
            if (errorList != null)
                await DisplayAlert("Ошибка", String.Join(Environment.NewLine, errorList.Select(x => x.error)), "OK");
            else
            {
                var user = await UserProvider.GetInfo(dbc);
                
                if (user != null)
                {
                    if (password == null)
                        user.password = dbc.GetUser().password;
                    else
                        user.password = this.entryPassword.Text;

                    dbc.SaveUser(user);
                    FillFields(user);
                }
                else
                {
                    await DisplayAlert("Внимание", "Редактирование прошло успешно, обновить локальные данные не удалось.", "OK");
                    await Navigation.PopAsync();
                }
                await DisplayAlert("Успех", "Ваш профиль успешно обновлен.", "OK");
                this.parentPage.Update();
                await Navigation.PopAsync();
            }
            ActivateControls();
        }
        private async void ButtonTelEdit_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            var telNumbers = await UserProvider.GetTelList(dbc);
            await Navigation.PushAsync(new AccountTelEditPage(dbc, telNumbers));
            ActivateControls();
        }
        private async void ButtonAddressEdit_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            var addresses = await UserProvider.GetAddressList(dbc);
            await Navigation.PushAsync(new AccountAddressEditPage(dbc, addresses));
            ActivateControls();
        }
    }
}
