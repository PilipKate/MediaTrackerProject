using MediaTracker.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaTracker
{
    
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            MainContent.Content = new Home();
            DataContext = new MainViewModel();
            using var context = new AppDbContext();
            context.Database.EnsureCreated();

        }

        private void Catalog_Button_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Catalog();
        }

        private void Main_Button_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Home();
        }

        private void Recommend_Button_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Recommendations();
        }

        private void Raiting_Button_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Raiting();
        }

        private void Mouseover(object sender, DependencyPropertyChangedEventArgs e)
        {

        }

        private void Add_Menu_Button_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Add();
        }

        private void Delete_Menu_Button_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new Delete();
        }
    }
}