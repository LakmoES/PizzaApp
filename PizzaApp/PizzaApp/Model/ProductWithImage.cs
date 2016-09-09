using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Microsoft.VisualBasic.CompilerServices;
using PizzaApp.Data.ServerEntities;

namespace PizzaApp.Model
{
    public class ProductWithImage : ObservableObject
    {
        public Product Product { get; set; }
        public ProductImageUrls Image { get; set; }
        public static implicit operator ProductWithImage(Product product)
        {
            return new ProductWithImage { Product = product };
        }
        private byte[] _smallBytes { get; set; }
        public byte[] SmallBytes
        {
            get { return _smallBytes; }
            set { _smallBytes = value; RaisePropertyChanged(() => SmallBytes); }
            //set { Set(() => SmallBytes, ref _smallBytes, value); }
        }
    }
}
