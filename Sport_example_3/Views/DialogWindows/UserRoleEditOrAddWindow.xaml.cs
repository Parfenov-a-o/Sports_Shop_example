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
    /// Логика взаимодействия для UserRoleEditOrAddWindow.xaml
    /// </summary>
    public partial class UserRoleEditOrAddWindow : Window
    {
        public UserRole UserRole { get; private set; }

        public UserRoleEditOrAddWindow(UserRole ur)
        {
            InitializeComponent();
            UserRole = ur;
            this.DataContext = UserRole;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
