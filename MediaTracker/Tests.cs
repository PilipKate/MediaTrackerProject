using MediaTracker.Views;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace MediaTracker
{

    [TestFixture]
    public class WorkFactoryTests
    {
        [Test]
        public void FilmFactory_CreateWork_ReturnsFilm()
        {
            var factory = new FilmFactory("Интерстеллар", "Фантастика", "Просмотрено", "poster.jpg", "1 час 20 мин");

            Film film = factory.CreateWork();

            Assert.That(film, Is.Not.Null);
            Assert.That(film, Is.TypeOf<Film>());
            Assert.That(film.Title, Is.EqualTo("Интерстеллар"));
            Assert.That(film.Type, Is.EqualTo("Фильм"));
            Assert.That(film.Ganre, Is.EqualTo("Фантастика"));
            Assert.That(film.Status, Is.EqualTo("Просмотрено"));
            Assert.That(film.Poster, Is.EqualTo("poster.jpg"));
            Assert.That(film.Duration, Is.EqualTo("1 час 20 мин"));
        }

        [Test]
        public void SerialFactory_CreateWork_ReturnsSerial()
        {
            var factory = new SerialFactory("Шерлок", "Детектив", "В процессе", "sherlock.jpg", 4, 13);

            Serial serial = factory.CreateWork();

            Assert.That(serial, Is.Not.Null);
            Assert.That(serial, Is.TypeOf<Serial>());
            Assert.That(serial.Title, Is.EqualTo("Шерлок"));
            Assert.That(serial.Type, Is.EqualTo("Сериал"));
            Assert.That(serial.Ganre, Is.EqualTo("Детектив"));
            Assert.That(serial.Status, Is.EqualTo("В процессе"));
            Assert.That(serial.Poster, Is.EqualTo("sherlock.jpg"));
            Assert.That(serial.SeasonNumber, Is.EqualTo(4));
            Assert.That(serial.SeriesNumber, Is.EqualTo(13));
        }
    }

    [TestFixture]
    public class ItemsIteratorTests
    {
        [Test]
        public void HasNext_WhenItemsExist_ReturnsTrue()
        {
            var items = new List<int> { 1, 2, 3 };
            var iterator = new ItemsIterator<int>(items);

            bool result = iterator.hasNext();

            Assert.That(result, Is.True);
        }

        [Test]
        public void Next_ReturnsItemsInCorrectOrder()
        {
            var items = new List<int> { 10, 20, 30 };
            var iterator = new ItemsIterator<int>(items);

            int first = iterator.next();
            int second = iterator.next();
            int third = iterator.next();

            Assert.That(first, Is.EqualTo(10));
            Assert.That(second, Is.EqualTo(20));
            Assert.That(third, Is.EqualTo(30));
        }

        [Test]
        public void HasNext_WhenAllItemsRead_ReturnsFalse()
        {
            var items = new List<int> { 1, 2 };
            var iterator = new ItemsIterator<int>(items);

            iterator.next();
            iterator.next();
            bool result = iterator.hasNext();

            Assert.That(result, Is.False);
        }

        [Test]
        public void Next_WhenNoItemsLeft_ThrowsException()
        {
            var items = new List<int> { 1 };
            var iterator = new ItemsIterator<int>(items);

            iterator.next();

            Assert.Throws<IndexOutOfRangeException>(
                () => iterator.next());
        }

        [Test]
        public void HasNext_WhenListIsEmpty_ReturnsFalse()
        {
            var items = new List<int>();
            var iterator = new ItemsIterator<int>(items);

            bool result = iterator.hasNext();

            Assert.That(result, Is.False);
        }
    }

    [TestFixture]
    public class WorkCardTests
    {
        [Test]
        public void GetWork_ReturnsCorrectWork()
        {
            var film = new Film("Интерстеллар", "Фильм", "Фантастика", "Просмотрено", "poster.jpg", "1 час 20 мин");

            var mockPrinter = new Mock<ICardPrint>();

            var card = new WorkCard(film, mockPrinter.Object);

            Work result = card.GetWork();

            Assert.That(result, Is.SameAs(film));
        }

        [Test]
        public void GetTemplate_CallsPrinter()
        {
            var film = new Film("Интерстеллар", "Фильм", "Фантастика", "Просмотрено", "poster.jpg", "1 час 20 мин");

            var mockPrinter = new Mock<ICardPrint>();

            var card = new WorkCard(film, mockPrinter.Object);

            card.GetTemplate();

            mockPrinter.Verify(
                printer => printer.GetTemplate(),
                Times.Once);
        }
    }

    [TestFixture]
    public class CardPrintFactoryTests
    {
        private CardPrintFactory _factory;

        [SetUp]
        public void SetUp()
        {
            _factory = new CardPrintFactory();
        }

        [Test]
        public void CreatePrinter_WhenFilm_ReturnsCardWithFilm()
        {
            var film = new Film( "Интерстеллар", "Фильм", "Фантастика", "Просмотрено", "poster.jpg", "169");

            WorkCard card = _factory.CreatePrinter(film);

            Assert.That(card, Is.Not.Null);
            Assert.That(card.GetWork(), Is.SameAs(film));
        }

        [Test]
        public void CreatePrinter_WhenSerial_ReturnsCardWithSerial()
        {
            var serial = new Serial("Шерлок", "Сериал", "Детектив", "Смотрю", "sherlock.jpg", 4, 13);

            WorkCard card = _factory.CreatePrinter(serial);

            Assert.That(card, Is.Not.Null);
            Assert.That(card.GetWork(), Is.SameAs(serial));
        }

        [Test]
        public void CreatePrinter_WhenUnsupportedWork_ThrowsException()
        {
            var work = new Work("Неизвестное произведение", "Другое", "Драма", "Просмотрено", "poster.jpg");

            Assert.Throws<NotSupportedException>(
                () => _factory.CreatePrinter(work));
        }
    }

    [TestFixture]
    public class CatalogViewModelTests
    {
        private Mock<IWorkRepository> workRepositoryMock;
        private Mock<ICardFactory> printFactoryMock;
        private CatalogViewModel viewModel;

        [SetUp]
        public void Setup()
        {
            workRepositoryMock = new Mock<IWorkRepository>();
            printFactoryMock = new Mock<ICardFactory>();

            viewModel = new CatalogViewModel(workRepositoryMock.Object, printFactoryMock.Object);
        }

        [Test]
        public void Constructor_InitializesWorks()
        {
            var result = viewModel.Works;

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetWorks_AddsFilmToCollection()
        {
            var film = new Film("Интерстеллар", "Фильм", "Фантастика", "Просмотрено", "poster.jpg", "169 минут");
            var template = new DataTemplate();
            var printerMock = new Mock<ICardPrint>();
            printerMock.Setup(x => x.GetTemplate()).Returns(template);
            var workCard = new WorkCard(film, printerMock.Object);
            workRepositoryMock.Setup(x => x.GetWorks()).Returns(new List<Work> { film });
            printFactoryMock.Setup(x => x.CreatePrinter(film)).Returns(workCard);

            viewModel.GetWorks();

            Assert.That(viewModel.Works, Has.Count.EqualTo(1));
            Assert.That(viewModel.Works[0].Work, Is.SameAs(film));
            Assert.That(viewModel.Works[0].Template, Is.SameAs(template));
        }

        [Test]
        public void GetWorks_AddsSerialToCollection()
        {
            var serial = new Serial("Шерлок", "Сериал", "Детектив", "Просмотрено", "poster.jpg", 4, 13);
            var template = new DataTemplate();
            var printerMock = new Mock<ICardPrint>();
            printerMock.Setup(x => x.GetTemplate()).Returns(template);
            var workCard = new WorkCard(serial, printerMock.Object);
            workRepositoryMock.Setup(x => x.GetWorks()).Returns(new List<Work> { serial });
            printFactoryMock.Setup(x => x.CreatePrinter(serial)).Returns(workCard);

            viewModel.GetWorks();

            Assert.That(viewModel.Works, Has.Count.EqualTo(1));
            Assert.That(viewModel.Works[0].Work, Is.SameAs(serial));
            Assert.That(viewModel.Works[0].Template, Is.SameAs(template));
        }

        [Test]
        public void GetWorks_AddsAllWorks()
        {
            var film = new Film("Интерстеллар", "Фильм", "Фантастика", "Просмотрено", "poster.jpg", "169 минут");

            var serial = new Serial("Шерлок", "Сериал", "Детектив", "Просмотрено", "poster.jpg", 4, 13);

            var works = new List<Work>
            {
                film,
                serial
            };

            workRepositoryMock.Setup(x => x.GetWorks()).Returns(works);

            foreach (var work in works)
            {
                var printerMock = new Mock<ICardPrint>();
                printerMock.Setup(x => x.GetTemplate()).Returns(new DataTemplate());
                var workCard = new WorkCard(work, printerMock.Object);
                printFactoryMock.Setup(x => x.CreatePrinter(work)).Returns(workCard);
            }

            viewModel.GetWorks();

            Assert.That(viewModel.Works, Has.Count.EqualTo(2));
            Assert.That(viewModel.Works[0].Work, Is.SameAs(film));
            Assert.That(viewModel.Works[1].Work, Is.SameAs(serial));
        }

        [Test]
        public void GetWorks_ClearsPreviousWorks()
        {
            viewModel.Works.Add(new WorkCardViewModel());
            workRepositoryMock.Setup(x => x.GetWorks()).Returns(new List<Work>());

            viewModel.GetWorks();

            Assert.That(viewModel.Works, Is.Empty);
        }

        [Test]
        public void GetWorks_WhenCollectionIsEmpty_ReturnsEmptyCollection()
        {
            workRepositoryMock.Setup(x => x.GetWorks()).Returns(new List<Work>());

            viewModel.GetWorks();

            Assert.That(viewModel.Works, Is.Empty);

            printFactoryMock.Verify(x => x.CreatePrinter(It.IsAny<Work>()), Times.Never);
        }
    }
}

