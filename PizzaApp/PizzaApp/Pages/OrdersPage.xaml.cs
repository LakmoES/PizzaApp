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
    public partial class OrdersPage : ContentPage
    {
        private bool firstPage, lastPage;
        private int page, pageSize;
        private ObservableCollection<Order> orders;
        private DBConnection dbc;
        public OrdersPage(DBConnection dbc)
        {
            Title = "Ваши заказы";
            InitializeComponent();
            this.dbc = dbc;

            page = 1;
            pageSize = 8;
            
            listViewOrders.BeginRefresh();
        }
        private void DeactivateControls()
        {
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;

            listViewOrders.IsEnabled = false;
            buttonNextPage.IsEnabled = false;
            buttonPreviousPage.IsEnabled = false;
        }
        private void ActivateControls()
        {
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;

            listViewOrders.IsEnabled = true;
            buttonNextPage.IsEnabled = true;
            buttonPreviousPage.IsEnabled = true;

        }
        private void UpdatePagesLabel(int page, int totalPages)
        {
            labelPages.Text = String.Format("Страница {0} из {1}", page, totalPages);
        }
        private async void ListViewOrders_Refreshing(object sender, EventArgs e)
        {
            DeactivateControls();
            await UpdateOrders();
        }
        private void ButtonPreviousPage_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            PreviousPage();
        }
        private void ButtonNextPage_Clicked(object sender, EventArgs e)
        {
            DeactivateControls();
            NextPage();
        }
        private void PreviousPage()
        {
            --page;
            listViewOrders.BeginRefresh();
        }
        private void NextPage()
        {
            ++page;
            listViewOrders.BeginRefresh();
        }

        public async Task UpdateOrders()
        {
            var orders = await OrderProvider.GetPage(dbc, page, pageSize);
            var totalPages = await OrderProvider.GetPageCount(dbc, pageSize);
            if (orders == null || totalPages == null)
            {
                this.orders = null;

                listViewOrders.IsRefreshing = false;
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;

                await DisplayAlert("Ошибка", "Не удалось связаться с сервером.", "OK");
                return;
            }
            this.orders = new ObservableCollection<Order>(orders);

            UpdatePagesLabel(page, (int)totalPages);
            listViewOrders.ItemsSource = this.orders;

            listViewOrders.IsRefreshing = false;
            ActivateControls();

            firstPage = page <= 1;
            lastPage = page >= totalPages;
            buttonNextPage.IsEnabled = !lastPage;
            buttonPreviousPage.IsEnabled = !firstPage;
        }
    }
}
