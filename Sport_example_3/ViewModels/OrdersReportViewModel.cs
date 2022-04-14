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
    internal class OrdersReportViewModel:INotifyPropertyChanged
    {
        ApplicationContext db;

        List<Order> _orderList;
        Order _selectedOrder;
        List<ProductInOrderBasket> _productInOrderList;
        double _totalSum;

        DateTime _beginDate;
        DateTime _endDate;

        RelayCommand _selectOrderCommand;
        RelayCommand _filterOnDatePeriodCommand;

        //Список заказов у поставщиков
        public List<Order> OrderList
        {
            get { return _orderList; }
            set { _orderList = value; OnPropertyChanged("OrderList"); }
        }

        //Выбранный заказ
        public Order SelectedOrder
        {
            get { return _selectedOrder; }
            set { _selectedOrder = value; OnPropertyChanged("SelectedOrder"); }
        }

        //Список товаров в заказе
        public List<ProductInOrderBasket> ProductInOrderList
        {
            get { return _productInOrderList; }
            set { _productInOrderList = value; OnPropertyChanged("ProductInOrderList"); }
        }

        //Итоговая сумма (стоимость) заказа
        public double TotalSum
        {
            get { return _totalSum; }
            set { _totalSum = value; OnPropertyChanged("TotalSum"); }
        }

        //Начальная дата для фильтрации
        public DateTime BeginDate
        {
            get { return _beginDate; }
            set { _beginDate = value; OnPropertyChanged("BeginDate"); }
        }

        //Конечная дата для фильтрации
        public DateTime EndDate
        {
            get { return _endDate; }
            set { _endDate = value; OnPropertyChanged("EndDate"); }
        }


        //Конструктор класса
        public OrdersReportViewModel()
        {
            db = new ApplicationContext();

            _productInOrderList = new List<ProductInOrderBasket>();

            BeginDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);

            //Загрузка связанных данных и применение фильтрации
            OrderList = db.Orders.Where(x => (x.DateTime.Date >= BeginDate.Date) && (x.DateTime.Date <= EndDate.Date)).Include(x => x.ProductInOrderBaskets).ThenInclude(x => x.Product).ThenInclude(x=>x.Suppliers).ToList();

        }

        //Команда, которая срабатывает при выборе заказа
        public RelayCommand SelectOrderCommand
        {
            get
            {
                return _selectOrderCommand ??
                  (_selectOrderCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null)
                      {
                          return;
                      }

                      // получаем выделенный объект
                      Order order = selectedItem as Order;


                      List<ProductInOrderBasket> localProductInOrderBasketList = order.ProductInOrderBaskets.ToList();
                      
                      //Заполнение списка товаров выбранного заказа
                      ProductInOrderList = localProductInOrderBasketList;

                      //Подсчет суммы заказа
                      TotalSum = ProductInOrderList.Sum(x => x.Sum).Value;




                  }));
            }
        }

        //Фильтрация заказов по дате
        public RelayCommand FilterOnDatePeriodCommand
        {
            get
            {
                return _filterOnDatePeriodCommand ??
                  (_filterOnDatePeriodCommand = new RelayCommand((o) =>
                  {

                      if (EndDate.Date < BeginDate.Date)
                      {
                          MessageBox.Show("Конечная дата должна быть позже начальной! Измените отображаемый период!");
                          OrderList = new List<Order>();
                          TotalSum = 0;
                          return;
                      }

                      //Загрузка связанных данных и применение фильтрации
                      OrderList = db.Orders.Where(x => (x.DateTime.Date >= BeginDate.Date) && (x.DateTime.Date <= EndDate.Date)).Include(x => x.ProductInOrderBaskets).ThenInclude(x => x.Product).ThenInclude(x => x.Suppliers).ToList();


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
