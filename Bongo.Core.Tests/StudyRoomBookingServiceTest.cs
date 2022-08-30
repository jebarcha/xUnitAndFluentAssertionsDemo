using Bongo.Core.Services;
using Bongo.DataAccess.Repository.IRepository;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using FluentAssertions;
using Moq;

namespace Bongo.Core;

public class StudyRoomBookingServiceTest
{
    private StudyRoomBooking _request;
    private List<StudyRoom> _availableStudyRoom;

    private Mock<IStudyRoomBookingRepository> _studyRoomBookingRepoMock;
    private Mock<IStudyRoomRepository> _studyRoomRepoMock;
    private StudyRoomBookingService _bookingService;

    public StudyRoomBookingServiceTest()
    {
        _request = new StudyRoomBooking
        {
            FirstName = "Jose",
            LastName = "Barajas",
            Email = "jose.barajas@mail.com",
            Date = new DateTime(2022, 9, 1)
        };

        _availableStudyRoom = new List<StudyRoom>
        {
            new StudyRoom
            {
                Id = 10,
                RoomName = "Saltillo",
                RoomNumber = "A202"
            }
        };

        _studyRoomBookingRepoMock = new Mock<IStudyRoomBookingRepository>();
        _studyRoomRepoMock = new Mock<IStudyRoomRepository>();
        _bookingService = new StudyRoomBookingService(
            _studyRoomBookingRepoMock.Object, 
            _studyRoomRepoMock.Object
            );

        _studyRoomRepoMock.Setup(x => x.GetAll()).Returns(_availableStudyRoom);
    }

    [Fact]
    public void GetAllBooking_InvokedMethod_CheckIfRepoIsCalled()
    {
           _bookingService.GetAllBooking();
        _studyRoomBookingRepoMock.Verify(x => x.GetAll(null), Times.Once);
    }

    [Fact]
    public void BookStudyRoom_NullRequest_ThrowsArgumentNullException()
    {
        //1.FluentAssertion one line:
        //_bookingService.Invoking(y => y.BookStudyRoom(null))
        //    .Should().Throw<ArgumentNullException>()
        //        .WithMessage("Value cannot be null. (Parameter 'request')");

        //2.arrange-act-assert syntax:
        Action act = () => _bookingService.BookStudyRoom(null);

        act.Should().Throw<ArgumentNullException>()
            .WithMessage("Value cannot be null. (Parameter 'request')");

        //3.xUnit
        //var exception = Assert.Throws<ArgumentNullException>(
        //    () => _bookingService.BookStudyRoom(null)
        //    );
        ////Assert.Equal("Value cannot be null. (Parameter 'request'", exception.Message);
        //Assert.Equal("request", exception.ParamName);
        //Assert.IsType<ArgumentNullException>(exception);

    }

    [Fact]
    public void StudyRoomBooking_SaveBookingWithAvailableRoom_ReturnsResultWithAllValues()
    {
        StudyRoomBooking savedStudyRoomBooking = null;
        _studyRoomBookingRepoMock.Setup(x => x.Book(It.IsAny<StudyRoomBooking>()))
            .Callback<StudyRoomBooking>(booking =>
            {
                savedStudyRoomBooking = booking;
            });

        //act
        _bookingService.BookStudyRoom(_request);

        //asssert
        _studyRoomBookingRepoMock.Verify(x => x.Book(It.IsAny<StudyRoomBooking>()), Times.Once);
        Assert.NotNull(savedStudyRoomBooking);
        Assert.Equal(_request.FirstName, savedStudyRoomBooking.FirstName);
        Assert.Equal(_request.LastName, savedStudyRoomBooking.LastName);
        Assert.Equal(_request.Email, savedStudyRoomBooking.Email);
        Assert.Equal(_request.Date, savedStudyRoomBooking.Date);
        Assert.Equal(_availableStudyRoom.First().Id, savedStudyRoomBooking.StudyRoomId);
    }

    [Fact]
    public void StudyRoomBookingResultCheck_InputRequest_ValuesMatchInResult()
    {
        StudyRoomBookingResult result = _bookingService.BookStudyRoom(_request);

        Assert.NotNull(result);
        Assert.Equal(_request.FirstName, result.FirstName);
        Assert.Equal(_request.LastName, result.LastName);
        Assert.Equal(_request.Email, result.Email);
        Assert.Equal(_request.Date, result.Date);
    }

    [Theory]
    [InlineData(true, StudyRoomBookingCode.Success)]
    [InlineData(false, StudyRoomBookingCode.NoRoomAvailable)]
    public void ResultCodeSuccess_RoomAvailability_ReturnSucessResultCode(bool roomAvailability, StudyRoomBookingCode expectedCode)
    {
        var result = _bookingService.BookStudyRoom(_request);

        if (!roomAvailability)
        {
            _availableStudyRoom.Clear();
        }
        var resultCode = _bookingService.BookStudyRoom(_request).Code;

        Assert.Equal(expectedCode, resultCode);
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(55, true)]
    public void StudyRoomBooking_BookRoomWithAvailability_ReturnsBookingId(int expectedBookingId, bool roomAvailability)
    {
        if (!roomAvailability)
        {
            _availableStudyRoom.Clear();
        }

        _studyRoomBookingRepoMock.Setup(x => x.Book(It.IsAny<StudyRoomBooking>()))
            .Callback<StudyRoomBooking>(booking =>
            {
                booking.BookingId = 55;
            });

        var result = _bookingService.BookStudyRoom(_request);
        Assert.Equal(expectedBookingId, result.BookingId);
    }

    [Fact]
    public void BookNotInvoked_SaveBookingWithoutAvailableRoom_BookMethodNotInvoked()
    {
        _availableStudyRoom.Clear();

        var result = _bookingService.BookStudyRoom(_request);

        _studyRoomBookingRepoMock.Verify(x => x.Book(It.IsAny<StudyRoomBooking>()), Times.Never);
    }
}
