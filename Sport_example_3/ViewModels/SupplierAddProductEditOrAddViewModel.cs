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
    //ViewModel для диалогового окна "Добавление поставляемых товаров поставщику"
    internal class SupplierAddProductEditOrAddViewModel:INotifyPropertyChanged
    {
        ApplicationContext db;
        List<ProductCategory> _productCategoryList;
        List<Product> _productList;
        
        ProductCategory _selectedProductCategory;
        Product _selectedProduct;
        
        RelayCommand filterProductCommand;

        //Выбранный поставщик
        public Supplier Supplier { get; set; }

        //Список категорий товаров
        public List<ProductCategory> ProductCategoryList
        {
            get { return _productCategoryList; }
            set { _productCategoryList = value; OnPropertyChanged("ProductCategoryList"); }
        }

        //Выбранная категория
        public ProductCategory SelectedProductCategory
        {
            get { return _selectedProductCategory; }
            set { _selectedProductCategory = value; OnPropertyChanged("SelectedProductCategory"); }
        }

        //Список товаров
        public List<Product> ProductList
        {
            get { return _productList; }
            set { _productList = value; OnPropertyChanged("ProductList"); }
        }

        //Выбранный товар
        public Product SelectedProduct
        {
            get { return _selectedProduct; }
            set { _selectedProduct = value; OnPropertyChanged("SelectedProduct"); }
        }


        //Конструктор класса
        public SupplierAddProductEditOrAddViewModel(Supplier sp)
        {
            db = new ApplicationContext();
            db.Products.ToList();
            ProductCategoryList = db.Categories.ToList();
            ProductList = new List<Product>();

            //Загрузка связанных данных
            db.Suppliers.Include(x => x.Products).ToList();
            Supplier = db.Suppliers.Find(sp.Id);

        }


        //Фильтрация товаров по выбранной категории
        public RelayCommand FilterProductCommand
        {
            get
            {
                return filterProductCommand ??
                  (filterProductCommand = new RelayCommand((selectedItem) =>
                  {

                      if (selectedItem == null)
                      {
                          return;
                      }
                      // получаем выделенный объект
                      ProductCategory category = selectedItem as ProductCategory;
                      List<Product> localProductList = new List<Product>();

                      
                      //Перебор всех товаров категории
                      foreach(Product product in category.Products.ToList())
                      {
                          //Отобразить только те товары, которые ещё не поставляются поставщиком
                          if(!Supplier.Products.ToList().Contains(product))
                          {
                              localProductList.Add(product);
                          }
                      }

                      
                      if(localProductList.Count == 0)
                      {
                          MessageBox.Show("В категории нет доступных товаров!");
                      }
                      else
                      {
                          ProductList = localProductList;
                      }
                      
                  }));
            }
        }

        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if(PropertyChanged != null)
            { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }
    }
}
