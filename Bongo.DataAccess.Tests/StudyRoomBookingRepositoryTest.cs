using Bongo.DataAccess.Repository;
using Bongo.Models.Model;
using FluentAssertions;
using FluentAssertions.AssertMultiple;
using FluentAssertions.Execution;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Bongo.DataAccess;

public class StudyRoomBookingRepositoryTest
{
    private StudyRoomBooking _studyRoomBooking_One;
    private StudyRoomBooking _studyRoomBooking_Two;
    private DbContextOptions<ApplicationDbContext> _options;

    public StudyRoomBookingRepositoryTest()
    {
        _studyRoomBooking_One = new StudyRoomBooking()
        {
            FirstName = "Ben1",
            LastName = "Spark1",
            Date = new DateTime(2023, 1, 1),
            Email = "ben1@mail.com",
            BookingId = 11,
            StudyRoomId = 1
        };

        _studyRoomBooking_Two = new StudyRoomBooking()
        {
            FirstName = "Ben2",
            LastName = "Spark2",
            Date = new DateTime(2023, 2, 2),
            Email = "ben2@mail.com",
            BookingId = 12,
            StudyRoomId = 2
        };

        _options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: "temp_Bongo")
            .Options;

    }

    [Fact]
    public void SaveBooking_Booking_One_CheckTheValuesFromDatabase()
    {
        //Arrange
        //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //    .UseInMemoryDatabase(databaseName: "temp_Bongo")
        //    .Options;

        //Act
        using var context = new ApplicationDbContext(_options);
        var repository = new StudyRoomBookingRepository(context);
        context.Database.EnsureDeleted();
        repository.Book(_studyRoomBooking_One);

        //Assert
        //using var context = new ApplicationDbContext(options);
        var bookingFromDb = context.StudyRoomBookings.FirstOrDefault(u => u.BookingId == 11);

        // 1.
        //_studyRoomBooking_One.Should().BeEquivalentTo(bookingFromDb);

        // 2.
        using (new AssertionScope())
        {
            _studyRoomBooking_One.BookingId.Should().Be(bookingFromDb.BookingId);
            _studyRoomBooking_One.FirstName.Should().Be(bookingFromDb.FirstName);
            _studyRoomBooking_One.LastName.Should().Be(bookingFromDb.LastName);
            _studyRoomBooking_One.Email.Should().Be(bookingFromDb.Email);
            _studyRoomBooking_One.Date.Should().Be(bookingFromDb.Date);
        }

        // 3.
        //AssertMultiple.Multiple(() =>
        //{
        //    Assert.Equal(_studyRoomBooking_One.BookingId, bookingFromDb.BookingId);
        //    Assert.Equal(_studyRoomBooking_One.FirstName, bookingFromDb.FirstName);
        //    Assert.Equal(_studyRoomBooking_One.LastName, bookingFromDb.LastName);
        //    Assert.Equal(_studyRoomBooking_One.Email, bookingFromDb.Email);
        //    Assert.Equal(_studyRoomBooking_One.Date, bookingFromDb.Date);
        //});

        // 4.
        //Assert.Equal(_studyRoomBooking_One.BookingId, bookingFromDb.BookingId);
        //Assert.Equal(_studyRoomBooking_One.FirstName, bookingFromDb.FirstName);
        //Assert.Equal(_studyRoomBooking_One.LastName, bookingFromDb.LastName);
        //Assert.Equal(_studyRoomBooking_One.Email, bookingFromDb.Email);
        //Assert.Equal(_studyRoomBooking_One.Date, bookingFromDb.Date);
    }

    [Fact]
    public void SaveBooking_Booking_OneAndTwo_CheckBoththeBookingFromDatabase()
    {
        //Arrange
        var expectedResult = new List<StudyRoomBooking>() { _studyRoomBooking_One, _studyRoomBooking_Two };
        //var options = new DbContextOptionsBuilder<ApplicationDbContext>()
        //    .UseInMemoryDatabase(databaseName: "temp_Bongo")
        //    .Options;

        //Act
        using var context = new ApplicationDbContext(_options);
        context.Database.EnsureDeleted();
        var repository = new StudyRoomBookingRepository(context);
        repository.Book(_studyRoomBooking_One);
        repository.Book(_studyRoomBooking_Two);

        List<StudyRoomBooking> actualList;
        actualList = repository.GetAll(null).ToList();

        //Assert
        expectedResult.Should().BeEquivalentTo(actualList);
    }

}
