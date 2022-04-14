using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sport_example_3.Models
{
    //Товары в корзине закупок у поставщиков, наследуется от класса "товары в покупательской корзине"
    public class ProductInOrderBasket:ProductInBasket, INotifyPropertyChanged
    {
        private int _supplierId;
        private Supplier? _supplier;

        //Код поставщика
        public int SupplierId
        {
            get { return _supplierId; }
            set { _supplierId = value; OnPropertyChanged("SupplierId"); }
        }
        //Ссылка на поставщика
        public Supplier? Supplier
        {
            get { return _supplier; }
            set { _supplier = value; OnPropertyChanged("Supplier"); }
        }


        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
