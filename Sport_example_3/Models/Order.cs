using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Sport_example_3.Models
{

    //Сущность Заказ (для хранения всех произведенных закупок товаров)
    public class Order:INotifyPropertyChanged
    {
        private DateTime _dateTime;
        private List<ProductInOrderBasket> _productInOrderBaskets = new();


        public int Id { get; set; }
        
        //Дата и время заказа
        public DateTime DateTime
        {
            get { return _dateTime; }
            set
            {
                _dateTime = value; OnPropertyChanged("DateTime");
            }
        }

        //Список товаров в заказе
        public List<ProductInOrderBasket> ProductInOrderBaskets
        {
            get { return _productInOrderBaskets; }
            set { _productInOrderBaskets = value; OnPropertyChanged("ProductInOrderBaskets"); }
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
