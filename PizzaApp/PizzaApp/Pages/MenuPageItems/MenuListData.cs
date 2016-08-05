using System;
using Xamarin.Forms;
using System.Collections.Generic;
using PizzaApp.Data.Persistence;

namespace PizzaApp.Pages.MenuPageItems
{

	public class MenuListData : List<MenuItem>
	{
		public MenuListData (DBConnection dbc)
		{
			this.Add (new MenuItem () { 
				Title = "�������", 
				IconSource = "contacts.png", 
				TargetType = typeof(MainPage)
			});

			this.Add (new MenuItem () { 
				Title = "������", 
				IconSource = "leads.png", 
				TargetType = typeof(ProductsPage)
			});

            this.Add(new MenuItem()
            {
                Title = "�������",
                IconSource = "accounts.png",
                TargetType = typeof(ShopCartPage)
            });

            this.Add (new MenuItem () { 
				Title = "�������", 
				IconSource = "accounts.png", 
				TargetType = typeof(AccountPage)
			});

			this.Add (new MenuItem () {
				Title = "Opportunities",
				IconSource = "opportunities.png",
				TargetType = typeof(OpportunitiesPage)
			});
		}
	}
}