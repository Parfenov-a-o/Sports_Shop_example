using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sport_example_3.Models;
using Sport_example_3.Views.DialogWindows;
using System.Windows;

namespace Sport_example_3.ViewModels
{
    internal class EmployeeEditOrAddViewModel : INotifyPropertyChanged
    {
        ApplicationContext db;
        private Employee employee;
        IEnumerable<PositionEmployee> positionEmployeeList;
        IEnumerable<string> genderList = new List<string>() {"Мужской", "Женский"};

        private PositionEmployee selectedPositionEmployee;

        //Выбранный сотрудник
        public Employee Employee
        {
            get { return employee; }
            set { employee = value; OnPropertyChanged("Employee"); }
        }
        
        //Список с должностями сотрудников
        public IEnumerable<PositionEmployee> PositionEmployeeList
        {
            get { return positionEmployeeList; }
            set { positionEmployeeList = value; OnPropertyChanged("PositionEmployeeList"); }
        }

        //Выбранная должность
        public PositionEmployee SelectedPositionEmployee
        {
            get { return selectedPositionEmployee; }
            set { selectedPositionEmployee = value; OnPropertyChanged("SelectedPositionEmployee"); }
        }

        //Список полов сотрудников (мужской/женский)
        public IEnumerable<string> GenderList
        {
            get { return genderList; }
            set { genderList = value; OnPropertyChanged("GenderList"); }
        }

        //Конструктор класса, принимает в качестве параемтра определенного сотрудника
        public EmployeeEditOrAddViewModel(Employee e)
        {

            db = new ApplicationContext();

            //Выгрузка всех существующих в компании должностей из БД
            positionEmployeeList = db.Positions.ToList();
            //Инициализация конкретного сотрудника
            employee = e;
            
            
            if(e.PositionEmployee!=null)
            {
                //Поиск должности в БД
                PositionEmployee position = db.Positions.Find(e.PositionEmployee.Id);
                if (position != null)
                {
                    //Если должность найдена то она устанавливается в качестве выбранной
                    selectedPositionEmployee = position;
                }
                else
                {
                    //Если должность в БД не найдена, то устанавливается первая по умолчанию
                    selectedPositionEmployee = positionEmployeeList.FirstOrDefault();
                }
            }
            else
            {
                selectedPositionEmployee = positionEmployeeList.FirstOrDefault();
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
