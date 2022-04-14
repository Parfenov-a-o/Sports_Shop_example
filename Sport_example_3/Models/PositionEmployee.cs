using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sport_example_3.Models
{

    //Должности сотрудников
    public class PositionEmployee:INotifyPropertyChanged
    {
        private string _name;


        public int Id { get; set; }
        
        //Название должности (напр. Бухгалтер)
        public string? Name 
        { 
            get { return _name; } 
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            } 
        }

        //Список всех сотрудников выбранной должности
        public List<Employee> Employees { get; set; } = new();

        
        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        
    }
}
