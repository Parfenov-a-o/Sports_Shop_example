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
    /// Логика взаимодействия для PricesWindow.xaml
    /// </summary>
    public partial class PricesWindow : Window
    {
        public PricesWindow()
        {
            InitializeComponent();

            this.DataContext =  new ViewModels.PricesViewModel();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
