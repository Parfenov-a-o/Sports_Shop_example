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
    //ViewModel для окна "Справочник должностей сотрудников"
    internal class PositionEmployeeViewModel : INotifyPropertyChanged
    {
        ApplicationContext db;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        IEnumerable<PositionEmployee> positionEmployees;
        private PositionEmployee selectedpositionEmployee;


        //Выбранная должность
        public PositionEmployee SelectedPositionEmployee
        {
            get { return selectedpositionEmployee; }
            set
            {
                selectedpositionEmployee = value;
                OnPropertyChanged("SelectedPositionEmployee");
            }
        }

        //Список должностей
        public IEnumerable<PositionEmployee> PositionEmployees
        {
            get { return positionEmployees; }
            set 
            { 
                positionEmployees = value;
                OnPropertyChanged("PositionEmployees");
            }
        }

        //Конструктор класса
        public PositionEmployeeViewModel()
        {
            db = new ApplicationContext();
            db.Positions.ToList();
            PositionEmployees = db.Positions.Local.ToBindingList();
        }

        //Команда для добавления
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      //Создание диалогового окна для добавления новой должности
                      PositionEmployeeEditOrAddWindow positionEmployeeWindow = new PositionEmployeeEditOrAddWindow(new PositionEmployee());

                      //Если диалоговое окно завершилось успешно
                      if (positionEmployeeWindow.ShowDialog() == true)
                      {
                          //Добавление в БД новой должности
                          PositionEmployee position = positionEmployeeWindow.PositionEmployee;
                          db.Positions.Add(position);
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
                      //В том случае, если должность для редактирования не выбрана
                      if (selectedItem == null)
                      {
                          MessageBox.Show("Вы не выбрали должность для редактирования!");
                          return;
                      } 

                      // получаем выделенный объект
                      PositionEmployee position = selectedItem as PositionEmployee;

                      //Создание диалогового окна для редактирования выбранной должности
                      PositionEmployeeEditOrAddWindow positionEmployeeWindow = new PositionEmployeeEditOrAddWindow(new PositionEmployee
                      {
                          Id = position.Id,
                          Name = position.Name,
                          Employees = position.Employees,
                      });

                      //Если диалоговое окно было завершено успешно, то изменить соответствующую запись в БД
                      if(positionEmployeeWindow.ShowDialog() == true)
                      {
                          position = db.Positions.Find((object)positionEmployeeWindow.PositionEmployee.Id);
                          if(position != null)
                          {
                              position.Id = positionEmployeeWindow.PositionEmployee.Id;
                              position.Name = positionEmployeeWindow.PositionEmployee.Name;
                              position.Employees = positionEmployeeWindow.PositionEmployee.Employees;
                              db.Entry(position).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
                      //В том случае, если должность для удаления не выбрана
                      if (selectedItem == null)
                      {
                          MessageBox.Show("Вы не выбрали должность для удаления!");
                          return;
                      } 
                      // получаем выделенный объект
                      PositionEmployee employee = selectedItem as Models.PositionEmployee;

                      
                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить выбранный элемент?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                      if (result == MessageBoxResult.Yes)
                      {
                          db.Positions.Remove(employee);
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
