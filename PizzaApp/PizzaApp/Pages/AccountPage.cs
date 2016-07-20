using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Threading.Tasks;
using PizzaApp.Data.Providers;
using PizzaApp.Data.Persistence;

namespace PizzaApp.Pages
{

	public class AccountPage : ContentPage
	{
        private Entry entryUsername, entryPassword;
        private Label labelTitle;
        private Button buttonLogin;

        private Label labelUsername;
        private Label labelName;
        private Label labelSurname;
        private Label labelEmail;
        private Label labelGuest;
        private Button buttonLogout;

        private DBConnection dbc;
		public AccountPage (DBConnection dbc)
		{
			Title = "Аккаунт";
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
                dbc.SaveToken(receivedToken);

                var user = await UserProvider.GetInfo(dbc);
                if (user == null)
                    throw new NullReferenceException("user is null");
                dbc.SaveUser(new User { username = user.username, password = entryPassword.Text, name = user.name, surname = user.surname, email = user.email, guest = user.guest });
            }

            (sender as Button).IsEnabled = true;

            Update();
        }
        private void Update()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                User user = dbc.GetUser();
                //try
                //{
                //    var u = UserProvider.GetInfo(dbc).Result;
                //    user = u;
                //}
                //catch { }
                labelTitle = new Label { Style = Device.Styles.TitleStyle };
                if (user == null)
                {
                    labelTitle.Text = "Пожалуйста, войдите в систему";
                    entryUsername = new Entry { Placeholder = "Имя пользователя" };
                    entryPassword = new Entry { Placeholder = "Пароль", IsPassword = true };

                    buttonLogin = new Button { Text = "Войти" };
                    buttonLogin.Clicked += this.ButtonLogin_Clicked;

                    Content = new StackLayout
                    {
                        Children =
                        {
                            labelTitle, entryUsername, entryPassword, buttonLogin
                        }
                    };
                }
                else
                {
                    labelTitle.Text = "Вы вошли как:";
                    labelUsername = new Label { Text = "Логин: " + user.username, Style = Device.Styles.SubtitleStyle };
                    labelName = new Label { Text = user.name == null ? "Имя: -" : "Имя: " + user.name, Style = Device.Styles.SubtitleStyle };
                    labelSurname = new Label { Text = user.surname == null ? "Фамилия: -" : "Фамилия: " + user.surname, Style = Device.Styles.SubtitleStyle };
                    labelEmail = new Label { Text = user.email == null ? "Email: -" : "Email: " + user.email, Style = Device.Styles.SubtitleStyle };
                    labelGuest = new Label { Text = user.guest == 0 ? "Статус: Пользователь" : "Статус: Гость", Style = Device.Styles.SubtitleStyle };
                    buttonLogout = new Button { Text = "Выйти" };
                    buttonLogout.Clicked += this.ButtonLogout_Clicked;
                    Content = new StackLayout
                    {
                        Children =
                        {
                            labelTitle, labelUsername, labelName, labelSurname, labelEmail, labelGuest, buttonLogout
                        }
                    };
                }
            });
        }
        private async Task<Token> Login(string username, string password)
        {
            Token t = await AuthProvider.Login(username, password);
            if (t != null)
                return t;
            else
            {
                await DisplayAlert("Failed", "Auth failed", "OK");
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