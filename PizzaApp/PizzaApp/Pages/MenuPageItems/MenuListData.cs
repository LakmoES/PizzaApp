using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace PizzaApp.Pages.MenuPageItems
{

	public class MenuListData : List<MenuItem>
	{
		public MenuListData ()
		{
			this.Add (new MenuItem () { 
				Title = "Главная", 
				IconSource = "contacts.png", 
				TargetType = typeof(MainPage)
			});

			this.Add (new MenuItem () { 
				Title = "Товары", 
				IconSource = "leads.png", 
				TargetType = typeof(ProductsPage)
			});

			this.Add (new MenuItem () { 
				Title = "Accounts", 
				IconSource = "accounts.png", 
				TargetType = typeof(AccountsPage)
			});

			this.Add (new MenuItem () {
				Title = "Opportunities",
				IconSource = "opportunities.png",
				TargetType = typeof(OpportunitiesPage)
			});
		}
	}
}