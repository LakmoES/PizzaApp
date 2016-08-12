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
        private ActivityIndicator activityIndicator;
        private Entry entryUsername, entryPassword;
        private Label labelTitle;
        private Button buttonLogin;
        private Button buttonRegister;
        private Button buttonEdit;
        private Button buttonOrders;
        private Button buttonGuestEnter;

        private Label labelUsername;
        private Label labelName;
        private Label labelSurname;
        private Label labelEmail;
        private Label labelGuest;
        private Button buttonLogout;

        private DBConnection dbc;
        public AccountPage(DBConnection dbc)
        {
            Title = "�������";
            Icon = "Accounts.png";
            this.dbc = dbc;

            Update();
        }
        private void DeactivateControls()
        {
            activityIndicator.IsVisible = true;
            activityIndicator.IsRunning = true;

            if (buttonLogin != null)
                this.buttonLogin.IsEnabled = false;
            if (entryUsername != null)
                this.entryUsername.IsEnabled = false;
            if (entryPassword != null)
                this.entryPassword.IsEnabled = false;
            if (buttonEdit != null)
                this.buttonEdit.IsEnabled = false;
            if (buttonLogout != null)
                this.buttonLogout.IsEnabled = false;
            if (buttonRegister != null)
                this.buttonRegister.IsEnabled = false;
            if (buttonOrders != null)
                this.buttonOrders.IsEnabled = false;
            if (buttonGuestEnter != null)
                this.buttonGuestEnter.IsEnabled = false;
        }
        private void ActivateControls()
        {
            activityIndicator.IsVisible = false;
            activityIndicator.IsRunning = false;

            if (buttonLogin != null)
                this.buttonLogin.IsEnabled = true;
            if (entryUsername != null)
                this.entryUsername.IsEnabled = true;
            if (entryPassword != null)
                this.entryPassword.IsEnabled = true;
            if (buttonEdit != null)
                this.buttonEdit.IsEnabled = true;
            if (buttonLogout != null)
                this.buttonLogout.IsEnabled = true;
            if (buttonRegister != null)
                this.buttonRegister.IsEnabled = true;
            if (buttonOrders != null)
                this.buttonOrders.IsEnabled = true;
            if (buttonGuestEnter != null)
                this.buttonGuestEnter.IsEnabled = true;
        }
        public void Update()
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                User user = dbc.GetUser();
                activityIndicator = new ActivityIndicator { IsRunning = false, IsVisible = false };
                labelTitle = new Label { Style = Device.Styles.TitleStyle };
                if (user == null)
                {
                    labelTitle.Text = "����������, ������� � �������";
                    entryUsername = new Entry { Placeholder = "��� ������������" };
                    entryPassword = new Entry { Placeholder = "������", IsPassword = true };

                    buttonLogin = new Button { Text = "�����" };
                    buttonLogin.Clicked += this.ButtonLogin_Clicked;

                    buttonRegister = new Button { Text = "�����������" };
                    buttonRegister.Clicked += this.ButtonRegister_Clicked;

                    buttonGuestEnter = new Button { Text = "�������� ����" };
                    buttonGuestEnter.Clicked += this.ButtonGuestEnter_Clicked;

                    Content = new StackLayout
                    {
                        Padding = new Thickness(10, 0, 10, 0),
                        Children =
                        {
                            activityIndicator, labelTitle, entryUsername, entryPassword, buttonLogin, buttonRegister, buttonGuestEnter
                        }
                    };
                }
                else
                {
                    buttonLogout = new Button { Text = "�����" };
                    buttonLogout.Clicked += this.ButtonLogout_Clicked;

                    buttonEdit = new Button { Text = "�������������" };
                    buttonEdit.Clicked += this.ButtonEdit_Clicked;

                    buttonOrders = new Button { Text = "������" };
                    buttonOrders.Clicked += async (sender, e) =>
                    {
                        DeactivateControls();
                        await Navigation.PushAsync(new OrdersPage(dbc));
                        ActivateControls();
                    };

                    if (user.guest == 0)
                    {
                        labelTitle.Text = "�� ����� ���:";
                        labelUsername = new Label { Text = "�����: " + user.username, Style = Device.Styles.SubtitleStyle };
                        labelName = new Label { Text = user.name == null ? "���: -" : "���: " + user.name, Style = Device.Styles.SubtitleStyle };
                        labelSurname = new Label { Text = user.surname == null ? "�������: -" : "�������: " + user.surname, Style = Device.Styles.SubtitleStyle };
                        labelEmail = new Label { Text = user.email == null ? "Email: -" : "Email: " + user.email, Style = Device.Styles.SubtitleStyle };
                        labelGuest = new Label { Text = user.guest == 0 ? "������: ������������" : "������: �����", Style = Device.Styles.SubtitleStyle };
                        Content = new StackLayout
                        {
                            Padding = new Thickness(10, 0, 10, 0),
                            Children =
                            {
                                activityIndicator, labelTitle, labelUsername, labelName, labelSurname, labelEmail, labelGuest, buttonEdit, buttonLogout, buttonOrders
                            }
                        };
                    }
                    else
                    {
                        labelTitle.Text = "�� ����� ��� �����.";
                        labelUsername = new Label { Text = "��� �����: " + user.username, Style = Device.Styles.SubtitleStyle };

                        buttonRegister = new Button { Text = "�����������" };
                        buttonRegister.Clicked += this.ButtonRegister_Clicked;

                        Content = new StackLayout
                        {
                            Padding = new Thickness(10, 0, 10, 0),
                            Children =
                            {
                                activityIndicator, labelTitle, labelUsername, buttonRegister, buttonLogout, buttonEdit, buttonOrders
                            }
                        };
                    }
                }
            });
        }

        private async void ButtonGuestEnter_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();

            var enterResult = await GuestEnter();
            
            if (enterResult != 1)
                await DisplayAlert("��������", "�� ������� ����� � �������� �������.", "OK");

            ActivateControls();
            Update();
        }
        private async Task<int> GuestEnter()
        {
            var guestAccount = await AuthProvider.NewGuest();
            if (guestAccount != null)
            {
                dbc.SaveUser(new User { username = guestAccount.username, password = guestAccount.password, guest = 1 });
                var token = await AuthProvider.Login(guestAccount.username, guestAccount.password);
                if (token != null)
                    dbc.SaveToken(token);
                else
                    return -1;
            }
            else
                return 0;
            return 1;
        }
        private async void ButtonLogin_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();

            Token receivedToken = await Login(entryUsername.Text, entryPassword.Text);

            if (receivedToken != null)
            {
                dbc.SaveToken(receivedToken);

                var user = await UserProvider.GetInfo(dbc);
                if (user == null)
                    throw new NullReferenceException("user is null");
                dbc.SaveUser(new User { username = user.username, password = entryPassword.Text, name = user.name, surname = user.surname, email = user.email, guest = user.guest });
            }

            ActivateControls();
            Update();
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
        private async void ButtonEdit_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            var user = await UserProvider.GetInfo(dbc);
            if (user == null)
                await DisplayAlert("������", "�� ������� ��������� � ��������.", "OK");
            else
                await Navigation.PushAsync(new AccountEditPage(dbc, user, this));
            ActivateControls();
        }
        private async void ButtonLogout_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            this.buttonLogout.IsEnabled = false;
            await Logout();
            ActivateControls();
            Update();
        }
        private async Task Logout()
        {
            Token t = dbc.GetToken();
            if (t != null)
            {
                string result = await AuthProvider.Logout(t.token_hash);
                if (result == null)
                    await DisplayAlert("Warning", "Connection error", "OK");
                dbc.RemoveUser();
                dbc.RemoveToken();
            }
        }
        private async void ButtonRegister_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            await Register();
            ActivateControls();
        }
        private async Task Register()
        {
            await Navigation.PushAsync(new RegistrationPage(dbc, this));
        }
    }
	
}