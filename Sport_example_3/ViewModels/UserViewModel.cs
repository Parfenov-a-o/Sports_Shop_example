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
    //ViewModel для окна "Справочник пользователей"
    internal class UserViewModel:INotifyPropertyChanged
    {

        ApplicationContext db;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        IEnumerable<User> userList;
        IEnumerable<UserRole> userRoleList;

        private User selectedUser;

        //Выбранный пользователь
        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        //Список пользователей
        public IEnumerable<User> UserList
        {
            get { return userList; }
            set
            {
                userList = value;
                OnPropertyChanged("UserList");
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
        public UserViewModel()
        {
            db = new ApplicationContext();


            db.Users.ToList();
            db.UserRoles.ToList();
            userList = db.Users.Local.ToBindingList();
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
                      UserEditOrAddWindow userWindow = new UserEditOrAddWindow(new User());

                      if (userWindow.ShowDialog() == true)
                      {
                          User user = userWindow.User;
                          user.UserRole = db.UserRoles.Find(userWindow.UserRole.Id);
                          db.Users.Add(user);
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
                      User user = selectedItem as User;

                      UserEditOrAddWindow userWindow = new UserEditOrAddWindow(new User
                      {
                          Id = user.Id,
                          Login = user.Login,
                          Password = user.Password,
                          UserRole = user.UserRole,
                          

                      });

                      if (userWindow.ShowDialog() == true)
                      {
                          user = db.Users.Find((object)userWindow.User.Id);
                          if (user != null)
                          {

                              user.Id = userWindow.User.Id;
                              user.Login = userWindow.User.Login;
                              user.Password = userWindow.User.Password;
                              user.UserRole = db.UserRoles.Find(userWindow.UserRole.Id);



                              db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
                      User user = selectedItem as User;

                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить выбранный элемент?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                      if (result == MessageBoxResult.Yes)
                      {
                          db.Users.Remove(user);
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
