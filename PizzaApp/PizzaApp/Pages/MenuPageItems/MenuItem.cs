using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace PizzaApp.Pages.MenuPageItems
{

	public class MenuItem
	{
		public string Title { get; set; }

		public string IconSource { get; set; }

		public Type TargetType { get; set; }
	}
	
}