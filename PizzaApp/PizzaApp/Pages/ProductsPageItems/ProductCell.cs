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
            Label labelID = new Label { IsVisible = true };

            //set bindings
            labelTop.SetBinding(Label.TextProperty, "title");
            labelBottom.SetBinding(Label.TextProperty, "subtitle");
            labelID.SetBinding(Label.TextProperty, "id");
            image.SetBinding(Image.SourceProperty, "image");

            //Set properties for desired design
            cellWrapper.BackgroundColor = Color.FromHex("#eee");
            horizontalLayout.Orientation = StackOrientation.Horizontal;
            verticalLayout.Orientation = StackOrientation.Vertical;
            labelTop.TextColor = Color.FromHex("#f35e20");
            labelTop.Style = Device.Styles.TitleStyle;
            labelBottom.TextColor = Color.FromHex("503026");
            labelBottom.Style = Device.Styles.SubtitleStyle;

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
