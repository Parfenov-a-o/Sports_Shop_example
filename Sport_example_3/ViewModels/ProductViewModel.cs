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

namespace Sport_example_3.ViewModels
{
    //ViewModel для окна "Справочник товаров"
    internal class ProductViewModel:INotifyPropertyChanged
    {
        ApplicationContext db;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        IEnumerable<Product> productList;
        IEnumerable<ProductCategory> productCategoryList;

        private Product selectedProduct;

        //Выбранный товар
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        //Список товаров
        public IEnumerable<Product> ProductList
        {
            get { return productList; }
            set
            {
                productList = value;
                OnPropertyChanged("ProductList");
            }
        }

        //Список категорий товаров
        public IEnumerable<ProductCategory> ProductCategoryList
        {
            get { return productCategoryList; }
            set
            {
                productCategoryList = value;
                OnPropertyChanged("ProductCategoryList");
            }
        }

        //Конструктор класса
        public ProductViewModel()
        {
            db = new ApplicationContext();


            db.Products.ToList();
            db.Categories.ToList();
            productList = db.Products.Local.ToBindingList();
            productCategoryList = db.Categories.Local.ToBindingList();

        }

        //Команда для добавления
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      //Создание диалогового окна для добавлении нового товара
                      ProductEditOrAddWindow productWindow = new ProductEditOrAddWindow(new Product());

                      //Если диалоговое окно завершено успешно то добавить новый товар в БД и назначить на него цену
                      if (productWindow.ShowDialog() == true)
                      {
                          Product product = productWindow.Product;
                          product.ProductCategory = db.Categories.Find(productWindow.ProductCategory.Id);
                          db.Products.Add(product);
                          db.ProductPrices.Add(new ProductPrice()
                          {
                             dateTime = DateTime.Now,
                             Product = product,
                             Price = productWindow.InitialPrice,
                          });
                          db.SaveChanges();
                      }


                  }));
            }
        }
        // команда редактирования
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ??
                  (editCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null)
                      {
                          MessageBox.Show("Вы не выбрали запись для редактирования!");
                          return;
                      }
                      // получаем выделенный объект
                      Product product = selectedItem as Product;

                      //Создание диалогового окна для редактирования информации о товаре
                      ProductEditOrAddWindow productWindow = new ProductEditOrAddWindow(new Product
                      {
                          
                          Id = product.Id,
                          Name = product.Name,
                          CountInStorage = product.CountInStorage,
                          ProductCategory = product.ProductCategory,
                          Unit = product.Unit,
                          ProductPrices = product.ProductPrices,

                      });


                      //Если диалоговое окно завершено успешно, то обновляем информацию в БД
                      if (productWindow.ShowDialog() == true)
                      {
                          product = db.Products.Find((object)productWindow.Product.Id);
                          if (product != null)
                          {
                              product.Id = productWindow.Product.Id;
                              product.Name = productWindow.Product.Name;
                              product.CountInStorage = productWindow.Product.CountInStorage;
                              product.Unit = productWindow.Product.Unit;
                              product.ProductPrices = productWindow.Product.ProductPrices;
                              product.ProductCategory = db.Categories.Find(productWindow.ProductCategory.Id);

                              db.ProductPrices.Add(new ProductPrice()
                              {
                                  dateTime = DateTime.Now,
                                  Product = product,
                                  Price = productWindow.InitialPrice,
                              });

                              db.Entry(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                              db.SaveChanges();
                          }
                      }


                  }));
            }
        }
        // команда удаления
        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ??
                  (deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null)
                      {
                          MessageBox.Show("Вы не выбрали запись для удаления!");
                          return;
                      }

                      // получаем выделенный объект
                      Product product = selectedItem as Product;

                      //Вызов окна для подтверждения удаления
                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить выбранный элемент?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                      if (result == MessageBoxResult.Yes)
                      {
                          db.Products.Remove(product);
                          db.SaveChanges();
                      }


                  }));
            }
        }





        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }




    }
}
