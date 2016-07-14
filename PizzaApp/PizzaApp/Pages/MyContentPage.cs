using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;

using Xamarin.Forms;

namespace PizzaApp.Pages
{
    public class MyContentPage : ContentPage
    {
        public MyContentPage()
        {
            var a = new Label { Text = "Hello ContentPage" };
            var b = new Button { Text = "Click me" };

            Content = new StackLayout
            {
                Children = { a, b }
            };
        }
    }
}
