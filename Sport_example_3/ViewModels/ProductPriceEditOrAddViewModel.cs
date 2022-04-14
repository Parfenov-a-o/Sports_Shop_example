using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sport_example_3.Models;


namespace Sport_example_3.ViewModels
{
    //ViewModel для диалогового окна "Изменение цены на товар"
    internal class ProductPriceEditOrAddViewModel:INotifyPropertyChanged
    {
        

        ProductPrice productPrice;
        double newPrice;

        
        public ProductPrice ProductPrice
        {
            get { return productPrice; }
            set { productPrice = value; OnPropertyChanged("ProductPrice"); }
        }

        //Назначенная цена
        public double NewPrice
        { get { return newPrice; } set { newPrice = value; OnPropertyChanged("NewPrice"); } }

        public ProductPriceEditOrAddViewModel(ProductPrice pp)
        {
            productPrice = pp;
        }

        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
