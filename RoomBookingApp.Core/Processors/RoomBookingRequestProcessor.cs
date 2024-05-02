using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Domain;
using RoomBookingApp.Domain.BaseModels;

namespace RoomBookingApp.Core.Processors;

public class RoomBookingRequestProcessor(IRoomBookingService roomBookingService) : IRoomBookingRequestProcessor
{
    public RoomBookingResult BookRoom(RoomBookingRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        var availableRooms = roomBookingService.GetAvailableRooms(request.Date);
        var result = CreateRoomBookingObject<RoomBookingResult>(request);

        if (availableRooms.Any()) {
            var room = availableRooms.First();
            var roomBooking = CreateRoomBookingObject<RoomBooking>(request);
            roomBooking.RoomId = room.Id;
            roomBookingService.Save(roomBooking);
            result.Flag = BookingResultFlag.Success;
            result.RoomBookingId = roomBooking.Id;
        } else {
            result.Flag = BookingResultFlag.Failure;
        }

        return result;
    }

    private static TRoomBooking CreateRoomBookingObject<TRoomBooking>(RoomBookingBase request) where TRoomBooking : RoomBookingBase, new()
    {
        return new TRoomBooking
        {
            FullName = request.FullName,
            Email = request.Email,
            Date = request.Date
        };
    }
}