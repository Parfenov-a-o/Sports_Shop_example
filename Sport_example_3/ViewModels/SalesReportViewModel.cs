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
    //ViewModel для окна "Отчет о продажах"
    internal class SalesReportViewModel : INotifyPropertyChanged
    {
        ApplicationContext db;

        List<Receipt> _receiptList;
        Receipt _selectedReceipt;
        List<ProductInBasket> _productInReceiptList;
        double _totalSum;

        DateTime _beginDate;
        DateTime _endDate;

        RelayCommand _selectReceiptCommand;
        RelayCommand _filterOnDatePeriodCommand;

        //Список чеков о продаже
        public List<Receipt> ReceiptList
        {
            get { return _receiptList; }
            set { _receiptList = value; OnPropertyChanged("ReceiptList"); }
        }

        //Выбранный чек
        public Receipt SelectedReceipt
        {
            get { return _selectedReceipt; }
            set { _selectedReceipt = value; OnPropertyChanged("SelectedReceipt"); }
        }
        //Список товаров в чеке
        public List<ProductInBasket> ProductInReceiptList
        {
            get { return _productInReceiptList; }
            set { _productInReceiptList = value; OnPropertyChanged("ProductInReceiptList"); }
        }

        //Итоговая стоимость чека
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
        public SalesReportViewModel()
        {
            db = new ApplicationContext();

            _productInReceiptList = new List<ProductInBasket>();

            BeginDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(1);

            //Загрузка связанных данных и фильтрация по дате
            ReceiptList = db.Receipts.Where(x => (x.DateTime.Date >= BeginDate.Date) && (x.DateTime.Date <= EndDate.Date)).Include(x => x.ProductInBaskets).ThenInclude(x => x.Product).ToList();

        }

        //Команда для отображения списка товаров в чеке (Выполняется при выборе чека в таблице)
        public RelayCommand SelectReceiptCommand
        {
            get
            {
                return _selectReceiptCommand ??
                  (_selectReceiptCommand = new RelayCommand((selectedItem) =>
                  {
                      if (selectedItem == null)
                      {
                          return;
                      }

                      // получаем выделенный объект
                      Receipt receipt = selectedItem as Receipt;


                      List<ProductInBasket> localProductInBasketList = receipt.ProductInBaskets.ToList();

                      ProductInReceiptList = localProductInBasketList;

                      //Перерасчет итоговой стоимости выбранного чека
                      TotalSum = ProductInReceiptList.Sum(x => x.Sum).Value;

                      


                  }));
            }
        }

        //Фильтрация чеков по дате
        public RelayCommand FilterOnDatePeriodCommand
        {
            get
            {
                return _filterOnDatePeriodCommand ??
                  (_filterOnDatePeriodCommand = new RelayCommand((o) =>
                  {

                      //В том случае, если конечная дата предшествует начальной
                      if (EndDate.Date < BeginDate.Date)
                      {
                          MessageBox.Show("Конечная дата должна быть позже начальной! Измените отображаемый период!");
                          ReceiptList = new List<Receipt>();
                          TotalSum = 0;
                          return;
                      }

                      //Загрузка связанных данных и фильтрация по дате
                      ReceiptList = db.Receipts.Where(x => (x.DateTime.Date >= BeginDate.Date) && (x.DateTime.Date <= EndDate.Date)).Include(x => x.ProductInBaskets).ThenInclude(x => x.Product).ToList();


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
