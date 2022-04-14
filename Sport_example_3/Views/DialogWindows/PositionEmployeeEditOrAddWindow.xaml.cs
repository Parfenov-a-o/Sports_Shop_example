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
    /// Логика взаимодействия для PositionEmployeeEditOrAddWindow.xaml
    /// </summary>
    public partial class PositionEmployeeEditOrAddWindow : Window
    {
        public PositionEmployee PositionEmployee { get; private set; }

        
        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public PositionEmployeeEditOrAddWindow(PositionEmployee pe)
        {
            InitializeComponent();

            PositionEmployee = pe;
            this.DataContext = PositionEmployee;
        }

    }
}
