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
    /// Логика взаимодействия для SupplierAddProductEditOrAddWindow.xaml
    /// </summary>
    public partial class SupplierAddProductEditOrAddWindow : Window
    {
        ViewModels.SupplierAddProductEditOrAddViewModel viewModel;

        public Supplier Supplier { get; private set; }

        public Product Product { get; private set; }

        public SupplierAddProductEditOrAddWindow(Supplier sp)
        {
            InitializeComponent();
            Supplier = sp;
            viewModel = new ViewModels.SupplierAddProductEditOrAddViewModel(sp);
            this.DataContext = viewModel;
        }

        private void Accept_Click(object sender, RoutedEventArgs e)
        {
            if(viewModel.SelectedProduct!=null)
            {
                Product = viewModel.SelectedProduct;
                this.DialogResult = true;
            }
            else
            {
                MessageBox.Show("Вы не выбрали товар!");
            }
            
        }
    }
}
