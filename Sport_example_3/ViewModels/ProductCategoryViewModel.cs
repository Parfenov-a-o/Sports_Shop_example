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
    //ViewModel для окна "Справочник категорий товаров"
    internal class ProductCategoryViewModel:INotifyPropertyChanged
    {
        ApplicationContext db;
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand deleteCommand;
        IEnumerable<ProductCategory> productCategoryList;
        private ProductCategory selectedProductCategory;

        //Выбранная категория товаров
        public ProductCategory SelectedProductCategory
        {
            get { return selectedProductCategory; }
            set
            {
                selectedProductCategory = value;
                OnPropertyChanged("SelectedProductCategory");
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
        public ProductCategoryViewModel()
        {
            db = new ApplicationContext();
            db.Categories.ToList();
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
                      //Создание диалогового окна для добавления новой категории товара
                      ProductCategoryEditOrAddWindow productCategoryWindow = new ProductCategoryEditOrAddWindow(new ProductCategory());

                      //Если диалоговое окно завершено успешно то добавить новую категорию в БД
                      if (productCategoryWindow.ShowDialog() == true)
                      {
                          ProductCategory productCategory = productCategoryWindow.ProductCategory;
                          db.Categories.Add(productCategory);
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
                      ProductCategory productCategory = selectedItem as ProductCategory;

                      //Создание диалогового окна для изменения выбранной категории товара
                      ProductCategoryEditOrAddWindow productCategoryWindow = new ProductCategoryEditOrAddWindow(new ProductCategory
                      {
                          Id = productCategory.Id,
                          Name = productCategory.Name,
                          Products = productCategory.Products,
                      });

                      //Если диалоговое окно завершено успешно то изменяем информацию в БД
                      if (productCategoryWindow.ShowDialog() == true)
                      {
                          productCategory = db.Categories.Find((object)productCategoryWindow.ProductCategory.Id);
                          if (productCategory != null)
                          {
                              productCategory.Id = productCategoryWindow.ProductCategory.Id;
                              productCategory.Name = productCategoryWindow.ProductCategory.Name;
                              productCategory.Products = productCategoryWindow.ProductCategory.Products;
                              db.Entry(productCategory).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
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
                      ProductCategory productCategory = selectedItem as ProductCategory;

                      //Вызов окна для подтверждения удаления
                      MessageBoxResult result = MessageBox.Show("Вы действительно хотите удалить выбранный элемент?", "Удаление записи", MessageBoxButton.YesNo, MessageBoxImage.Question);
                      if (result == MessageBoxResult.Yes)
                      {
                          db.Categories.Remove(productCategory);
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
