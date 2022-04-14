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
    /// Логика взаимодействия для UserEditOrAddWindow.xaml
    /// </summary>
    public partial class UserEditOrAddWindow : Window
    {
        ViewModels.UserEditOrAddViewModel viewmodel;

        public User User { get; private set; }

        public UserRole UserRole { get; private set; }


        public UserEditOrAddWindow(User u)
        {
            InitializeComponent();

            viewmodel = new ViewModels.UserEditOrAddViewModel(u);
            this.DataContext = viewmodel;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            User = viewmodel.User;
            UserRole = viewmodel.SelectedUserRole;

            this.DialogResult = true;
        }
    }
}
