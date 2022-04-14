using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Sport_example_3.Models;


namespace Sport_example_3.Views.DialogWindows
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationWindow.xaml
    /// </summary>
    public partial class AuthorizationWindow : Window
    {
        public string UserRole { get; set; }
        public string Login { get; set; }

        public AuthorizationWindow()
        {
            InitializeComponent();
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            

            string login = this.LoginTextBox.Text;
            string password = this.PasswordTextBox.Password;

            using (ApplicationContext db = new ApplicationContext())
            {
                var users_list = db.Users.Where(u => ((u.Login == login) && (u.Password == password))).ToList();
                var user_roles_list = db.UserRoles.ToList();

                if (users_list.Count > 0)
                {
                    

                    switch(users_list[0].UserRole.Name)
                    {
                        case "Администратор":
                            UserRole = "Администратор";
                            break;
                        case "Менеджер по продажам":
                            UserRole = "Менеджер по продажам";
                            break;
                        case "Менеджер по закупкам":
                            UserRole = "Менеджер по закупкам";
                            break;

                    }
                    Login = login;
                    this.DialogResult = true;

                }
                else
                {
                    MessageBox.Show("Вы ввели неправильный логин или пароль");
                }
            }
        }
    }
}
