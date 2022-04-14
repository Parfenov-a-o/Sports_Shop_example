using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Sport_example_3.Models;


namespace Sport_example_3.ViewModels
{
    //ViewModel для окна "Добавление/изменение информации о товаре"
    internal class ProductEditOrAddViewModel:INotifyPropertyChanged
    {
        ApplicationContext db;
        private Product product;
        IEnumerable<ProductCategory> productCategoryList;
        IEnumerable<string> unitTypeList = new List<string>() { "Шт.", "кг", "г" };
        private double? _initialPrice;

        private ProductCategory selectedProductCategory;

        //Выбранный товар
        public Product Product
        {
            get { return product; }
            set { product = value; OnPropertyChanged("Product"); }
        }

        //Список категорий товаров
        public IEnumerable<ProductCategory> ProductCategoryList
        {
            get { return productCategoryList; }
            set { productCategoryList = value; OnPropertyChanged("ProductCategoryList"); }
        }

        //Выбранная категория
        public ProductCategory SelectedProductCategory
        {
            get { return selectedProductCategory; }
            set { selectedProductCategory = value; OnPropertyChanged("SelectedProductCategory"); }
        }

        //Начальная цена товара
        public double? InitialPrice
        {
            get { return _initialPrice; }
            set { _initialPrice = value; OnPropertyChanged("InitialPrice"); }
        }

        //Список возможных единиц товара
        public IEnumerable<string> UnitTypeList
        {
            get { return unitTypeList; }
            set { unitTypeList = value; OnPropertyChanged("UnitTypeList"); }
        }

        
        public ProductEditOrAddViewModel(Product p)
        {
            db = new ApplicationContext();

            productCategoryList = db.Categories.ToList();
            product = p;

            //Получить актуальную цену на товар (если мы его изменяем), в противном случае возвращает 0
            _initialPrice = db.ProductPrices.Where(pp => pp.ProductId == p.Id).OrderByDescending(pp => pp.dateTime).FirstOrDefault()?.Price??0;

            if (p.ProductCategory != null)
            {
                ProductCategory category = db.Categories.Find(p.ProductCategory.Id);
                if (category != null)
                {
                    selectedProductCategory = category;
                }
                else
                {
                    selectedProductCategory = productCategoryList.FirstOrDefault();
                }
            }
            else
            {
                selectedProductCategory = productCategoryList.FirstOrDefault();
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
