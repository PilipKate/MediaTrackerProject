using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
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

namespace MediaTracker.Views
{
    public class ReccomendViewModel
    {
        public ObservableCollection<WorkCardViewModel> Recs { get; set; }
        private readonly CardPrintFactory printFactory;

        public ReccomendViewModel()
        {
            Recs = new ObservableCollection<WorkCardViewModel>();
            printFactory = new CardPrintFactory();
        }

        public void GetWorks(IRecStrategy strategy)
        {
            var works = strategy.getFilteredWorks(WorkCollection.Instance.GetWorks());

            Recs.Clear();

            Iterator<Work> iterator = new ItemsIterator<Work>(works);

            while (iterator.hasNext())
            {
                var work = iterator.next();
                WorkCard printer = printFactory.CreatePrinter(work);

                Recs.Add(new WorkCardViewModel
                {
                    Work = work,
                    Template = printer.GetTemplate()
                });
            }
        }
    }
    public interface IRecStrategy
    {
        public List<Work> getFilteredWorks(List<Work> works);
    }
    public class FilmsRecStrategy : IRecStrategy

    {
        public List<Work> getFilteredWorks(List<Work> works)
        {
            List<Work> result = new List<Work>();
            Iterator<Work> iterator = new ItemsIterator<Work>(works);
            while (iterator.hasNext())
            {
                var work = iterator.next();
                if (work is Film)
                    result.Add(work);
            }
            return result;
        }
    }
    public class SerialsRecStrategy : IRecStrategy

    {
        public List<Work> getFilteredWorks(List<Work> works)
        {
            List<Work> result = new List<Work>();
            Iterator<Work> iterator = new ItemsIterator<Work>(works);
            while (iterator.hasNext())
            {
                var work = iterator.next();
                if (work is Serial)
                    result.Add(work);
            }
            return result;
        }
    }
    public class BestRecStrategy : IRecStrategy

    {
        public List<Work> getFilteredWorks(List<Work> works)
        {
            List<Work> result = new List<Work>();
            Iterator<Work> iterator = new ItemsIterator<Work>(works);
            while (iterator.hasNext())
            {
                var work = iterator.next();
                if (work.Rate > 8)
                    result.Add(work);
            }
            return result;
        }
    }
    public class YouRecStrategy : IRecStrategy

    {
        string type;
        string ganre;
        public YouRecStrategy(string type, string ganre)
        {
            this.type = type;
            this.ganre = ganre;
        }
        public List<Work> getFilteredWorks(List<Work> works)
        {
            List<Work> result = new List<Work>();
            Iterator<Work> iterator = new ItemsIterator<Work>(works);
            while (iterator.hasNext())
            {
                var work = iterator.next();
                if (work.Type == type & work.Ganre == ganre)
                    result.Add(work);
            }
            return result;
        }
    }
    public partial class Recommendations : UserControl
    {
        public Recommendations()
        {
            InitializeComponent();
            
        }

        private void Films_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReccomendViewModel viewModel = new ReccomendViewModel();
                DataContext = viewModel;
                IRecStrategy strategy = new FilmsRecStrategy();
                viewModel.GetWorks(strategy);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось применить фильтр: {ex.Message}");
            }
        }

        private void Serials_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReccomendViewModel viewModel = new ReccomendViewModel();
                DataContext = viewModel;
                IRecStrategy strategy = new SerialsRecStrategy();
                viewModel.GetWorks(strategy);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось применить фильтр: {ex.Message}");
            }
        }

        private void Best_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReccomendViewModel viewModel = new ReccomendViewModel();
                DataContext = viewModel;
                IRecStrategy strategy = new BestRecStrategy();
                viewModel.GetWorks(strategy);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось применить фильтр: {ex.Message}");
            }
        }

        private void You_Filter_Button_Click(object sender, RoutedEventArgs e)
        {
            
            string ganre = "", type = "";
            if (DataContext is MainViewModel RecviewModel)
            {
                ganre = RecviewModel.SelectedGanreFilter?.Name ??"";
                type = RecviewModel.SelectedTypeFilter?.Name ??"";
            }

            if (string.IsNullOrWhiteSpace(type))
            {
                MessageBox.Show("Выберите тип произведения");
                return;
            }

            if (string.IsNullOrWhiteSpace(ganre))
            {
                MessageBox.Show("Выберите жанр произведения");
                return;
            }
            try
            {
                ReccomendViewModel viewModel = new ReccomendViewModel();
                DataContext = viewModel;
                IRecStrategy strategy = new YouRecStrategy(type, ganre);
                viewModel.GetWorks(strategy);
            }
            catch (Exception ex) 
            {
                MessageBox.Show($"Не удалось применить фильтр: {ex.Message}");
            }
        }

        private void Add_Filter_Button_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel viewModel = new MainViewModel();
            DataContext = viewModel;
        }
    }
}
