using PizzaApp.Data.Persistence;
using PizzaApp.Data.Providers;
using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PizzaApp.Pages
{
    public partial class AccountAddressEditPage : ContentPage
    {
        private DBConnection dbc;
        private ObservableCollection<Address> addresses;
        public AccountAddressEditPage(DBConnection dbc, List<Address> addresses)
        {
            InitializeComponent();
            Title = "Управление адресами доставки";
            this.dbc = dbc;
            this.addresses = new ObservableCollection<Address>(addresses);
            this.listViewAddressList.ItemsSource = this.addresses;
            this.buttonAddressAdd.Clicked += ButtonAddressAdd_Clicked;
        }
        private async void ButtonAddressAdd_Clicked(object sender, EventArgs e)
        {
            if (!await UserProvider.AddAddress(dbc, this.entryNewAddress.Text))
                await DisplayAlert("Ошибка", "Не удалось добавить адрес.", "OK");
            else
            {
                this.entryNewAddress.Text = String.Empty;
                this.addresses = new ObservableCollection<Address>(await UserProvider.GetAddressList(dbc));
                UpdateAddressList();
            }
        }
        private void UpdateAddressList()
        {
            this.listViewAddressList.ItemsSource = null;
            this.listViewAddressList.ItemsSource = addresses;
        }
        public async Task GetAddressListFromServer()
        {
            this.addresses = new ObservableCollection<Address>(await UserProvider.GetAddressList(dbc));
            UpdateAddressList();
        }
        public async void OnEdit(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            int addressID = Convert.ToInt32(mi.CommandParameter);
            await Navigation.PushAsync(new CurrentAddressEditPage(dbc, addresses.Where(x => x.id == addressID).FirstOrDefault(), this));
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Подтверждение", "Действительно хотите удалить?", "Да", "Нет");
            if (!answer)
                return;
            
            var mi = ((MenuItem)sender);
            int addressID = Convert.ToInt32(mi.CommandParameter);
            bool removeResult = await UserProvider.RemoveAddress(dbc, addressID);
            if (removeResult)
            {
                for (int i = 0; i < addresses.Count; ++i)
                    if (addresses.ElementAt(i).id == addressID)
                    {
                        addresses.RemoveAt(i);
                        break;
                    }
            }
            else
                await DisplayAlert("Ошибка", "Не удалось удалить адрес.", "OK");
        }
    }
}
