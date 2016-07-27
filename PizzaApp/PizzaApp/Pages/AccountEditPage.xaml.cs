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
        }
        private void FillFields(User user)
        {
            this.entryUsername.Text = user.username;
            this.entryEmail.Text = user.email ?? "";
            this.entryName.Text = user.name ?? "";
            this.entrySurname.Text = user.surname ?? "";
        }
        private async void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            this.IsEnabled = true;
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
            }
            this.IsEnabled = false;
        }
        private async void ButtonTelEdit_Clicked(object sender, EventArgs e)
        {
            var telNumbers = await UserProvider.GetTelList(dbc);
            //await DisplayAlert("Count", telNumbers.Count + "", "OK");
            await Navigation.PushAsync(new AccountTelEditPage(dbc, telNumbers));
        }
    }
}
