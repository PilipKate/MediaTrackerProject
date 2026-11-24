using Moq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MediaTracker.Views
{
    
    public class CatalogViewModel
    {
        public ObservableCollection<WorkCardViewModel> Works { get; set; }
        private readonly IWorkRepository workRepository;
        private readonly ICardFactory printFactory;

        public CatalogViewModel(): this(WorkCollection.Instance, new CardPrintFactory())
        {
        }
        public CatalogViewModel(IWorkRepository workRepository, ICardFactory printFactory)
        {
            Works = new ObservableCollection<WorkCardViewModel>();
            this.workRepository = workRepository;
            this.printFactory = printFactory;
        }

        public void GetWorks()
        {
            var works = workRepository.GetWorks();

            Works.Clear();

            Iterator<Work> iterator = new ItemsIterator<Work>(works);

            while (iterator.hasNext())
            {
                var work = iterator.next();
                WorkCard printer = printFactory.CreatePrinter(work);
                Works.Add(new WorkCardViewModel
                {
                    Work = work,
                    Template = printer.GetTemplate()
                });
            }
        }
    }
    public partial class Catalog : UserControl
    {
        public Catalog()
        {
            InitializeComponent();

            CatalogViewModel _viewModel = new CatalogViewModel();
            DataContext = _viewModel;

            _viewModel.GetWorks();

        }
    }
}
