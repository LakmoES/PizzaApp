using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using PizzaApp.Data.Providers;
using PizzaApp.Data.ServerEntities;

namespace PizzaApp.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            Title = "Главная";
            Page = 1;
            PageSize = 3;
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        #region Private fields

        private bool _isBusy;
        private int _page;
        private int _pageSize;
        private ICollection<Product> _products;
        #endregion

        #region Public properties
        public string Title { get; set; }
        public bool IsBusy
        {
            get { return _isBusy; }
            set { _isBusy = value;RaisePropertyChanged(() => IsBusy); }
        }
        public int Page
        {
            get { return _page; }
            set { _page = value; RaisePropertyChanged(() => Page); }
        }

        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; RaisePropertyChanged(() => PageSize); }
        }
        public ICollection<Product> Products
        {
            get { return _products; }
            set { _products = value; RaisePropertyChanged(() => Products); }
        }
        #endregion

        private ICommand _refreshProductsCommand;

        public ICommand RefreshProductsCommand
        {
            get
            {
                return _refreshProductsCommand ??
                    (_refreshProductsCommand = new RelayCommand(async () =>
                    {
                        IsBusy = true;
                        var products = await ProductProvider.GetProductPage(Page, PageSize);
                        Products = products != null ? new List<Product>(products) : null;
                        IsBusy = false;
                    }));
            }
        }
    }
}