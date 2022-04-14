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
    /// Логика взаимодействия для ProductPriceEditOrAddWindow.xaml
    /// </summary>
    public partial class ProductPriceEditOrAddWindow : Window
    {
        ViewModels.ProductPriceEditOrAddViewModel viewModel;

        public ProductPrice ProductPrice { get; private set; }
        public double NewPrice { get; private set; }

        public ProductPriceEditOrAddWindow(ProductPrice pp)
        {
            InitializeComponent();
            viewModel = new ViewModels.ProductPriceEditOrAddViewModel(pp);
            this.DataContext = viewModel;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {

            ProductPrice = viewModel.ProductPrice;
            NewPrice = viewModel.NewPrice;

            if (Validation.GetErrors(NewPriceTextBox).Count > 0)
            {
                MessageBox.Show("Введенные данные некорректны!");
            }
            else
            {
                this.DialogResult = true;
            }

            

        }
    }
}
