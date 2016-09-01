﻿using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
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

        public ActivityProgressBar BusyProgressBar
        {
            get
            {
                return _busyProgressBar ?? (_busyProgressBar = FindViewById<ActivityProgressBar>(Resource.Id.MainActivityProgressBar));
            }
        }

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
    }
}

