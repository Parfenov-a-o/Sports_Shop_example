using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sport_example_3.Models
{

    public class Employee:INotifyPropertyChanged
    {
        private string? _surname;
        private string? _name;
        private string? _patronymic;
        private string? _fullname;
        private string? _gender;
        private string? _phoneNumber;

        private int _positionEmployeeId;
        private PositionEmployee? _positionEmployee;



        public int Id { get; set; }
        
        //Фамилия сотрудника
        public string? Surname 
        { 
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged("Surname");
            } 
        }

        //Имя сотрудника
        public string? Name 
        {
            get { return _name; }
            set 
            {
                _name = value;
                OnPropertyChanged("Name");

            } 
        }
        
        //Отчество сотрудника
        public string? Patronymic 
        {
            get { return _patronymic; }
            set 
            {
                _patronymic = value;
                OnPropertyChanged("Patronymic");
            } 
        }

        //ФИО сотрудника
        public string? Fullname 
        { 
            get { return _fullname; }
            set { _fullname = _surname + " " + _name + " " + _patronymic; OnPropertyChanged("Fullname"); }
        }

        //Пол сотрудника (мужской/женский)
        public string? Gender 
        { 
            get { return _gender; }
            set
            {
                _gender = value;
                OnPropertyChanged("Gender");
            }
        }

        //Контактный телефон
        public string? PhoneNumber 
        {
            get { return _phoneNumber; }
            set
            {
                _phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }

        //Код должности сотрудника
        public int PositionEmployeeId 
        {
            get { return _positionEmployeeId; }
            set
            {
                _positionEmployeeId = value;
                OnPropertyChanged("PositionEmployeeId");
            }
        }

        //Должность сотрудника 
        public PositionEmployee? PositionEmployee 
        {
            get { return _positionEmployee; }
            set
            {
                _positionEmployee = value;
                OnPropertyChanged("PositionEmployee");
            }
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
