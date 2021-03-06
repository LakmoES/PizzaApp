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
        private string selectedProductName;

        private DBConnection dbc;
        private ListView listView;
        private Button buttonPreviousPage, buttonNextPage;
        private Label labelPages;
        private ActivityIndicator activityIndicator;

        private Button buttonSort;
        private bool orderDesc;
        private char orderBy;
        private bool orderNeed;

        private StackLayout stackLayoutProductName;
        private Entry entryProductName;
        private Button buttonProductName;
        private bool showProductName;

        private IEnumerable<ProductCategory> categoryList;
        private Picker pickerCategory;
        private bool showProductCategory;

        private List<Product> products;
        public ProductsPage(DBConnection dbc)
        {
            Title = "������";
            Icon = "Leads.png";

            showProductCategory = false;
            pickerCategory = new Picker { Title = "�������� ���������", IsVisible = false };
            categoryList = dbc.GetCategoryList();
            foreach (var category in categoryList)
                pickerCategory.Items.Add(category.title);
            pickerCategory.SelectedIndexChanged += PickerCategory_SelectedIndexChanged;

            showProductName = false;
            entryProductName = new Entry { Placeholder = "������� ����� ��������", HorizontalOptions = LayoutOptions.Fill };
            buttonProductName = new Button { Text = "�����", HorizontalOptions = LayoutOptions.EndAndExpand };
            buttonProductName.Clicked += ButtonProductName_Clicked;
            stackLayoutProductName = new StackLayout
            {
                IsVisible = false,
                Orientation = StackOrientation.Horizontal,
                Children =
                {
                    entryProductName,
                    buttonProductName
                }
            };

            ToolbarItems.Add(new ToolbarItem
            {
                Text = "�����",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(() => ShowEntry())
            });
            ToolbarItems.Add(new ToolbarItem
            {
                Text = "���������",
                Order = ToolbarItemOrder.Primary,
                Command = new Command(() => ShowPicker())
            });

            this.dbc = dbc;
            selectedCategoryID = -1;
            selectedProductName = null;
            page = 1;
            var pageSizeInDB = dbc.GetProductPageSize();
            if (pageSizeInDB != null)
                pageSize = (int)pageSizeInDB;
            else
                pageSize = 8;
            firstPage = true;
            var label = new Label
            {
                Text = "���� ������",
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

            buttonPreviousPage = new Button { Text = "����������" };
            buttonPreviousPage.Clicked += (sender, e) => {
                DeactivateControls();
                PreviousPage();
            };
            buttonNextPage = new Button { Text = "���������" };
            buttonNextPage.Clicked += (sender, e) => {
                DeactivateControls();
                NextPage();
            };

            buttonSort = new Button { Text = "����.", HorizontalOptions = LayoutOptions.EndAndExpand };
            buttonSort.Clicked += ButtonSort_Clicked;
            orderDesc = false;
            orderBy = 'c';
            orderNeed = false;

            activityIndicator = new ActivityIndicator { IsVisible = false, IsRunning = false };

            Content = new StackLayout
            {
                //Padding = new Thickness(0, 20, 0, 0),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    stackLayoutProductName,
                    pickerCategory,
                    activityIndicator,
                    listView,
                    new StackLayout
                    {
                        VerticalOptions = LayoutOptions.EndAndExpand,
                        Children =
                        {
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
                                HorizontalOptions = LayoutOptions.Center,
                                Children =
                                {
                                    labelPages, buttonSort
                                }
                            },
                            new StackLayout
                            {
                                Orientation = StackOrientation.Horizontal,
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

        private void ButtonSort_Clicked(object sender, EventArgs e)
        {
            ListViewSort();
            ActivateControls();
        }

        private void ShowEntry()
        {
            ChangePickerVisible(true);
            ChangeEntryVisible();
        }
        private void ShowPicker()
        {
            ChangeEntryVisible(true);
            ChangePickerVisible();
        }
        private void ChangeEntryVisible(bool disable = false)
        {
            
            showProductName = !showProductName;
            if (disable)
                showProductName = false;
            stackLayoutProductName.IsVisible = showProductName;
            if (!showProductName && selectedProductName != null)
            {
                entryProductName.Text = String.Empty;
                selectedProductName = null;
                DeactivateControls();
                listView.BeginRefresh();
            }
        }
        private void ChangePickerVisible(bool disable = false)
        {
            showProductCategory = !showProductCategory;
            if (disable)
                showProductCategory = false;
            pickerCategory.IsVisible = showProductCategory;
            if (!showProductCategory && selectedCategoryID != -1)
            {
                pickerCategory.SelectedIndex = -1;
                selectedCategoryID = -1;
                DeactivateControls();
                listView.BeginRefresh();
            }
        }
        private void ButtonProductName_Clicked(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(entryProductName.Text))
            {
                selectedProductName = null;
                return;
            }
            selectedProductName = entryProductName.Text;
            page = 1;
            DeactivateControls();
            listView.BeginRefresh();
        }

        private void PickerCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (pickerCategory.SelectedIndex == -1)
                return;
            selectedCategoryID = categoryList.ElementAt(pickerCategory.SelectedIndex).id;
            page = 1;
            DeactivateControls();
            listView.BeginRefresh();
        }
        private void UpdatePagesLabel(int page, int totalPages)
        {
            labelPages.Text = String.Format("�������� {0} �� {1}", page, totalPages);
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
            entryProductName.IsEnabled = false;
            buttonProductName.IsEnabled = false;
            buttonSort.IsEnabled = false;
            activityIndicator.IsRunning = true;
            activityIndicator.IsVisible = true;
        }
        private void ActivateControls()
        {
            listView.IsEnabled = true;
            pickerCategory.IsEnabled = true;
            entryProductName.IsEnabled = true;
            buttonProductName.IsEnabled = true;
            buttonSort.IsEnabled = true;
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
            if (!listView.IsEnabled)
            {
                listView.SelectedItem = null;
                return;
            }

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
            int? pages = null;
            if (String.IsNullOrWhiteSpace(selectedProductName))
            {
                products = await ProductProvider.GetProductPage(page, pageSize, selectedCategoryID, orderNeed ? orderBy : (char?)null, orderNeed ? orderDesc : (bool?)null) as List<Product>;
                pages = await ProductProvider.GetProductPageCount(pageSize, selectedCategoryID);
            }
            else
            {
                products = await ProductProvider.GetProductPageByName(selectedProductName, page, pageSize) as List<Product>;
                pages = await ProductProvider.GetProductPageByNameCount(selectedProductName, pageSize);
            }
            if (products == null || pages == null)
            {
                await DisplayAlert("Warning", "Connection problem", "OK");
                return false;
            }
            lastPage = page >= pages;
            firstPage = page <= 1;
            UpdatePagesLabel(page, (int)pages);
            listView.ItemsSource = products.Select(a => new { id = a.id, title = a.title, subtitle = a.cost.ToString() + " ���", image = ImageSource.FromFile("icon.png") });
            return true;
        }
        private void ListViewSort()
        {
            if (!orderNeed)
                orderDesc = false;
            else
                orderDesc = !orderDesc;
            orderNeed = true;

            DeactivateControls();
            listView.BeginRefresh();
        }
	}
	
}