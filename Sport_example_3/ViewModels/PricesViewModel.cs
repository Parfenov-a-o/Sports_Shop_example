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
    //ViewModel для окна "Прайс-лист"
    internal class PricesViewModel:INotifyPropertyChanged
    {
        private ApplicationContext db;
        private List<ProductPrice> productPricesList;
        private ProductPrice selectedProductPrice;

        private RelayCommand changePriceCommand;


        //Список с актуальными ценами на товары
        public List<ProductPrice> ProductPricesList
        {
            get { return productPricesList; }
            set { productPricesList = value; OnPropertyChanged("ProductPricesList"); }
        }

        //Выбранная запись с информацией о продукте и о цене на него
        public ProductPrice SelectedProductPrice
        {
            get { return selectedProductPrice; }
            set { selectedProductPrice = value; OnPropertyChanged("SelectedProductPrice"); }
        }

        //Конструктор класса
        public PricesViewModel()
        {
            db = new ApplicationContext();
            db.Categories.ToList();
            db.Products.ToList();

            //Загрузка актуальных (последних) цен на товары 
            productPricesList = db.ProductPrices.OrderByDescending(pp => pp.dateTime).AsEnumerable().GroupBy(pp => pp.ProductId).Select(pp => pp.First()).OrderBy(pp=>pp.Product.Name).ToList();
        }

        //Команда для изменения цены на товар
        public RelayCommand ChangePriceCommand
        {
            get
            {
                return changePriceCommand ??
                  (changePriceCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null)
                      {
                          MessageBox.Show("Вы не выбрали запись для редактирования!");
                          return;
                      }
                      // получаем выделенный объект
                      ProductPrice productPrice = selectedItem as ProductPrice;

                      //Создание диалогового окна для изменения цены на товар
                      ProductPriceEditOrAddWindow productPriceWindow = new ProductPriceEditOrAddWindow(new ProductPrice
                      {
                          Id = productPrice.Id,
                          dateTime = productPrice.dateTime,
                          Product = productPrice.Product,
                          ProductId = productPrice.ProductId,
                          Price = productPrice.Price,

                      });

                      //Если диалоговое окно завершено успешно, то добавляется новая запись в таблицу Товар-Цена
                      if (productPriceWindow.ShowDialog() == true)
                      {
                          db.ProductPrices.Add(new ProductPrice()
                          {
                              dateTime = DateTime.Now,
                              Product = productPriceWindow.ProductPrice.Product,
                              ProductId = productPriceWindow.ProductPrice.ProductId,
                              Price = productPriceWindow.NewPrice,
                          });

                          db.SaveChanges();
                      }

                      //Обновление списка с актуальными ценами
                      ProductPricesList = db.ProductPrices.OrderByDescending(pp => pp.dateTime).AsEnumerable().GroupBy(pp => pp.ProductId).Select(pp => pp.First()).OrderBy(pp => pp.Product.Name).ToList();




                  }));
            }
        }


        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }
    }
}
