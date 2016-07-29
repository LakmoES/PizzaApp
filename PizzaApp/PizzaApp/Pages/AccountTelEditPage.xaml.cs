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
    public partial class AccountTelEditPage : ContentPage
    {
        private DBConnection dbc;
        private ObservableCollection<TelNumber> telNumbers { get; set; }
        public AccountTelEditPage(DBConnection dbc, List<TelNumber> telNumbers)
        {
            InitializeComponent();
            Title = "Правка номеров";
            this.dbc = dbc;
            this.telNumbers = new ObservableCollection<TelNumber>(telNumbers);
            this.listViewTelList.ItemsSource = telNumbers;
            this.telNumbers.CollectionChanged += TelNumbers_CollectionChanged;
            this.buttonTelAdd.Clicked += ButtonTelAdd_Clicked;
        }

        private async void ButtonTelAdd_Clicked(object sender, EventArgs e)
        {
            if (!await UserProvider.AddTel(dbc, this.entryNewNumber.Text))
                await DisplayAlert("Ошибка", "Не удалось добавить номер", "OK");
            else
            {
                this.entryNewNumber.Text = String.Empty;
                this.telNumbers = new ObservableCollection<TelNumber>(await UserProvider.GetTelList(dbc));
                UpdateTelList();
            }
        }

        private void TelNumbers_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            UpdateTelList();
        }

        private void UpdateTelList()
        {
            this.listViewTelList.ItemsSource = null;
            this.listViewTelList.ItemsSource = telNumbers;
        }
        public async Task GetTelListFromServer()
        {
            this.telNumbers = new ObservableCollection<TelNumber>(await UserProvider.GetTelList(dbc));
            UpdateTelList();
        }
        public async void OnEdit(object sender, EventArgs e)
        {
            var mi = ((MenuItem)sender);
            int telID = Convert.ToInt32(mi.CommandParameter);
            await Navigation.PushAsync(new CurrentTelEditPage(dbc, telNumbers.Where(x => x.id == telID).FirstOrDefault(), this));
        }

        public async void OnDelete(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Подтверждение", "Действительно хотите удалить?", "Да", "Нет");
            if (!answer)
                return;

            var mi = ((MenuItem)sender);
            int telID = Convert.ToInt32(mi.CommandParameter);
            bool removeResult = await UserProvider.RemoveTel(dbc, telID);
            if (removeResult)
            {
                for (int i = 0; i < telNumbers.Count; ++i)
                    if (telNumbers.ElementAt(i).id == telID)
                    {
                        telNumbers.RemoveAt(i);
                        break;
                    }
                //await DisplayAlert("Успех", "Номер успешно удален.", "OK");
            }
            else
                await DisplayAlert("Ошибка", "Не удалось удалить номер.", "OK");
        }
    }
}
