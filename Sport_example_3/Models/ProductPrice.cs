using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sport_example_3.Models
{
    //Сущность для хранения истории изменений цен на товары
    public class ProductPrice:INotifyPropertyChanged
    {
        
        private DateTime _date;
        private double? _price;
        private int _productId;
        private Product? product;




        public int Id { get; set; }

        //Дата и время изменения цены
        public DateTime dateTime
        {
            get { return _date; }
            set { _date = value; OnPropertyChanged("dateTime"); }
        }

        //Установленная цена
        public double? Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged("Price"); }
        }

        //Код товара
        public int ProductId
        {
            get { return _productId; }
            set { _productId = value; OnPropertyChanged("ProductId"); }
        }

        //Ссылка на товар
        public Product? Product
        {
            get { return product; }
            set { product = value; OnPropertyChanged("Product"); }
        }

        
        
        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop="")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
