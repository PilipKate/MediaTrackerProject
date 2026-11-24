using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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
    public partial class Raiting : UserControl
    {
        public Raiting()
        {
            InitializeComponent();
        }

        private void Rate_Button_Click(object sender, RoutedEventArgs e)
        {
            string titleToRate = TitleRaiting.Text;
            int.TryParse(Rate.Text, out int rate);
            if (string.IsNullOrWhiteSpace(titleToRate))
            {
                MessageBox.Show("Введите название произведения");
                return;
            }
            if (rate == 0 || rate > 10 || rate < 0)
            {
                MessageBox.Show("Введите корректный рейтинг");
                return;
            }
            using var context = new AppDbContext();
            var RateUpdate = context.works.FirstOrDefault(w => w.Title == titleToRate);
            if (RateUpdate != null)
            {
                try
                {
                    RateUpdate.Rate = rate;
                    context.SaveChanges();
                    MessageBox.Show("Рейтинг успешно изменен!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось оценить произведение: {ex.Message}");
                }

            }
            else
            {
                MessageBox.Show("Произведение не существует");
            }
        }

        private void Change_Status_Button_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleStatus.Text;
            string status = "";
            if (DataContext is MainViewModel viewModel)
            {
                status = viewModel.NewSelectedStatus?.Name ??"";
            }
            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Введите название произведения");
                return;
            }
            if (string.IsNullOrWhiteSpace(status))
            {
                MessageBox.Show("Укажите статус");
                return;
            }
            using var context = new AppDbContext();
            var StatusUpdate = context.works.FirstOrDefault(w => w.Title == title);
            if (StatusUpdate != null)
            {
                try
                {
                    StatusUpdate.Status = status;
                    context.SaveChanges();
                    MessageBox.Show("Статус успешно изменен!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Не удалось изменить статус: {ex.Message}");
                }

            }
            else
            {
                MessageBox.Show("Произведение не существует");
            }
        }
    }
}
