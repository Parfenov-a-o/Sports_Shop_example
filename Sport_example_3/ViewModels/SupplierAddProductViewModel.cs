using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sport_example_3.Models;
using Sport_example_3.Views.DialogWindows;
using System.Windows;
using Microsoft.EntityFrameworkCore;

namespace Sport_example_3.ViewModels
{

    //ViewModel для окна "Поставляемые товары каждым поставщиком"
    internal class SupplierAddProductViewModel:INotifyPropertyChanged
    {
        ApplicationContext db;

        List<Supplier> _supplierList;
        List<Product> _productList;

        Supplier _selectedSupplier;
        Product _selectedProduct;

        RelayCommand _filterOnSupplierCommand;
        RelayCommand _addCommand;

        //Список поставщиков
        public List<Supplier> SupplierList
        {
            get { return _supplierList; }
            set { _supplierList = value; OnPropertyChanged("SupplierList"); }
        }

        //Выбранный поставщик
        public Supplier SelectedSupplier
        {
            get { return _selectedSupplier; }
            set { _selectedSupplier = value; OnPropertyChanged("SelectedSupplier"); }
        }

        //Выбранный товар
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged("SelectedProduct"); }
        }

        //Список товаров
        public List<Product> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged("ProductList"); }
        }

        //Конструктор класса
        public SupplierAddProductViewModel()
        {
            db = new ApplicationContext();

            //Загрузка связанных данных
            _supplierList = db.Suppliers.Include(x=>x.Products).ToList();
            _productList = new List<Product>();

            SupplierList.Insert(0, new Supplier() { Name = "Выберите поставщика", Id = 0 });
            _selectedSupplier = SupplierList.ToList()[0];

        }

        //Фильтрация товаров по выбранному поставщику
        public RelayCommand FilterOnSupplierCommand
        {
            get
            {
                return _filterOnSupplierCommand ?? (_filterOnSupplierCommand = new RelayCommand((selectedItem) =>
                {
                    if (selectedItem == null)
                    {
                        return;
                    }

                    Supplier supplier = selectedItem as Supplier;

                    //В том случае, если был выбран вариант "Выберите поставщика"
                    if(supplier.Id==0)
                    {
                        ProductList = new List<Product>();
                        return;
                    }

                    List<Product> localProducts = db.Suppliers.Find(supplier.Id).Products.ToList();


                    if (localProducts.Count == 0)
                    {
                        MessageBox.Show("У выбранного поставщика нет товаров!");
                    }
                    
                    ProductList = localProducts;

                }
                ));
            }
        }

        //Команда для добавления товара поставщику
        public RelayCommand AddCommand
        {
            get 
            {
                return _addCommand ?? (_addCommand = new RelayCommand((o) => 
                {
                    //В том случае, если выбран вариант "Выберите поставщика"
                    if(SelectedSupplier.Id==0)
                    {
                        MessageBox.Show("Вы не выбрали поставщика!");
                        return;
                    }

                    //Создание диалогового окна для добавления товара поставщику
                    SupplierAddProductEditOrAddWindow supplierAddProductWindow = new SupplierAddProductEditOrAddWindow(SelectedSupplier);

                    //Если диалоговое окно завершено успешно, то товар добавляется поставщику
                    if (supplierAddProductWindow.ShowDialog() == true)
                    {
                        db = new ApplicationContext();

                        db.Suppliers.Include(x => x.Products).ToList();
                        
                        Supplier supplier = db.Suppliers.Find(supplierAddProductWindow.Supplier.Id);

                        Product product = db.Products.Find(supplierAddProductWindow.Product.Id);

                        supplier.Products.Add(product);

                        db.SaveChanges();

                        db.Suppliers.Include(x => x.Products).ToList();
                        ProductList = db.Suppliers.Find(supplier.Id).Products.ToList();
                    }
                }));
            }
        }


        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged (this, new PropertyChangedEventArgs (prop));
            }
        }
    }
}
