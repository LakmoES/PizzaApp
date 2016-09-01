using PizzaApp.Data.Persistence;
using PizzaApp.Data.ServerConsts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PizzaApp.Pages
{
    public partial class SettingsPage : ContentPage
    {
        private DBConnection dbc;
        private IList<int> pageSizes;
        public SettingsPage(DBConnection dbc)
        {
            InitializeComponent();
            Title = "Настройки";

            this.dbc = dbc;

            string serverURLInDB = dbc.GetServerURL();
            entryServerURL.Text = serverURLInDB ?? "";

            pageSizes = new List<int> { 5, 8, 10, 12, 15, 20 };

            var pageSizeInDB = dbc.GetProductPageSize();
            int selectedIndex = -1;
            if (pageSizeInDB != null)
            {
                int foundIndex = pageSizes.IndexOf((int)pageSizeInDB);
                if (foundIndex == -1)
                {
                    pageSizes.Add((int)pageSizeInDB);
                    selectedIndex = pageSizes.Count - 1;
                }
                else
                    selectedIndex = foundIndex;
            }

            foreach (var ps in pageSizes)
                pickerPageSize.Items.Add(String.Format("{0} ед.", ps));
            if (selectedIndex != -1)
                pickerPageSize.SelectedIndex = selectedIndex;
        }
        private async void ButtonSave_Clicked(object sender, EventArgs e)
        {
            if (!SaveSettings())
                await DisplayAlert("Ошибка", "Не удалось сохранить настройки.", "OK");
        }
        private bool SaveSettings()
        {
            bool succeedFlag = true;

            if (pickerPageSize.SelectedIndex == -1)
                succeedFlag = false;
            else
                dbc.SaveProductPageSize(pageSizes[pickerPageSize.SelectedIndex]);
            if (String.IsNullOrWhiteSpace(entryServerURL.Text))
                succeedFlag = false;
            else
            {
                dbc.SaveServerUrl(entryServerURL.Text);
                ServerAddress.Url = entryServerURL.Text;
            }

            return succeedFlag;
        }
    }
}
