using PizzaApp.Data.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace PizzaApp.Pages
{
    public partial class MakingOrderPage : ContentPage
    {
        private DBConnection dbc;
        public MakingOrderPage(DBConnection dbc, string promocode = null)
        {
            InitializeComponent();
            Title = "Сделать заказ";
            this.dbc = dbc;
        }
    }
}
