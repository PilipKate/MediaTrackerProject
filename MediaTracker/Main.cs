using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Windows;

namespace MediaTracker
{
    [Table("works")]
    public class Work
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Column("title")]
        public string Title { get; set; }
        [Column("type")]
        public string Type { get; set; }

        [Column("ganre")]
        public string Ganre { get; set; }

        [Column("rate")]
        public double? Rate { get; set; }

        [Column("status")]
        public string Status { get; set; }
        [Column("poster")]
        public string Poster { get; set; }
        public Work(string Title, string Type, string Ganre, string Status, string Poster)
        {
            this.Title = Title;
            this.Type = Type;
            this.Ganre = Ganre;
            this.Status = Status;
            this.Poster = Poster;
        }
    }

    [Table("films")]
    public class Film : Work
    {

        [Column("duration")]
        public string Duration { get; set; }

        public Film(string Title, string Type, string Ganre,  string Status, string Poster, string Duration) : base(Title, Type, Ganre, Status, Poster)
        {
            this.Duration = Duration;
        }
    }

    [Table("serials")]
    public class Serial : Work
    {
        [Column("seasonnumber")]
        public int SeasonNumber { get; set; }

        [Column("seriesnumber")]
        public int SeriesNumber { get; set; }

        public Serial(string Title, string Type, string Ganre, string Status, string Poster, int SeasonNumber, int SeriesNumber) : base(Title, Type, Ganre,  Status,Poster)
        {
            this.SeasonNumber = SeasonNumber;
            this.SeriesNumber = SeriesNumber;
        }
    }

    public abstract class WorkFactory
    {
        public abstract Work CreateWork();
    }
    public class FilmFactory : WorkFactory
    {
        private readonly string title;
        private readonly string ganre;
        private readonly double rate;
        private readonly string status;
        private readonly string poster;
        private readonly string duration;

        public FilmFactory(string title, string ganre,  string status,string poster, string duration)
        {
            this.title = title;
            this.ganre = ganre;
            this.status = status;
            this.poster = poster;
            this.duration = duration;
        }

        public override Film CreateWork()
        {
            return new Film(this.title, "Фильм", this.ganre,  this.status,this.poster, this.duration);
        }
    }
    public class SerialFactory : WorkFactory
    {
        private readonly string title;
        private readonly string genre;
        private readonly double rate;
        private readonly string status;
        private readonly string poster;
        private readonly int seasonNumber;
        private readonly int seriesNumber;

        public SerialFactory(string title, string genre,  string status, string poster, int seasonNumber, int seriesNumber)
        {
            this.title = title;
            this.genre = genre;
            this.status = status;
            this.poster = poster;
            this.seasonNumber = seasonNumber;
            this.seriesNumber = seriesNumber;
        }

        public override Serial CreateWork()
        {
            return new Serial(this.title, "Сериал", this.genre, this.status, this.poster, this.seasonNumber, this.seriesNumber);
        }
    }
    interface Iterator<T>
    {
        bool hasNext();
        T next();
    }
    class ItemsIterator<T> : Iterator<T>
    {
        private List<T> items;
        private int position;

        public ItemsIterator(List<T> items)
        {
            this.items = items;
            this.position = 0;
        }
        public bool hasNext()
        {
            return position < items.Count;
        }
        public T next()
        {
            if (this.hasNext())
                return items[position++];
            throw new IndexOutOfRangeException();
        }
    }
    public interface ICardFactory
    {
        WorkCard CreatePrinter(Work work);
    }
    public interface ICardPrint
    {
        DataTemplate GetTemplate();
    }
    public class FilmCardPrint : ICardPrint
    {
        public DataTemplate GetTemplate()
        {
            return (DataTemplate)Application.Current.FindResource("FilmCardTemplate");
        }
    }

    public class SerialCardPrint : ICardPrint
    {
        public DataTemplate GetTemplate()
        {
            return (DataTemplate)Application.Current.FindResource("SerialCardTemplate");
        }
    }
    public class CardPrintFactory : ICardFactory
    {
        public WorkCard CreatePrinter(Work work)
        {
            if (work is Film)
                return new WorkCard(work, new FilmCardPrint());
            else if (work is Serial)
                return new WorkCard(work, new SerialCardPrint());
            else
                throw new NotSupportedException();
        }
    }
    public class WorkCard
    {
        protected Work Work { get; set; }

        protected ICardPrint Printer { get; set; }

        public WorkCard(Work work, ICardPrint printer)
        {
            Work = work;
            Printer = printer;
        }

        public DataTemplate GetTemplate()
        {
            return Printer.GetTemplate();
        }

        public Work GetWork()
        {
            return Work;
        }
    }
    public class WorkCardViewModel
    {
        public Work Work { get; set; }

        public DataTemplate Template { get; set; }
    }
    public interface IWorkRepository
    {
        List<Work> GetWorks();
    }
    public class WorkCollection : IWorkRepository
    {
        private static WorkCollection collection;

        public static WorkCollection Instance
        {
            get
            {
                if (collection == null)
                    collection = new WorkCollection();

                return collection;
            }
        }

        private WorkCollection()
        {
        }

        public List<Work> GetWorks()
        {
            using (var context = new AppDbContext())
            {
                return context.Set<Work>().OrderBy(W => W.id).ToList();
            }
        }
    }
}
