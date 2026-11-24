using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : UserControl
    {
        public Home()
        {
            InitializeComponent();


            List<Work> collection = WorkCollection.Instance.GetWorks();
            All.Text = collection.Count.ToString();
            int seen = 0, inProcess = 0, notSeen = 0;
            Iterator<Work> iterator = new ItemsIterator<Work>(collection);

            while (iterator.hasNext())
            {
                Work work = iterator.next();
                if (work.Status == "Просмотрено")
                {
                    seen++;
                }
                else if (work.Status == "Не просмотрено")
                {
                    notSeen++;
                }
                else if (work.Status == "В процессе")
                {
                    inProcess++;
                }
            }
            Seen.Text = seen.ToString();
            NotSeen.Text = notSeen.ToString();
            InProcess.Text = inProcess.ToString();
        }

    }
}
