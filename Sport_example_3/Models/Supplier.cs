using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sport_example_3.Models
{

    //Сущность поставщик
    public class Supplier:INotifyPropertyChanged
    {
        private string? _name;
        private string? _personalAccount;
        private string? _address;
        private string? _phoneNumber;

        public int Id { get; set; }

        //Название организации - поставщика
        public string? Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        //Номер личного счёта
        public string? PersonalAccount
        {
            get { return _personalAccount; }
            set { _personalAccount = value; OnPropertyChanged("PersonalAccount"); }
        }

        //Адрес
        public string? Address
        {
            get { return _address; }
            set { _address = value; OnPropertyChanged("Address"); }
        }

        //Номер контактного телефона
        public string? PhoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; OnPropertyChanged("PhoneNumber"); }
        }

        //Список поставляемых продуктов
        public List<Product> Products { get; set; } = new();



        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
