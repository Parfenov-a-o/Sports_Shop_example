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
    //ViewModel для окна "Склад"
    internal class StorageViewModel:INotifyPropertyChanged
    {
        private ApplicationContext db;
        private IEnumerable<Product> productInStorageList;

        //Список товаров на складе
        public IEnumerable<Product> ProductInStorageList
        {
            get { return productInStorageList; }
            set { productInStorageList = value; OnPropertyChanged("ProductInStorageList"); }
        }

        //Конструктор класса
        public StorageViewModel()
        {
            db = new ApplicationContext();
            db.Products.ToList();
            db.Categories.ToList();
            productInStorageList = db.Products.Local.ToBindingList();
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
