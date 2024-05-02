using RoomBookingApp.Domain;

namespace RoomBookingApp.Core.DataServices;

public interface IRoomBookingService
{
    void Save(RoomBooking booking);

    List<Room> GetAvailableRooms(DateTime date);
}