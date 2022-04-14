using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sport_example_3.Models;

namespace Sport_example_3.ViewModels
{
    //ViewModel для окна "Добавление/изменение информации о пользователе"
    internal class UserEditOrAddViewModel:INotifyPropertyChanged
    {
        ApplicationContext db;
        private User user;
        IEnumerable<UserRole> userRoleList;

        private UserRole selectedUserRole;

        //Выбранный пользователь
        public User User
        {
            get { return user; }
            set { user = value; OnPropertyChanged("User"); }
        }
        //Список ролей доступа
        public IEnumerable<UserRole> UserRoleList
        {
            get { return userRoleList; }
            set { userRoleList = value; OnPropertyChanged("UserRoleList"); }
        }
        //Выбранная роль доступа
        public UserRole SelectedUserRole
        {
            get { return selectedUserRole; }
            set { selectedUserRole = value; OnPropertyChanged("SelectedUserRole"); }
        }

        //Конструктор класса
        public UserEditOrAddViewModel(User u)
        {
            db = new ApplicationContext();

            userRoleList = db.UserRoles.ToList();
            user = u;
            if (u.UserRole != null)
            {
                UserRole role = db.UserRoles.Find(u.UserRole.Id);
                if (role != null)
                {
                    selectedUserRole = role;
                }
                else
                {
                    selectedUserRole = userRoleList.FirstOrDefault();
                }
            }
            else
            {
                selectedUserRole = userRoleList.FirstOrDefault();
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
