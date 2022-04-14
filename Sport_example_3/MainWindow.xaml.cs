using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Sport_example_3.Views.DialogWindows;
using Sport_example_3.Views;
using System.Windows.Media.Animation;



namespace Sport_example_3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            AuthorizationWindow view = new AuthorizationWindow();
            if (view.ShowDialog() == true)
            {
                Paragraph myParagraph = new Paragraph();
                myParagraph.TextAlignment = TextAlignment.Center;
                myParagraph.FontSize = 16;
                string s = String.Empty;
                switch (view.UserRole)
                {
                    case "Администратор":
                        MainMenu.IsEnabled = true;
                        s = String.Format("Добро пожаловать, {0}!\nВас приветствует информационная система магазина спортивных товаров Спортландия!\n Вы обладаете правами администратора!\nЖелаем вам продуктивной работы!", view.Login);
                        



                        break;
                    case "Менеджер по продажам":
                        MainMenu.IsEnabled = true;
                        OrderMenuItem.IsEnabled = false;
                        HandbooksMenuItem.IsEnabled = false;
                        AdminPanelMenuItem.IsEnabled = false;
                        OrderReportsMenuItem.IsEnabled = false;
                        s = String.Format("Добро пожаловать, {0}!\nВас приветствует информационная система магазина спортивных товаров Спортландия!\n Вы обладаете правами кассира!\nЖелаем вам продуктивной работы!", view.Login);


                        break;
                    case "Менеджер по закупкам":
                        MainMenu.IsEnabled = true;
                        SalesMenuItem.IsEnabled = false;
                        HandbooksMenuItem.IsEnabled = false;
                        AdminPanelMenuItem.IsEnabled = false;
                        SalesReportsMenuItem.IsEnabled = false;
                        s = String.Format("Добро пожаловать, {0}!\nВас приветствует информационная система магазина спортивных товаров Спортландия!\n Вы обладаете правами специалиста по закупкам!\nЖелаем вам продуктивной работы!", view.Login);

                        break;


                }
                Run myRun = new Run(s);
                myParagraph.Inlines.Add(myRun);
                this.WelcomeRichTextBox.Document.Blocks.Add(myParagraph);

                DoubleAnimation TextAnimation1 = new DoubleAnimation();
                TextAnimation1.From = WelcomeRichTextBox.Opacity;
                TextAnimation1.To = 1;
                TextAnimation1.Duration = TimeSpan.FromSeconds(3);
                TextAnimation1.Completed += TextAnimation_Completed;
                WelcomeRichTextBox.BeginAnimation(RichTextBox.OpacityProperty, TextAnimation1);

            }
        }

        private void TextAnimation_Completed(object sender, EventArgs e)
        {
            DoubleAnimation TextAnimation2 = new DoubleAnimation();
            TextAnimation2.From = WelcomeRichTextBox.Opacity;
            TextAnimation2.To = WelcomeRichTextBox.Opacity;
            TextAnimation2.Duration = TimeSpan.FromSeconds(3);
            TextAnimation2.Completed += TextAnimation2_Completed;
            WelcomeRichTextBox.BeginAnimation(RichTextBox.OpacityProperty, TextAnimation2);
        }

        private void TextAnimation2_Completed(object sender, EventArgs e)
        {
            DoubleAnimation TextAnimation2 = new DoubleAnimation();
            TextAnimation2.From = WelcomeRichTextBox.Opacity;
            TextAnimation2.To = 0;
            TextAnimation2.Duration = TimeSpan.FromSeconds(6);
            WelcomeRichTextBox.BeginAnimation(RichTextBox.OpacityProperty, TextAnimation2);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void UserRolesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            UserRolesMenuItem.IsEnabled = false;
            UserRoleWindow view = new UserRoleWindow();
            if(view.ShowDialog() == true)
            {
                UserRolesMenuItem.IsEnabled = true;
            }
        }

        private void PositionEmployeesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PositionEmployeesMenuItem.IsEnabled = false;
            PositionEmployeeWindow view = new PositionEmployeeWindow();
            if (view.ShowDialog() == true)
            {
                PositionEmployeesMenuItem.IsEnabled = true;
            }
        }

        private void ProductCategoriesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProductCategoriesMenuItem.IsEnabled = false;
            ProductCategoryWindow view = new ProductCategoryWindow();
            if(view.ShowDialog()==true)
            {
                ProductCategoriesMenuItem.IsEnabled = true;
            }
        }

        private void EmployeesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            EmployeesMenuItem.IsEnabled = false;
            EmployeeWindow view = new EmployeeWindow();
            if(view.ShowDialog()==true)
            {
                EmployeesMenuItem.IsEnabled = true;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void UsersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            UsersMenuItem.IsEnabled = false;
            UserWindow view = new UserWindow();
            if (view.ShowDialog() == true)
            {
                UsersMenuItem.IsEnabled = true;
            }
        }

        private void SuppliersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SuppliersMenuItem.IsEnabled = false;
            SupplierWindow view = new SupplierWindow();
            if (view.ShowDialog() == true)
            {
                SuppliersMenuItem.IsEnabled = true;
            }
        }

        private void ProductsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProductsMenuItem.IsEnabled = false;
            ProductWindow view = new ProductWindow();
            if (view.ShowDialog() == true)
            {
                ProductsMenuItem.IsEnabled = true;
            }
        }

        private void SalesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SalesMenuItem.IsEnabled = false;
            SalesWindow view = new SalesWindow();
            if (view.ShowDialog() == true)
            {
                SalesMenuItem.IsEnabled = true;
            }
        }

        private void StorageMenuItem_Click(object sender, RoutedEventArgs e)
        {
            StorageMenuItem.IsEnabled = false;
            StorageWindow view = new StorageWindow();
            if (view.ShowDialog() == true)
            {
                StorageMenuItem.IsEnabled = true;
            }
        }

        private void PricesMenuItem_Click(object sender, RoutedEventArgs e)
        {
            PricesMenuItem.IsEnabled = false;
            PricesWindow view = new PricesWindow();
            if (view.ShowDialog() == true)
            {
                PricesMenuItem.IsEnabled = true;
            }
        }

        private void OrderMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OrderMenuItem.IsEnabled = false;
            OrderWindow view = new OrderWindow();
            if (view.ShowDialog() == true)
            {
                OrderMenuItem.IsEnabled = true;
            }
        }

        private void ProductsSuppliersMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ProductsSuppliersMenuItem.IsEnabled = false;
            SupplierAddProductWindow view = new SupplierAddProductWindow();
            if (view.ShowDialog() == true)
            {
                ProductsSuppliersMenuItem.IsEnabled = true;
            }
        }

        private void SalesReportsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SalesReportsMenuItem.IsEnabled = false;
            SalesReportWindow view = new SalesReportWindow();
            if (view.ShowDialog() == true)
            {
                SalesReportsMenuItem.IsEnabled = true;
            }
        }

        private void OrderReportsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OrderReportsMenuItem.IsEnabled = false;
            OrdersReportWindow view = new OrdersReportWindow();
            if (view.ShowDialog() == true)
            {
                OrderReportsMenuItem.IsEnabled = true;
            }
        }
    }
}
