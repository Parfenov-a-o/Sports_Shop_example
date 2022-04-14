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
    //ViewModel для окна "Продажа товаров"
    internal class SalesViewModel:INotifyPropertyChanged
    {
        ApplicationContext db;

        private List<ProductCategory> productCategoryList;
        private List<Product> productList;
        private List<ProductInBasket> productInBasketList;
        


        private Product selectedProduct;
        private ProductCategory selectedProductCategory;
        private ProductInBasket selectedProductInBasket;
        private double? actualPrice;

        RelayCommand filterProductCommand;
        RelayCommand selectProductCommand;
        RelayCommand addToBasketCommand;
        RelayCommand deleteCommand;
        RelayCommand createReceiptCommand;

        private double countInStorage;
        private double actualCount;
        private double totalSum;


        //Список категорий товаров
        public List<ProductCategory> ProductCategoryList
        {
            get { return productCategoryList; }
            set { productCategoryList = value; OnPropertyChanged("ProductCategoryList");}
        }

        //Выбранная категория
        public ProductCategory SelectedProductCategory
        {
            get { return selectedProductCategory; }
            set { selectedProductCategory = value; OnPropertyChanged("SelectedProductCategory"); }
        }

        //Список товаров
        public List<Product> ProductList
        {
            get { return productList; }
            set { productList = value; OnPropertyChanged("ProductList"); }
        }

        //Выбранный товар
        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set { selectedProduct = value; OnPropertyChanged("SelectedProduct"); }
        }
        //Количество товара на складе
        public double CountInStorage
        { 
            get { return countInStorage; } set { countInStorage = value; OnPropertyChanged("CountInStorage"); } 
        }
        //Список товаров в корзине
        public List<ProductInBasket > ProductInBasketList
        {
            get { return productInBasketList; }
            set { productInBasketList = value; OnPropertyChanged("ProductInBasketList"); }
        }

        //Количество единиц приобретаемого товара
        public double ActualCount
        {
            get { return actualCount; }
            set { actualCount = value; OnPropertyChanged("ActualCount"); }
        }

        //Актуальная цена товара
        public double? ActualPrice
        {
            get { return actualPrice; }
            set { actualPrice = value; OnPropertyChanged("ActualPrice"); }
        }

        //Выбранный товар в корзине
        public ProductInBasket SelectedProductInBasket
        {
            get { return selectedProductInBasket; }
            set { selectedProductInBasket = value; OnPropertyChanged("SelectedProductInBasket"); }
        }

        //Общая сумма покупки
        public double TotalSum
        {
            get { return totalSum; }
            set { totalSum = value; OnPropertyChanged("TotalSum"); }
        }

        //Конструктор класса
        public SalesViewModel()
        {
            db = new ApplicationContext();
            
            productList = new List<Product>();
            productInBasketList = new List<ProductInBasket>();
            productCategoryList = db.Categories.ToList();

            productCategoryList.Insert(0, new ProductCategory() { Name = "Выберите категорию", Id = 0 });
            productList.Insert(0, new Product() { Name = "Выберите категорию", Id = 0 });
            selectedProductCategory = productCategoryList[0];
            selectedProduct = productList[0];

        }

        //Команда для фильтрации продуктов по категории
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

                      ProductList.Clear();
                      //Загрузка списка товаров выбранной категории
                      List<Product> productsFromDb = db.Products.Include(pc => pc.ProductCategory).Where(x => x.ProductCategory!.Id == category.Id).OrderBy(p => p.Name).ToList();
                      productsFromDb.Insert(0, new Product() { Name = "Выберите товар", Id = 0 });

                      ProductList = productsFromDb;
                      if(ProductList.Count>0)
                      {
                          SelectedProduct = ProductList[0];
                      }
                      


                  }));
            }
        }

        //Команда для загрузки информации о выбранном товаре (Актуальная цена и количество на складе)
        public RelayCommand SelectProductCommand
        {
            get
            {
                return selectProductCommand ??
                  (selectProductCommand = new RelayCommand((selectedItem) =>
                  {

                      if (selectedItem == null || SelectedProductCategory.Id == 0)
                      {
                          CountInStorage = 0;
                          return;
                      }
                      // получаем выделенный объект
                      Product product = selectedItem as Product;

                      CountInStorage = product.CountInStorage;
                      //Загрузка актуальной цены на выбранный товар
                      ActualPrice = db.ProductPrices.Where(pp => pp.ProductId == product.Id).OrderByDescending(pp => pp.dateTime).FirstOrDefault()?.Price;

                  }));
            }
        }


        //Команда для добавления товара в корзину
        public RelayCommand AddToBasketCommand
        {
            get
            {
                return addToBasketCommand ??
                  (addToBasketCommand = new RelayCommand((selectedItem) =>
                  {

                      if (selectedItem == null || SelectedProduct.Id == 0)
                      {
                          MessageBox.Show("Вы не выбрали товар!");
                          return;
                      }
                      // получаем выделенный объект
                      Product product = selectedItem as Product;

                      //Проверка, является ли количество товара приобретаемого пользователем больше нуля
                      if(ActualCount>0)
                      {
                          //Проверка, хватает ли на складе товара
                          if (ActualCount <= CountInStorage)
                          {
                              //Загрузка списка корзины в локальный список
                              List<ProductInBasket> localProductInBasketList = ProductInBasketList.ToList();

                              //Проверка, пустая ли корзина с товарами
                              if (localProductInBasketList.Count > 0)
                              {
                                  //Проверка, имеется ли уже выбранный товар в корзине
                                  if (localProductInBasketList.Where(p => p.ProductId == product.Id).Count() == 0)
                                  {
                                      //Если выбранного товара нет в корзине, то в список добавляется новый элемент
                                      localProductInBasketList.Add(new ProductInBasket()
                                      {

                                          IndexNumber = localProductInBasketList.Count + 1,
                                          Product = product,
                                          ProductId = product.Id,
                                          Price = ActualPrice,
                                          Count = ActualCount,
                                          Sum = ActualCount * ActualPrice,
                                      });
                                  }
                                  else
                                  {
                                      //Если выбранный товар уже есть в корзине, то изменяется его количество и сумма
                                      ProductInBasket editProductInBasket = localProductInBasketList.Where(p => p.ProductId == product.Id).FirstOrDefault();
                                      editProductInBasket.Count += ActualCount;
                                      editProductInBasket.Sum = editProductInBasket.Count * editProductInBasket.Price;
                                  }
                              }
                              else
                              {
                                  //Если корзина пустая, то в список добавляется элемент без проверок
                                  localProductInBasketList.Add(new ProductInBasket()
                                  {
                                      IndexNumber = localProductInBasketList.Count + 1,
                                      Product = product,
                                      ProductId = product.Id,
                                      Price = ActualPrice,
                                      Count = ActualCount,
                                      Sum = ActualCount * ActualPrice,
                                  });
                              }


                              ProductInBasketList = localProductInBasketList;

                              TotalSum = ProductInBasketList.Sum(p => p.Sum).Value;

                              Product editProduct = ProductList.Where(p => p.Id == product.Id).FirstOrDefault();
                              editProduct.CountInStorage -= ActualCount;
                              CountInStorage -= ActualCount;
                          }
                          else
                          {
                              MessageBox.Show("Единиц товара не хватает на складе. \nУменьшите требуемое количество единиц товара!");
                          }
                      }
                      else
                      {
                          MessageBox.Show("Вы не указали количество приобретаемого товара!");
                      }
                      
                  }));
            }
        }

        //Команда для удаления товара из корзины 
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
                      ProductInBasket productInBasket = selectedItem as ProductInBasket;

                      List<ProductInBasket> localProductInBasketList = ProductInBasketList.ToList();

                      //Вызов диалогового окна для подтверждения удаления
                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить выбранный элемент?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                      if (result == MessageBoxResult.Yes)
                      {
                          localProductInBasketList.Remove(productInBasket);
                          //Переиндексация товаров в корзине после удаления товара из корзины
                          for (int i = 0; i < localProductInBasketList.Count; i++)
                          {
                              localProductInBasketList[i].IndexNumber = i + 1;
                          }
                          ProductInBasketList = localProductInBasketList;

                          Product editProduct = ProductList.Where(p => p.Id == productInBasket.ProductId).FirstOrDefault();
                          editProduct.CountInStorage += productInBasket.Count;
                          
                          //Перерасчет итоговой суммы 
                          TotalSum = ProductInBasketList.Sum(p => p.Sum).Value;

                          SelectedProduct = ProductList[0];
                          CountInStorage = 0;
                      }


                  }));
            }
        }


        //Команда для создания чека
        public RelayCommand CreateReceiptCommand
        {
            get
            {
                return createReceiptCommand ??
                  (createReceiptCommand = new RelayCommand((o) =>
                  {
                      //Проверка на наличие товаров в корзине
                      if (ProductInBasketList.Count > 0)
                      {
                          //Создание нового объекта и запись его в БД
                          Receipt rc1 = new Receipt() { DateTime = DateTime.Now };
                          rc1.ProductInBaskets = ProductInBasketList;
                          db.Receipts.Add(rc1);
                          db.SaveChanges();

                          MessageBox.Show("Заказ успешно оформлен!");
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
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
