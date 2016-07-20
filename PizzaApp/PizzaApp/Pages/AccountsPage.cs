using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaApp.Data.Providers;
using PizzaApp.Data.Persistence;

namespace PizzaApp.Pages
{

	public class AccountsPage : ContentPage
	{
        private Entry entryUsername, entryPassword;
        private Label labelResult;
        private Button buttonLogin;

        private Label labelUsername;
        private Button buttonLogout;

        private DBConnection dbc;
		public AccountsPage (DBConnection dbc)
		{
			Title = "Accounts";
			Icon = "Accounts.png";
            this.dbc = dbc;

            Update();
        }
        private async void ButtonLogin_Clicked(object sender, EventArgs e)
        {
            (sender as Button).IsEnabled = false;

            Token receivedToken = await Login(entryUsername.Text, entryPassword.Text);
            if(receivedToken != null)
            {
                dbc.SaveUser(new User { username = entryUsername.Text, password = entryPassword.Text});
                dbc.SaveToken(receivedToken);
            }

            (sender as Button).IsEnabled = true;

            Update();
        }
        private void Update()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                User user = dbc.GetUser();
                if (user == null)
                {
                    entryUsername = new Entry { Placeholder = "Имя пользователя" };
                    entryPassword = new Entry { Placeholder = "Пароль", IsPassword = true };
                    labelResult = new Label();

                    buttonLogin = new Button { Text = "Войти" };
                    buttonLogin.Clicked += this.ButtonLogin_Clicked;

                    Content = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        HorizontalOptions = LayoutOptions.Fill,
                        Children = {
                            entryUsername, entryPassword, buttonLogin, labelResult
                        }
                    };
                }
                else
                {
                    labelUsername = new Label { Text = user.username };
                    buttonLogout = new Button { Text = "Выйти" };
                    buttonLogout.Clicked += this.ButtonLogout_Clicked;
                    Content = new StackLayout
                    {
                        Orientation = StackOrientation.Vertical,
                        Children = { labelUsername, buttonLogout }
                    };
                }
            });
        }
        private async Task<Token> Login(string username, string password)
        {
            Token t = await AuthProvider.Login(username, password);
            if (t != null)
            {
                labelResult.Text = String.Format("{0}, {1} - {2}", t.token_hash, t.createTime, t.expTime);
                return t;
            }
            else
            {
                labelResult.Text = "Auth failed";
                return null;
            }
        }
        private async void ButtonLogout_Clicked(object sender, EventArgs e)
        {
            this.buttonLogout.IsEnabled = false;
            await Logout();
            Update();
        }
        private async Task Logout()
        {
            dbc.RemoveUser();
            Token t = dbc.GetToken();
            if (t != null)
            {
                await AuthProvider.Logout(t.token_hash);
                dbc.RemoveToken();
            }
        }
    }
	
}