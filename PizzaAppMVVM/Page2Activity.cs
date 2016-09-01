using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;

namespace PizzaAppMVVM
{
    [Activity(Label = "Page2Activity")]
    public class Page2Activity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Page2);

            var button = FindViewById<Button>(Resource.Id.NavigateButton);
            button.Click += (s, e) =>
            {
                var nav = ServiceLocator.Current.GetInstance<INavigationService>();
                nav.NavigateTo(
                    MainActivity.Page3Key,
                    "Hello Xamarin " + DateTime.Now.ToString("HH:mm:ss"));
            };
        }
    }
}