using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Sport_example_3.Models
{
    //Товары в покупательской корзине
    public class ProductInBasket : INotifyPropertyChanged
    {
        private int _indexNumber;
        private double _count;
        private double? _price;
        private double? _sum;
        private int _productId;
        private Product? _product;

        public int Id { get; set; }

        //Номер позиции товара в чеке
        public int IndexNumber
        {
            get { return _indexNumber; }
            set { _indexNumber = value; OnPropertyChanged("IndexNumber"); }
        }

        //Количество товара
        public double Count
        { get { return _count; } set { _count = value; OnPropertyChanged("Count");} }

        //Цена товара
        public double? Price
        { get { return _price; } set { _price = value; OnPropertyChanged("Price"); } }

        //Сумма
        public double? Sum
        {
            get { return _sum; } set { _sum = value; OnPropertyChanged("Sum"); }
        }

        //Код товара
        public int ProductId
        {
            get { return _productId; } set { _productId = value; OnPropertyChanged("ProductId"); }
        }

        //Ссылка на товар
        public Product? Product
        {
            get { return _product; } set { _product = value; OnPropertyChanged("Product"); }
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
