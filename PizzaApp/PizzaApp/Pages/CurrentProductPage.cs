using PizzaApp.Data.ServerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PizzaApp.Pages
{
    public class CurrentProductPage : ContentPage
    {
        Label labelMain;
        public CurrentProductPage(Product product)
        {
            labelMain = new Label { Style = Device.Styles.SubtitleStyle };
            if (product == null)
                labelMain.Text = "Товар не указан";
            else
                labelMain.Text = product.ToString();
            Content = new StackLayout
            {
                Children = {
                    labelMain
                }
            };
        }
    }
}
