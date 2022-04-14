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
    /// Логика взаимодействия для ProductEditOrAddWindow.xaml
    /// </summary>
    public partial class ProductEditOrAddWindow : Window
    {
        ViewModels.ProductEditOrAddViewModel viewmodel;

        public Product Product { get; private set; }

        public ProductCategory ProductCategory { get; private set; }

        public double? InitialPrice { get; private set; }

        public bool HasErrors { get; set; }


        public ProductEditOrAddWindow(Product p)
        {
            InitializeComponent();

            viewmodel = new ViewModels.ProductEditOrAddViewModel(p);
            this.DataContext = viewmodel;

        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            Product = viewmodel.Product;
            ProductCategory = viewmodel.SelectedProductCategory;
            InitialPrice = viewmodel.InitialPrice;

            if(Validation.GetErrors(CountInStorageTextBox).Count>0 || Validation.GetErrors(InitialPriceTextBox).Count > 0)
            {
                MessageBox.Show("Введенные данные некорректны!");
            }
            else
            {
                this.DialogResult = true;
            }

            
            
        }

        private void TextBox_Error(object sender, ValidationErrorEventArgs e)
        {

            
        }
    }
}
