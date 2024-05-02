using Microsoft.AspNetCore.Mvc;
using Moq;
using RoomBookingApp.Api.Controllers;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Api.Tests;

public class RoomBookingControllerTests
{
    private readonly Mock<IRoomBookingRequestProcessor> _processorMock;
    private readonly RoomBookingController _controller;
    private readonly RoomBookingRequest _request;
    private readonly RoomBookingResult _result;

    public RoomBookingControllerTests()
    {
        _processorMock = new Mock<IRoomBookingRequestProcessor>();
        _controller = new RoomBookingController(_processorMock.Object);
        _request = new RoomBookingRequest();
        _result = new RoomBookingResult();

        _processorMock.Setup(q => q.BookRoom(_request))
            .Returns(_result);
    }

    [Theory]
    [InlineData(1, true, typeof(OkObjectResult), BookingResultFlag.Success)]
    [InlineData(0, false, typeof(BadRequestObjectResult), BookingResultFlag.Failure)]
    public async Task Should_Call_Booking_Method_When_Valid(
        int expectedMethodCalls, 
        bool isModelValid, 
        Type expectedActionTypeResult,
        BookingResultFlag resultFlag
        )
    {
        if (!isModelValid) _controller.ModelState.AddModelError("Key", "ErrorMessage");
        _result.Flag = resultFlag;

        var result = await _controller.BookRoom(_request);
        
        Assert.IsType(expectedActionTypeResult, result);
        _processorMock.Verify(x => x.BookRoom(_request), Times.Exactly(expectedMethodCalls));
    }
}