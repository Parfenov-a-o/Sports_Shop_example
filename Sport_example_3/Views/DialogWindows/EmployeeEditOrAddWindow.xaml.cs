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
    /// Логика взаимодействия для EmployeeEditOrAddWindow.xaml
    /// </summary>
    public partial class EmployeeEditOrAddWindow : Window
    {
        ViewModels.EmployeeEditOrAddViewModel viewmodel;

        public Employee Employee { get; private set; }

        public PositionEmployee PositionEmployee { get; private set; }

        public EmployeeEditOrAddWindow(Employee e)
        {
            InitializeComponent();


            viewmodel = new ViewModels.EmployeeEditOrAddViewModel(e);
            this.DataContext = viewmodel;

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Employee = viewmodel.Employee;
            PositionEmployee = viewmodel.SelectedPositionEmployee;

            this.DialogResult = true;
        }
    }
}
