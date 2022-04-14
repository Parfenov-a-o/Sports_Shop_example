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
    internal class EmployeeViewModel : INotifyPropertyChanged
    {
        ApplicationContext db;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        IEnumerable<Employee> employeeList;
        IEnumerable<PositionEmployee> positionEmployeeList;
        
        private Employee selectedEmployee;

        //Свойство для выбранного сотрудника
        public Employee SelectedEmployee
        {
            get { return selectedEmployee; }
            set
            {
                selectedEmployee = value;
                OnPropertyChanged("SelectedEmployee");
            }
        }

        //Список сотрудников
        public IEnumerable<Employee> EmployeeList
        {
            get { return employeeList; }
            set
            {
                employeeList = value;
                OnPropertyChanged("EmployeeList");
            }
        }

        //Список должностей сотрудников
        public IEnumerable<PositionEmployee> PositionEmployeeList
        {
            get { return positionEmployeeList; }
            set
            {
                positionEmployeeList = value;
                OnPropertyChanged("PositionEmployeeList");
            }
        }
        
        //Конструктор класса
        public EmployeeViewModel()
        {
            db = new ApplicationContext();
            
            
            db.Employees.ToList();
            db.Positions.ToList();
            employeeList = db.Employees.Local.ToBindingList();
            positionEmployeeList = db.Positions.Local.ToBindingList();

        }

        //Команда для добавления
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      //Создание диалогового окна
                      EmployeeEditOrAddWindow employeeWindow = new EmployeeEditOrAddWindow(new Employee());

                      //Если диалоговое окно завершено успешно
                      if (employeeWindow.ShowDialog() == true)
                      {
                          //Инициализируем новый объект сотрудника данными из всплывающего окна
                          Employee employee = employeeWindow.Employee;
                          
                          //Инициализируем должность сотрудника
                          employee.PositionEmployee = db.Positions.Find(employeeWindow.PositionEmployee.Id);
                          //Добавляем в контекст нового сотрудника
                          db.Employees.Add(employee);

                          //Сохраняем изменения в БД
                          db.SaveChanges();

                          
                      }


                  }));
            }
        }
        // команда редактирования
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {
                      //Если принимаемый командой параметр пуст
                      if (selectedItem == null)
                      {
                          MessageBox.Show("Вы не выбрали запись для редактирования!");
                          return;
                      }
                      // получаем выделенный объект
                      Employee employee = selectedItem as Employee;

                      //Создаем диалоговое окно
                      EmployeeEditOrAddWindow employeeWindow = new EmployeeEditOrAddWindow(new Employee
                      {
                          Id = employee.Id,
                          Surname = employee.Surname,
                          Name = employee.Name,
                          Patronymic = employee.Patronymic,
                          Fullname = employee.Fullname,
                          Gender = employee.Gender,
                          PhoneNumber = employee.PhoneNumber,
                          PositionEmployee = employee.PositionEmployee,
                          
                      });

                      //Если диалоговое окно завершено успешно, то изменяем объект сотрудника и сохраняем изменения в БД
                      if (employeeWindow.ShowDialog() == true)
                      {

                          employee = db.Employees.Find((object)employeeWindow.Employee.Id);
                          if (employee != null)
                          {
                              employee.Id = employeeWindow.Employee.Id;
                              employee.Surname = employeeWindow.Employee.Surname;
                              employee.Name = employeeWindow.Employee.Name;
                              employee.Patronymic = employeeWindow.Employee.Patronymic;
                              employee.Fullname = employeeWindow.Employee.Fullname;
                              employee.Gender = employeeWindow.Employee.Gender;
                              employee.PhoneNumber = employeeWindow.Employee.PhoneNumber;
                              employee.PositionEmployee = db.Positions.Find(employeeWindow.PositionEmployee.Id);

                              db.Entry(employee).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                              db.SaveChanges();
                          }
                      }


                  }));
            }
        }
        // команда удаления
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {

                      //Если параметр команды был пуст
                      if (selectedItem == null)
                      {
                          MessageBox.Show("Вы не выбрали запись для удаления!");
                          return;
                      }

                      // получаем выделенный объект
                      Employee employee = selectedItem as Employee;

                      //Создание окна подтверждения удаления
                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить выбранный элемент?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                      if (result == MessageBoxResult.Yes)
                      {
                          //Удаление выбранной записи
                          db.Employees.Remove(employee);
                          db.SaveChanges();
                      }


                  }));
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
