using Bongo.Core.Services.IServices;
using Bongo.Models.Model;
using Bongo.Models.Model.VM;
using Bongo.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Bongo.Web;

public class RoomBookingControllerTest
{

    private Mock<IStudyRoomBookingService> _studyRoomBookingService;
    private RoomBookingController _bookingController;

    public RoomBookingControllerTest()
    {
        _studyRoomBookingService = new Mock<IStudyRoomBookingService>();
        _bookingController = new RoomBookingController(_studyRoomBookingService.Object);
    }

    [Fact]
    public void IndexPage_CallRequest_VerifyGetAllInvoked()
    {
        _bookingController.Index();
        _studyRoomBookingService.Verify(x => x.GetAllBooking(), Times.Once);
    }

    [Fact]
    public void BookRoomCheck_ModelStateInvalid_ReturnViewBook()
    {
        _bookingController.ModelState.AddModelError("test", "test");

        var result = _bookingController.Book(new StudyRoomBooking());

        ViewResult viewResult = result as ViewResult;
        Assert.Equal("Book", viewResult.ViewName);
    }

    [Fact]
    public void BookRoomCheck_NoRoomAvailable_ReturnNoRoomCode()
    {
        _studyRoomBookingService.Setup(x => x.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
            .Returns(new StudyRoomBookingResult()
            {
                Code = StudyRoomBookingCode.NoRoomAvailable
            });

        var result = _bookingController.Book(new StudyRoomBooking());

        Assert.IsType<ViewResult>(result);

        ViewResult viewResult = result as ViewResult;
        Assert.Equal("No Study Room available for selected date", viewResult.ViewData["Error"]);
    }

    [Fact]
    public void BookRoomCheck_Successful_SuccessCodeAndRedirect()
    {
        _studyRoomBookingService.Setup(x => x.BookStudyRoom(It.IsAny<StudyRoomBooking>()))
            .Returns((StudyRoomBooking booking) => new StudyRoomBookingResult()
            {
                Code = StudyRoomBookingCode.Success,
                FirstName = booking.FirstName,
                LastName = booking.LastName,
                Email = booking.Email,
                Date = booking.Date
            });

        var result = _bookingController.Book(new StudyRoomBooking()
        {
            Date = DateTime.Now,
            Email = "test@test.com",
            FirstName = "test",
            LastName = "lntest",
            StudyRoomId = 1
        });

        Assert.IsType<RedirectToActionResult>(result);

        RedirectToActionResult actionResult = result as RedirectToActionResult;
        Assert.Equal("test", actionResult.RouteValues["FirstName"]);
        Assert.Equal(StudyRoomBookingCode.Success, actionResult.RouteValues["Code"]);
    }
}
