using Microsoft.Extensions.Primitives;
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
    public partial class Delete : UserControl
    {
        public Delete()
        {
            InitializeComponent();
        }

        private void Delete_Button_Click(object sender, RoutedEventArgs e)
        {
            string titleToDelete = TitleDelete.Text;
            if (string.IsNullOrWhiteSpace(titleToDelete))
            {
                MessageBox.Show("Введите название произведения");
                return;
            }
            using var context = new AppDbContext();
            var WorkDelete = context.works.FirstOrDefault(w => w.Title == titleToDelete);
            if (WorkDelete != null)
            {
                try
                {
                    context.works.Remove(WorkDelete);
                    context.SaveChanges();
                    MessageBox.Show("Произведение успешно удалено!");
                }
                catch (Exception ex) 
                {
                    MessageBox.Show($"Не удалось удалить произведение: {ex.Message}");
                }

            }
            else
            {
                MessageBox.Show("Произведение не существует");
            }
        }
    }
}
