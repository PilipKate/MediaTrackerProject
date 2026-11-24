using Microsoft.EntityFrameworkCore;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
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
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace MediaTracker.Views
{
    public class Ganre 
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Status
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class Type
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
    public class MainViewModel : INotifyPropertyChanged
    {
        private Type _selectedFilterType;
        public ObservableCollection<Type> TypeFilter { get; set; }
        private Ganre _selectedFilterGanre;
        private Ganre _selectedGanreFilm;
        private Ganre _selectedGanreSerial;
        public ObservableCollection<Ganre> Ganres { get; set; }
        private Status _selectedStatusFilm;
        private Status _selectedStatusSerial;
        private Status _newSelectedStatus;
        public ObservableCollection<Status> Statuses { get; set; }
        public Type SelectedTypeFilter
        {
            get => _selectedFilterType;
            set
            {
                _selectedFilterType = value;
                OnPropertyChanged();
            }
        }
        public Ganre SelectedGanreFilter
        {
            get => _selectedFilterGanre;
            set
            {
                _selectedFilterGanre = value;
                OnPropertyChanged();
            }
        }
        public Ganre SelectedGanreFilm
        {
            get => _selectedGanreFilm;
            set
            {
                _selectedGanreFilm = value;
                OnPropertyChanged();
            }
        }
        public Ganre SelectedGanreSerial
        {
            get => _selectedGanreSerial;
            set
            {
                _selectedGanreSerial = value;
                OnPropertyChanged();
            }
        }
        public Status SelectedStatusFilm
        {
            get => _selectedStatusFilm;
            set
            {
                _selectedStatusFilm = value;
                OnPropertyChanged();
            }
        }
        public Status SelectedStatusSerial
        {
            get => _selectedStatusSerial;
            set
            {
                _selectedStatusSerial = value;
                OnPropertyChanged();
            }
        }
        public Status NewSelectedStatus
        {
            get => _newSelectedStatus;
            set
            {
                _newSelectedStatus = value;
                OnPropertyChanged();
            }
        }
        public MainViewModel()
        {
            Ganres = new ObservableCollection<Ganre>
            {
                new Ganre { ID = 1, Name = "Фентези" },
                new Ganre { ID = 3, Name = "Фантастика" },
                new Ganre { ID = 2, Name = "Комедия" },
                new Ganre { ID = 3, Name = "Ужасы" },
                new Ganre { ID = 3, Name = "Романтика" },
                new Ganre { ID = 3, Name = "Сверхъестественное" }
            };
            TypeFilter = new ObservableCollection<Type>
            {
                new Type { ID = 1, Name = "Фильм" },
                new Type { ID = 2, Name = "Сериал" },
            };

            Statuses = new ObservableCollection<Status>
            {
                new Status { ID = 1, Name = "Просмотрено" },
                new Status { ID = 2, Name = "Не просмотрено" },
                new Status { ID = 3, Name = "В процессе" }
            };
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
        }
    }

    public partial class Add : UserControl
    {
        public Add()
        {
            InitializeComponent();

        }

        private void Add_Button_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleFilm.Text;
            string ganre = "";
            string status = "";
            string poster = PosterFilm.Text;
            string duration = DurationFilm.Text;

            if (DataContext is MainViewModel viewModel)
            {
                ganre = viewModel.SelectedGanreFilm?.Name ??"";
                status = viewModel.SelectedStatusFilm?.Name ??"";
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Введите название фильма");
                return;
            }

            if (string.IsNullOrWhiteSpace(ganre))
            {
                MessageBox.Show("Выберите жанр фильма");
                return;
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                MessageBox.Show("Выберите статус фильма");
                return;
            }

            if (string.IsNullOrWhiteSpace(duration))
            {
                MessageBox.Show("Укажите длительность фильма");
                return;
            }
            try
            {
                var filmFactory = new FilmFactory(title, ganre, status, poster, duration);
                using var context = new AppDbContext();
                context.Database.EnsureCreated();
                Film film = filmFactory.CreateWork();
                context.films.Add(film);
                context.SaveChanges();
                MessageBox.Show("Фильм успешно добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось добавить фильм: {ex.Message}");
            }
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Add_Serial_Button_Click(object sender, RoutedEventArgs e)
        {
            string title = TitleSerial.Text;
            string ganre = "";
            string status = "";
            string poster = PosterSerial.Text;
            int.TryParse(SeasonNumber.Text, out int seasonNumber);
            int.TryParse(SeriesNumber.Text, out int seriesNumber);
            if (DataContext is MainViewModel viewModel)
            {
                ganre = viewModel.SelectedGanreSerial?.Name ??"";
                status = viewModel.SelectedStatusSerial?.Name ??"";
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                MessageBox.Show("Введите название сериала");
                return;
            }

            if (string.IsNullOrWhiteSpace(ganre))
            {
                MessageBox.Show("Выберите жанр сериала");
                return;
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                MessageBox.Show("Выберите статус сериала");
                return;
            }

            if (seasonNumber == 0)
            {
                MessageBox.Show("Укажите количество сезонов");
                return;
            }

            if (seriesNumber == 0)
            {
                MessageBox.Show("Укажите количество серий");
                return;
            }
            try
            {
                var serialFactory = new SerialFactory(title, ganre, status, poster, seasonNumber, seriesNumber);
                using var context = new AppDbContext();
                context.Database.EnsureCreated();
                context.serials.Add(serialFactory.CreateWork());
                context.SaveChanges();
                MessageBox.Show("Сериал успешно добавлен!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Не удалось добавить сериал: {ex.Message}");
            }
        }
    }
}
