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


            //Employee = e;
            viewmodel = new ViewModels.EmployeeEditOrAddViewModel(e);
            this.DataContext = viewmodel;

            //Employee = viewmodel.Employee;
            //PositionEmployee = viewmodel.SelectedPositionEmployee;

            //Employee.PositionEmployee = viewmodel.SelectedPositionEmployee;
            //Employee.PositionEmployee.Id = viewmodel.SelectedPositionEmployee.Id;
            //this.DataContext = Employee;
            //this.EmployeePositionComboBox.ItemsSource = PositionEmployeeList;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Employee = viewmodel.Employee;
            PositionEmployee = viewmodel.SelectedPositionEmployee;
            //ApplicationContext db = new ApplicationContext();

            //PositionEmployee = db.Positions.Find(viewmodel.SelectedPositionEmployee.Id);

            this.DialogResult = true;
        }
    }
}
