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
    internal class OrderViewModel : INotifyPropertyChanged
    {
        ApplicationContext db;
        List<Supplier> _supplierList;
        List<ProductCategory> _productCategoryList;
        List<Product> _productList;
        List<Product> _productsForSelectedSupplier;
        List<ProductInOrderBasket> _productInOrderBasketList;

        Supplier _selectedSupplier;
        ProductCategory _selectedProductCategory;
        Product _selectedProduct;
        double _countValue;
        double _priceValue;
        double _totalSum;
        ProductInOrderBasket _selectedProductInOrderBasket;


        RelayCommand _filterOnSupplierCommand;
        RelayCommand _filterOnProductCategoryCommand;
        RelayCommand _addToBasketCommand;
        RelayCommand _deleteCommand;
        RelayCommand _createOrderCommand;

        //Список поставщиков
        public List<Supplier> SupplierList
        {
            get { return _supplierList; }
            set { _supplierList = value; OnPropertyChanged("SupplierList"); }
        }

        //Выбранный поставщик
        public Supplier SelectedSupplier
        { get { return _selectedSupplier; } set { _selectedSupplier = value; OnPropertyChanged("SelectedSupplier"); } }

        //Список категорий товаров
        public List<ProductCategory> ProductCategoryList
        {
            get { return _productCategoryList; }
            set { _productCategoryList = value; OnPropertyChanged("ProductCategoryList"); }
        }

        //Выбранная категория товаров
        public ProductCategory SelectedProductCategory
        {
            get { return _selectedProductCategory; }
            set { _selectedProductCategory = value; OnPropertyChanged("SelectedProductCategory"); }
        }

        //Список товаров
        public List<Product> ProductList
        { get { return _productList; } set { _productList = value; OnPropertyChanged("ProductList"); } }

        //Выбранный товар
        public Product SelectedProduct
        {
            get { return _selectedProduct; } set { _selectedProduct = value; OnPropertyChanged("SelectedProduct"); }
        }

        //Закупочная цена товара
        public double PriceValue
        {
            get { return _priceValue; } set { _priceValue = value; OnPropertyChanged("PriceValue"); }
        }

        //Количество приобретаемого товара
        public double CountValue
        {
            get { return _countValue; }
            set { _countValue = value; OnPropertyChanged("CountValue"); }
        }

        //Список товаров в корзине заказа
        public List<ProductInOrderBasket> ProductInOrderBasketList
        {
            get { return _productInOrderBasketList; }
            set { _productInOrderBasketList = value; OnPropertyChanged("ProductInOrderBasketList"); }
        }

        //Выбранный товар в корзине
        public ProductInOrderBasket SelectedProductInOrderBasket
        {
            get { return _selectedProductInOrderBasket; }
            set { _selectedProductInOrderBasket = value; OnPropertyChanged("SelectedProductInOrderBasket"); }
        }

        //Итоговая сумма закупки
        public double TotalSum
        {
            get { return _totalSum; } set { _totalSum = value; OnPropertyChanged("TotalSum"); }
        }

        //Конструктор класса
        public OrderViewModel()
        {
            db = new ApplicationContext();
            db.Categories.ToList();

            _supplierList = db.Suppliers.ToList();

            ProductCategoryList = new List<ProductCategory>();
            ProductList = new List<Product>();
            _productsForSelectedSupplier = new List<Product>();
            _productInOrderBasketList = new List<ProductInOrderBasket>();

            SupplierList.Insert(0, new Supplier() { Name = "Выберите поставщика", Id = 0 });
            _selectedSupplier = SupplierList.ToList()[0];
        }


        //Команда для фильтрации категорий товаров по поставщику
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

                    //Если был выбран пункт "Выберите поставщика"
                    if(supplier.Id == 0)
                    {
                        ProductCategoryList = new List<ProductCategory>();
                        ProductList = new List<Product>();
                        return;
                    }

                    //Загрузка связанных данных
                    db.Suppliers.Include(x => x.Products).ToList();

                    
                    //Заполнение списка доступных у поставщика категорий товаров
                    Supplier suppFromDb = db.Suppliers.Find(supplier.Id);

                    List<Product> products = suppFromDb.Products;

                    List<ProductCategory> categories = new List<ProductCategory>();


                    _productsForSelectedSupplier = products.DistinctBy(x => x.ProductCategoryId).ToList();

                    foreach(Product product in _productsForSelectedSupplier)
                    {
                        ProductCategory newCategory = db.Categories.Find(product.ProductCategory.Id);

                        categories.Add(newCategory);
                    }

                    ProductCategoryList = categories;

                    if(products.Count==0)
                    {
                        MessageBox.Show("У выбранного поставщика нет товаров!");

                    }
                    
                    SelectedProduct = null;

                }
                ));
            }
        }

        //Фильтрация товаров по выбранной категории
        public RelayCommand FilterOnProductCategoryCommand
        {
            get
            {
                return _filterOnProductCategoryCommand ?? (_filterOnProductCategoryCommand = new RelayCommand((selectedItem) =>
                {
                    if (selectedItem == null)
                    {
                        return;
                    }

                    ProductCategory productCategory = selectedItem as ProductCategory;

                    //В том случае если выбрали пункт "Выберите категорию"
                    if (productCategory.Id == 0)
                    {
                        ProductCategoryList = new List<ProductCategory>();
                        return;
                    }

                    //Заполнение списка доступных у поставщика товаров
                    ProductList = _productsForSelectedSupplier.Where(x=>x.ProductCategoryId == productCategory.Id).ToList();


                }
                ));
            }
        }

        //Команда для добавления товара в корщину заказа
        public RelayCommand AddToBasketCommand
        {
            get
            {
                return _addToBasketCommand ??
                  (_addToBasketCommand = new RelayCommand((selectedItem) =>
                  {
                      //В том случае если пользователь не выбрал товар
                      if (selectedItem == null || SelectedProduct.Id == 0)
                      {
                          MessageBox.Show("Вы не выбрали товар!");
                          return;
                      }
                      // получаем выделенный объект
                      Product product = selectedItem as Product;

                      //Проверка, является ли количество товара приобретаемого пользователем больше нуля
                      if ((CountValue > 0) && (PriceValue > 0))
                      {
                          //Загрузка списка корзины в локальный список
                          List<ProductInOrderBasket> localProductInBasketList = ProductInOrderBasketList.ToList();


                          //Если выбранного товара нет в корзине, то в список добавляется новый элемент
                          localProductInBasketList.Add(new ProductInOrderBasket()
                          {

                              IndexNumber = localProductInBasketList.Count + 1,
                              Product = product,
                              ProductId = product.Id,
                              Price = PriceValue,
                              Count = CountValue,
                              Sum = PriceValue * CountValue,
                              Supplier = SelectedSupplier,
                              SupplierId = SelectedSupplier.Id,
                          });

                          ProductInOrderBasketList = localProductInBasketList;

                          //Подсчёт итоговой суммы заказа
                          TotalSum = ProductInOrderBasketList.Sum(p => p.Sum).Value;
                      }
                      else
                      {
                          MessageBox.Show("Вы ввели некорректные данные цены или количества!");
                      }

                          
                  }));
            }
        }

        //Команда для удаления товара из корзины
        public RelayCommand DeleteCommand
        {
            get
            {
                return _deleteCommand ??
                  (_deleteCommand = new RelayCommand((selectedItem) =>
                  {
                      //В том случае если запись для удаления не выбрана
                      if (selectedItem == null)
                      {
                          MessageBox.Show("Вы не выбрали запись для удаления!");
                          return;
                      }

                      // получаем выделенный объект
                      ProductInOrderBasket productInBasket = selectedItem as ProductInOrderBasket;

                      List<ProductInOrderBasket> localProductInBasketList = ProductInOrderBasketList.ToList();

                      //Вызов окна-подтверждения для удаления товара
                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить выбранный элемент?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                      if (result == MessageBoxResult.Yes)
                      {
                          localProductInBasketList.Remove(productInBasket);
                          
                          //Обновление индексации товаров в заказе после удаления товара из корзины
                          for (int i = 0; i < localProductInBasketList.Count; i++)
                          {
                              localProductInBasketList[i].IndexNumber = i + 1;
                          }
                          ProductInOrderBasketList = localProductInBasketList;


                          //Перерасчет суммы заказа
                          TotalSum = ProductInOrderBasketList.Sum(p => p.Sum).Value;

                          SelectedProduct = null;
                      }


                  }));
            }
        }

        //Команда для оформления заказа
        public RelayCommand CreateOrderCommand
        {
            get
            {
                return _createOrderCommand ??
                  (_createOrderCommand = new RelayCommand((o) =>
                  {

                      //В том случае, если в корзине есть товары
                      if (ProductInOrderBasketList.Count > 0)
                      {
                          //Создание нового объекта и инициализация текущего даты и времени
                          Order or1 = new Order() { DateTime = DateTime.Now };

                          //Перебор всех товаров в корзине, в целях пополнения количества товаров на складе
                          foreach (ProductInOrderBasket product in ProductInOrderBasketList)
                          {
                              Product product1 = db.Products.Find(product.ProductId);
                              product1.CountInStorage += product.Count;
                              
                          }
                          
                          //Добавление товаров из корзины в чек
                          or1.ProductInOrderBaskets = ProductInOrderBasketList;
                          db.Orders.Add(or1);
                          //Сохранение результатов в БД
                          db.SaveChanges();

                          MessageBox.Show("Заказ успешно оформлен!");

                          ProductInOrderBasketList = new List<ProductInOrderBasket>();
                      }
                      else
                      {
                          MessageBox.Show("Ваша корзина пуста!");
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
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
