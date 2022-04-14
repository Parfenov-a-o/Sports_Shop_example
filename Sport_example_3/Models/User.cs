using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Sport_example_3.Models
{
    //Сущность - пользователь
    public class User:INotifyPropertyChanged
    {
        private string? _login;
        private string? _password;
        private int _userRoleId;
        private UserRole? _userRole;

        public int Id { get; set; }

        //Логин пользователя
        public string? Login
        {
            get { return _login; }
            set { _login = value; OnPropertyChanged("Login"); }
        }

        //Пароль пользователя
        public string? Password
        {
            get { return _password; }
            set { _password = value; OnPropertyChanged("Password"); }
        }

        //Код роли доступа
        public int UserRoleId
        {
            get { return _userRoleId; }
            set { _userRoleId = value; OnPropertyChanged("UserRoleId"); }
        }

        //Ссылка на роль доступа
        public UserRole? UserRole
        {
            get { return _userRole; }
            set { _userRole = value; OnPropertyChanged("UserRole"); }
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
