using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PizzaApp.Pages.ProductsPageItems
{
    public class ProductCell : ViewCell
    {
        public ProductCell()
        {
            //instantiate each of our views
            var image = new Image();
            StackLayout cellWrapper = new StackLayout();
            StackLayout horizontalLayout = new StackLayout();
            StackLayout verticalLayout = new StackLayout();
            Label labelTop = new Label();
            Label labelBottom = new Label();

            //set bindings
            labelTop.SetBinding(Label.TextProperty, "title");
            labelBottom.SetBinding(Label.TextProperty, "subtitle");
            image.SetBinding(Image.SourceProperty, "image");

            //Set properties for desired design
            cellWrapper.BackgroundColor = Color.FromHex("#eee");
            System.Diagnostics.Debug.WriteLine("cellWrapper Orientation is {0}", cellWrapper.Orientation);
            horizontalLayout.Orientation = StackOrientation.Horizontal;
            verticalLayout.Orientation = StackOrientation.Vertical;
            labelBottom.HorizontalOptions = LayoutOptions.EndAndExpand;
            labelTop.TextColor = Color.FromHex("#f35e20");
            labelBottom.TextColor = Color.FromHex("503026");

            //add views to the view hierarchy
            horizontalLayout.Children.Add(image);
            horizontalLayout.Children.Add(verticalLayout);
            verticalLayout.Children.Add(labelTop);
            verticalLayout.Children.Add(labelBottom);
            cellWrapper.Children.Add(horizontalLayout);
            View = cellWrapper;
        }
    }
}
