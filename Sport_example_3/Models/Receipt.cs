using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Sport_example_3.Models
{
    //Чек продажи товаров покупателям
    public class Receipt:INotifyPropertyChanged
    {
        
        private DateTime _dateTime;
        private List<ProductInBasket> productInBaskets = new();

        public int Id { get; set; }
        
        //Дата и время совершения продажи
        public DateTime DateTime
        {
            get { return _dateTime; } set { _dateTime = value; OnPropertyChanged("DateTime"); }
        }
        
        //Список товаров в чеке
        public List<ProductInBasket> ProductInBaskets
        {
            get { return productInBaskets; }
            set { productInBaskets = value; OnPropertyChanged("ProductInBaskets"); }
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
