using RoomBookingApp.Core.DataServices;
using RoomBookingApp.Domain;

namespace RoomBookingApp.Persistence.Repositories;

public class RoomBookingService(RoomBookingAppDbContext context) : IRoomBookingService
{
    public void Save(RoomBooking booking)
    {
        context.Add(booking);
        context.SaveChanges();
    }

    public List<Room> GetAvailableRooms(DateTime date)
    {
        return context.Rooms.Where(q => q.RoomBookings.All(x => x.Date != date)).ToList();
    }
}