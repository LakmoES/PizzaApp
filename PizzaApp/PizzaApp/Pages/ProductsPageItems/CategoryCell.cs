using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PizzaApp.Pages.ProductsPageItems
{
    class CategoryCell : ViewCell
    {
        public CategoryCell()
        {
            //instantiate each of our views
            var image = new Image();
            StackLayout cellWrapper = new StackLayout();
            StackLayout horizontalLayout = new StackLayout();
            Label left = new Label();

            //set bindings
            left.SetBinding(Label.TextProperty, "title");
            image.SetBinding(Image.SourceProperty, "image");

            //Set properties for desired design
            cellWrapper.BackgroundColor = Color.FromHex("#eee");
            System.Diagnostics.Debug.WriteLine("cellWrapper Orientation is {0}", cellWrapper.Orientation);
            horizontalLayout.Orientation = StackOrientation.Horizontal;
            left.TextColor = Color.FromHex("#f35e20");

            //add views to the view hierarchy
            horizontalLayout.Children.Add(image);
            horizontalLayout.Children.Add(left);
            cellWrapper.Children.Add(horizontalLayout);
            View = cellWrapper;
        }
    }
}
