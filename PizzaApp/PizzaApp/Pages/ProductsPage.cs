using System;
using Xamarin.Forms;
using System.Collections.Generic;
using PizzaApp.Pages.ProductsPageItems;
using PizzaApp.Data;
using System.Linq;
using Android.Graphics;
using System.Threading.Tasks;
using PizzaApp.Data.Providers;
using PizzaApp.Data.Persistence;
using PizzaApp.Data.ServerEntities;

namespace PizzaApp.Pages
{

	public class ProductsPage : ContentPage
	{
        private int page, pageSize;
        private bool firstPage, lastPage;
        private int selectedCategoryID;
        private DBConnection dbc;
        private ListView listView;
        private Button buttonPreviousPage, buttonNextPage;
        private Label labelPages;
        private ActivityIndicator activityIndicator;

        private IEnumerable<ProductCategory> categoryList;
        private Picker pickerCategory;
        private bool showPicker;

        private List<Product> products;
        public ProductsPage(DBConnection dbc)
        {
            Title = "Товары";
            Icon = "Leads.png";

            showPicker = false;
            pickerCategory = new Picker { Title = "Выберите категорию", IsVisible = false };
            categoryList = dbc.GetCategoryList();
            foreach (var category in categoryList)
                pickerCategory.Items.Add(category.title);
            pickerCategory.SelectedIndexChanged += PickerCategory_SelectedIndexChanged;

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Поиск",
                Order = ToolbarItemOrder.Primary//,
                //Command = new Command(() => Navigation.PushAsync(new AccountPage(dbc)))
            });
            ToolbarItems.Add(new ToolbarItem
            {
                Text = "Категория",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(() => ShowPicker())
            });

            this.dbc = dbc;
            selectedCategoryID = -1;
            page = 1;
            pageSize = 8;
            firstPage = true;
            var label = new Label
            {
                Text = "Наши товары",
                HorizontalOptions = LayoutOptions.Center,
                Style = Device.Styles.SubtitleStyle
            };
            labelPages = new Label
            {
                Text = "",
                HorizontalOptions = LayoutOptions.Center,
                Style = Device.Styles.SubtitleStyle
            };

            listView = new ListView();
            listView.IsPullToRefreshEnabled = true;
            listView.ItemTemplate = new DataTemplate(typeof(ProductCell));
            listView.Refreshing += ListRefreshing;
            listView.ItemSelected += ListItemSelected;

            buttonPreviousPage = new Button { Text = "Предыдущая" };
            buttonPreviousPage.Clicked += (sender, e) => {
                DeactivateControls();
                PreviousPage();
            };
            buttonNextPage = new Button { Text = "Следующая" };
            buttonNextPage.Clicked += (sender, e) => {
                DeactivateControls();
                NextPage();
            };

            activityIndicator = new ActivityIndicator { IsVisible = false, IsRunning = false };

            Content = new StackLayout
            {
                //Padding = new Thickness(0, 20, 0, 0),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    pickerCategory,
                    activityIndicator,
                    listView,
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        Children =
                        {
                            labelPages,
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                //VerticalOptions = LayoutOptions.EndAndExpand,
                                HorizontalOptions = LayoutOptions.Center,
                                Children =
                                {
                                    buttonPreviousPage, buttonNextPage
                                }
                            }
                        }
                    }
                }
            };

            DeactivateControls();
            listView.BeginRefresh();
        }

        private void PickerCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerCategory.SelectedIndex == -1)
                return;
            selectedCategoryID = categoryList.ElementAt(pickerCategory.SelectedIndex).id;
            DeactivateControls();
            listView.BeginRefresh();
        }

        private void ShowPicker()
        {
            showPicker = !showPicker;
            pickerCategory.IsVisible = showPicker;
            if(!showPicker && selectedCategoryID != -1)
            {
                pickerCategory.SelectedIndex = -1;
                selectedCategoryID = -1;
                DeactivateControls();
                listView.BeginRefresh();
            }
        }
        private void UpdatePagesLabel(int page, int totalPages)
        {
            labelPages.Text = String.Format("Страница {0} из {1}", page, totalPages);
        }
        private void NextPage()
        {
            ++page;
            listView.BeginRefresh();
        }
        private void PreviousPage()
        {
            --page;
            listView.BeginRefresh();
        }
        private void DeactivateControls()
        {
            listView.IsEnabled = false;
            buttonNextPage.IsEnabled = false;
            buttonPreviousPage.IsEnabled = false;
            pickerCategory.IsEnabled = false;
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
        }
        private void ActivateControls()
        {
            listView.IsEnabled = true;
            pickerCategory.IsEnabled = true;
            if (!lastPage)
                buttonNextPage.IsEnabled = true;
            if (!firstPage)
                buttonPreviousPage.IsEnabled = true;
            activityIndicator.IsRunning = false;
            activityIndicator.IsVisible = false;
        }
        private async void ListRefreshing(object sender, EventArgs e)
        {
            if (await FillList())
                ActivateControls();
            else
            {
                activityIndicator.IsRunning = false;
                activityIndicator.IsVisible = false;
            }
            (sender as ListView).IsRefreshing = false;
        }
        private async void ListItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            DeactivateControls();

            var itemSourceList = (sender as ListView).ItemsSource.Cast<dynamic>().ToList();
            int index = itemSourceList.FindIndex(a => a.id == (e.SelectedItem as dynamic).id);
            
            Product p = products.ElementAt(index);
            await Navigation.PushAsync(new CurrentProductPage(dbc, p, await ShopCartProvider.ProductExists(dbc, p.id)));

            (sender as ListView).SelectedItem = null;
            ActivateControls();
        }
        private async Task<bool> FillList()
        {
            products = await ProductProvider.GetProductPage(page, pageSize, selectedCategoryID) as List<Product>;
            var pages = await ProductProvider.GetProductPageCount(pageSize, selectedCategoryID);
            if (products == null || pages == null)
            {
                await DisplayAlert("Warning", "Connection problem", "OK");
                return false;
            }
            lastPage = page >= pages;
            firstPage = page <= 1;
            UpdatePagesLabel(page, (int)pages);
            listView.ItemsSource = products.Select(a => new { id = a.id, title = a.title, subtitle = a.cost.ToString() + " грн", image = ImageSource.FromFile("icon.png") });
            return true;
        }
	}
	
}