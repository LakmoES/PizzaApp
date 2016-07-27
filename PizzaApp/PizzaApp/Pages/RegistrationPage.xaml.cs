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
    public partial class RegistrationPage : ContentPage
    {
        private DBConnection dbc;
        private AccountPage parentPage;
        public RegistrationPage(DBConnection dbc, AccountPage parentPage)
        {
            InitializeComponent();
            Title = "Регистрация";
            
            this.dbc = dbc;
            this.parentPage = parentPage;

            this.buttonSubmit.Clicked += ButtonSubmit_Clicked;
        }
        private async void ButtonSubmit_Clicked(object sender, EventArgs e)
        {
            this.IsEnabled = false;
            await Register();
        }
        private async Task Register()
        {
            var errorList = await AuthProvider.Register(entryUsername.Text, entryPassword.Text, entryEmail.Text, entryName.Text, entrySurname.Text);
            if (errorList != null)
            {
                await DisplayAlert("Warning", String.Join(Environment.NewLine, errorList.Select(x => x.error)), "OK");
                this.IsEnabled = true;
                return;
            }
            Token token = await AuthProvider.Login(entryUsername.Text, entryPassword.Text);
            if (token == null)
            {
                await DisplayAlert("Error", "Something went wrong. Token is null", "OK");
                this.IsEnabled = true;
                return;
            }
            dbc.SaveToken(token);
            User user = await UserProvider.GetInfo(dbc);
            if(user == null)
            {
                await DisplayAlert("Error", "Something went wrong. User is null", "OK");
                this.IsEnabled = true;
                return;
            }
            dbc.SaveUser(user);
            parentPage.Update();

            await Navigation.PopAsync();
        }
    }
}
