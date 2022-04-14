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
    //ViewModel для окна "Справочник ролей пользователей"
    internal class UserRoleViewModel : INotifyPropertyChanged
    {
        ApplicationContext db;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        IEnumerable<UserRole> userRoleList;
        private UserRole selectedUserRole;

        //Выбранная роль пользователя
        public UserRole SelectedUserRole
        {
            get { return selectedUserRole; }
            set
            {
                selectedUserRole = value;
                OnPropertyChanged("SelectedUserRole");
            }
        }

        //Список ролей пользователей
        public IEnumerable<UserRole> UserRoleList
        {
            get { return userRoleList; }
            set 
            { 
                userRoleList = value;
                OnPropertyChanged("UserRoleList");
            }
        }

        //Конструктор класса
        public UserRoleViewModel()
        {
            db = new ApplicationContext();
            db.UserRoles.ToList();
            userRoleList = db.UserRoles.Local.ToBindingList();
        }

        //Команда для добавления
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      //Создание диалогового окна для добавления новой роли пользователя
                      UserRoleEditOrAddWindow userRoleWindow = new UserRoleEditOrAddWindow(new UserRole());

                      if (userRoleWindow.ShowDialog() == true)
                      {
                          UserRole userRole = userRoleWindow.UserRole;
                          db.UserRoles.Add(userRole);
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
                      if (selectedItem == null)
                      {
                          MessageBox.Show("Вы не выбрали запись для редактирования!");
                          return;
                      } 
                      // получаем выделенный объект
                      UserRole userRole = selectedItem as UserRole;

                      //Создание диалогового окна для изменения информации о роли пользователей
                      UserRoleEditOrAddWindow userRoleWindow = new UserRoleEditOrAddWindow(new UserRole
                      {
                          Id = userRole.Id,
                          Name = userRole.Name,
                          Users = userRole.Users,
                      });

                      //Если диалоговое окно завершено успешно, то изменения роли пользователей сохраняются в БД
                      if(userRoleWindow.ShowDialog() == true)
                      {
                          userRole = db.UserRoles.Find((object)userRoleWindow.UserRole.Id);
                          if(userRole != null)
                          {
                              userRole.Id = userRoleWindow.UserRole.Id;
                              userRole.Name = userRoleWindow.UserRole.Name;
                              userRole.Users = userRoleWindow.UserRole.Users;
                              db.Entry(userRole).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
                      if (selectedItem == null)
                      {
                          MessageBox.Show("Вы не выбрали запись для удаления!");
                          return;
                      }
                      
                      // получаем выделенный объект
                      UserRole userRole = selectedItem as UserRole;


                      //Вызов диалогового окна для подтверждения удаления
                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить выбранный элемент?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                      if(result == MessageBoxResult.Yes)
                      {
                          db.UserRoles.Remove(userRole);
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
