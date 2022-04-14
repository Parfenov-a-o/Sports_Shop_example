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

namespace Sport_example_3.Views
{
    /// <summary>
    /// Логика взаимодействия для SupplierAddProductWindow.xaml
    /// </summary>
    public partial class SupplierAddProductWindow : Window
    {
        

        public SupplierAddProductWindow()
        {
            InitializeComponent();

            this.DataContext = new ViewModels.SupplierAddProductViewModel();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();

            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
