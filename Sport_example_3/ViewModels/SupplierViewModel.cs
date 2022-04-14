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
    //ViewModel для окна "Справочник поставщиков"
    internal class SupplierViewModel:INotifyPropertyChanged
    {
        ApplicationContext db;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        IEnumerable<Supplier> supplierList;
        private Supplier selectedSupplier;

        //Выбранный поставщик
        public Supplier SelectedSupplier
        {
            get { return selectedSupplier; }
            set
            {
                selectedSupplier = value;
                OnPropertyChanged("SelectedSupplier");
            }
        }

        //Список поставщиков
        public IEnumerable<Supplier> SupplierList
        {
            get { return supplierList; }
            set
            {
                supplierList = value;
                OnPropertyChanged("SupplierList");
            }
        }

        //Конструктор класса
        public SupplierViewModel()
        {
            db = new ApplicationContext();
            db.Suppliers.ToList();
            supplierList = db.Suppliers.Local.ToBindingList();
        }

        //Команда для добавления
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                  (addCommand = new RelayCommand((o) =>
                  {
                      //Создание диалогового окна для добавления нового поставщика
                      SupplierEditOrAddWindow supplierWindow = new SupplierEditOrAddWindow(new Supplier());

                      //Если диалоговое окно завершено успешно, то добавить поставщика в БД
                      if (supplierWindow.ShowDialog() == true)
                      {
                          Supplier supplier = supplierWindow.Supplier;
                          db.Suppliers.Add(supplier);
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
                      Supplier supplier = selectedItem as Supplier;

                      //Создание диалогового окна для редактирования информации о выбранном поставщике
                      SupplierEditOrAddWindow supplierWindow = new SupplierEditOrAddWindow(new Supplier
                      {
                          Id = supplier.Id,
                          Name = supplier.Name,
                          PersonalAccount = supplier.PersonalAccount,
                          Address = supplier.Address,
                          PhoneNumber = supplier.PhoneNumber,
                          Products = supplier.Products,
                      });

                      //Если диалоговое окно завершено успешно, то изменения вносятся в БД
                      if (supplierWindow.ShowDialog() == true)
                      {
                          supplier = db.Suppliers.Find((object)supplierWindow.Supplier.Id);
                          if (supplier != null)
                          {
                              supplier.Id = supplierWindow.Supplier.Id;
                              supplier.Name = supplierWindow.Supplier.Name;
                              supplier.PersonalAccount = supplierWindow.Supplier.PersonalAccount;
                              supplier.Address = supplierWindow.Supplier.Address;
                              supplier.PhoneNumber = supplierWindow.Supplier.PhoneNumber;
                              supplier.Products = supplierWindow.Supplier.Products;

                              db.Entry(supplier).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
                      Supplier supplier = selectedItem as Supplier;

                      //Вызов окна для подтверждения удаления
                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить выбранный элемент?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                      if (result == MessageBoxResult.Yes)
                      {
                          db.Suppliers.Remove(supplier);
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
