using Microsoft.AspNetCore.Mvc;
using RoomBookingApp.Core.Enums;
using RoomBookingApp.Core.Models;
using RoomBookingApp.Core.Processors;

namespace RoomBookingApp.Api.Controllers
{   
    [ApiController]
    [Route("api/[controller]")]
    public class RoomBookingController(IRoomBookingRequestProcessor roomBookingProcessor) : ControllerBase
    {
        private readonly IRoomBookingRequestProcessor _roomBookingProcessor = roomBookingProcessor;

        [HttpPost]
        public async Task<IActionResult> BookRoom(RoomBookingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var result = _roomBookingProcessor.BookRoom(request);
            if (result.Flag == BookingResultFlag.Success) return Ok(result);
            ModelState.AddModelError(nameof(RoomBookingRequest.Date), "No rooms available of given date");
            return BadRequest(ModelState);
        }
    }
    
    
}
