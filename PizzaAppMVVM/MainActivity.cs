using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using customviews;
using GalaSoft.MvvmLight.Helpers;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using PizzaApp.Data.Persistence;
using PizzaApp.ViewModel;
using PizzaApp.Data.ServerConsts;
using PizzaApp.Data.ServerEntities;
using PizzaApp.Model;
using PizzaAppMVVM.Persistence;

namespace PizzaAppMVVM
{
    [Activity(Label = "PizzaAppMVVM", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : ActivityBase
    {
        public DBConnection dbc;
        public const string Page2Key = "Page2";
        public const string Page3Key = "Page3";

        private static bool _initialized;

        private readonly List<Binding> bindings = new List<Binding>();

        private MainViewModel Vm
        {
            get
            {
                return App.Locator.Main;
            }
        }

        private ActivityProgressBar _busyProgressBar;
        public ActivityProgressBar BusyProgressBar => _busyProgressBar 
            ?? (_busyProgressBar = FindViewById<ActivityProgressBar>(Resource.Id.MainActivityProgressBar));

        private ListView listViewProducts;
        public ListView ListViewProducts => listViewProducts 
            ?? (listViewProducts = FindViewById<ListView>(Resource.Id.ListViewProducts));

        private Button _mainButton;

        public Button MainButton => _mainButton ?? (_mainButton = FindViewById<Button>(Resource.Id.MyButton));

        protected override void OnCreate(Bundle bundle)
        {
            this.dbc = new DBConnection(new SQLiteDroid().GetConnection());
            ServerAddress.Url = dbc.GetServerURL() ?? "http://asp-lakmoes.tk";

            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            if (!_initialized)
            {
                _initialized = true;
                ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

                var nav = new NavigationService();
                nav.Configure(Page2Key, typeof(Page2Activity));
                nav.Configure(Page3Key, typeof(Page3Activity));

                SimpleIoc.Default.Register<INavigationService>(() => nav);
            }
            //Vm.UpdateProducts();
            ListViewProducts.Adapter = Vm.Products.GetAdapter(GetProductAdapter);

            //var button = FindViewById<Button>(Resource.Id.MyButton);
            //button.Click += (s, e) =>
            //{
            //    ServiceLocator.Current.GetInstance<INavigationService>().NavigateTo(Page2Key);
            //};

            bindings.Add(
                this.SetBinding(
                    () => Vm.Title,
                    () => this.Title
                    )
                );

            bindings.Add(
                this.SetBinding(
                    () => Vm.IsBusy,
                    () => BusyProgressBar.IsVisible
                    )
                );

            MainButton.SetCommand("Click", Vm.RefreshProductsCommand);
        }
        private View GetProductAdapter(int position, ProductWithImage product, View convertView)
        {
            // Not reusing views here
            convertView = LayoutInflater.Inflate(Resource.Layout.ProductTemplate, null);

            var title = convertView.FindViewById<TextView>(Resource.Id.TextViewTitle);
            title.Text = product.Product.title;

            var cost = convertView.FindViewById<TextView>(Resource.Id.TextViewSubtitle);
            cost.Text = $"{product.Product.cost} грн/{product.Product.measure}";

            var advertising = convertView.FindViewById<TextView>(Resource.Id.TextViewAdvertising);
            advertising.Text = product.Product.advertising == 0 ? "" : "Акция!";

            var available = convertView.FindViewById<TextView>(Resource.Id.TextViewAvailable);
            available.Text = product.Product.available == 1 ? "" : "Нет в наличии";

            var image = convertView.FindViewById<ImageView>(Resource.Id.ImageViewImage);

            return convertView;
        }
    }
}

