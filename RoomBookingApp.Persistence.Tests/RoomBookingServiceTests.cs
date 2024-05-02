using Microsoft.EntityFrameworkCore;
using RoomBookingApp.Domain;
using RoomBookingApp.Persistence.Repositories;

namespace RoomBookingApp.Persistence.Tests;

public class RoomBookingServiceTests
{
    [Fact]
    public void Should_Return_Available_Rooms()
    {
        var date = new DateTime(2024, 01, 01);

        var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
            .UseInMemoryDatabase("AvailableRoomsTest")
            .Options;

        using var context = new RoomBookingAppDbContext(dbOptions);
        context.Add(new Room { Id = 1, Name = "Room 1" });
        context.Add(new Room { Id = 2, Name = "Room 2" });
        context.Add(new Room { Id = 3, Name = "Room 3" });

        context.Add(new RoomBooking { RoomId = 1, Date = date });
        context.Add(new RoomBooking { RoomId = 2, Date = date.AddDays(-1) });

        context.SaveChanges();

        var roomBookingService = new RoomBookingService(context);

        var availableRooms = roomBookingService.GetAvailableRooms(date);
        
        Assert.Equal(2, availableRooms.Count());
        Assert.Contains(availableRooms, q => q.Id == 2);
        Assert.Contains(availableRooms, q => q.Id == 3);
        Assert.DoesNotContain(availableRooms, q => q.Id == 1);
    }

    [Fact]
    public void Should_Save_Room_Booking()
    {
        var dbOptions = new DbContextOptionsBuilder<RoomBookingAppDbContext>()
            .UseInMemoryDatabase("ShouldSaveRoomTest")
            .Options;
        
        using var context = new RoomBookingAppDbContext(dbOptions);
        var roomBookingService = new RoomBookingService(context);
        
        var roomBooking = new RoomBooking { 
            RoomId = 1, 
            Email = "test@test.com", 
            FullName = "test", 
            Date = new DateTime(2024, 01, 01) 
        };
        
        roomBookingService.Save(roomBooking);

        var bookings = context.RoomBookings.ToList();
        var booking = Assert.Single(bookings);
        
        Assert.Equal(1, booking.Id);
        Assert.Equal(roomBooking.RoomId, booking.RoomId);
        Assert.Equal(roomBooking.Date, booking.Date);
    }
}