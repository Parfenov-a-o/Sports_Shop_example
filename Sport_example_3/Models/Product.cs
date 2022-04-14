using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Sport_example_3.Models
{

    //Сущность товар
    public class Product:INotifyPropertyChanged
    {
        private string? _name;
        private string? _unit;
        private int _productCategoryId;
        private ProductCategory? productCategory;
        private double _countInStorage;



        public int Id { get; set; }

        //Наименование товара
        public string? Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged("Name"); }
        }

        //Единица измерения товара
        public string? Unit
        {
            get { return _unit; }
            set { _unit = value; OnPropertyChanged("Unit"); }
        }

        //Код категории товара
        public int ProductCategoryId
        {
            get { return _productCategoryId; }
            set { _productCategoryId = value; OnPropertyChanged("ProductCategoryId"); }
        }

        //Категория товара
        public ProductCategory? ProductCategory
        {
            get { return productCategory; }
            set { productCategory = value; OnPropertyChanged("ProductCategory"); }
        }

        //Список с хронологией изменения цен на данный товар
        public List<ProductPrice> ProductPrices { get; set; } = new();

        //Количество товара на складе
        public double CountInStorage
        {
            get { return _countInStorage; }
            set { _countInStorage = value; OnPropertyChanged("CountInStorage"); }
        }

        //Список всех поставщиков данного товара
        public List<Supplier> Suppliers { get; set; } = new();

        //Реализация интерфейса INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
