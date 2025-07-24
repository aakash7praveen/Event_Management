using EventManagementAPI.Dtos.Requests;
using EventManagementAPI.Models;
using EventManagementAPI.Models.Analytics;
using EventManagementAPI.Models.Requests;
using EventManagementAPI.Repositories;
using EventManagementAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EventManagementAPI.Services;

namespace EventManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequestDto createEventRequestDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var eventId = await _eventService.CreateEvent(createEventRequestDto);
            return CreatedAtAction(nameof(GetEventById), new { id = eventId }, new { id = eventId });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var result = await _eventService.GetEventById(id);
            return result == null ? NotFound() : Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventService.GetAllEvents();
            return Ok(events);
        }

        [HttpDelete("{eventId}/cancel")]
        public async Task<IActionResult> CancelEvent(int eventId)
        {
            var success = await _eventService.CancelEvent(eventId);
            return success
                ? Ok(new { success = true, message = "Event cancelled successfully." })
                : StatusCode(500, new { success = false, message = "Failed to cancel event." });
        }

        [HttpGet("todays-events")]
        public async Task<IActionResult> GetTodaysEvents() =>
            Ok(await _eventService.GetTodaysEvents());

        [HttpPost("rsvp")]
        public async Task<IActionResult> RSVPToEvent([FromBody] RsvpRequestDto request)
        {
            var response = await _eventService.RSVPToEvent(request);
            return response
                ? Ok(new { message = "RSVP successful" })
                : BadRequest(new { message = "Failed to RSVP." });
        }

        [HttpDelete("rsvp/remove")]
        public async Task<IActionResult> RemoveRsvp([FromQuery] int userId, [FromQuery] int eventId)
        {
            if (userId <= 0 || eventId <= 0)
                return BadRequest(new { message = "Invalid userId or eventId." });

            var success = await _eventService.RemoveRsvp(userId, eventId);
            return success
                ? Ok(new { message = "RSVP removed successfully.", userId, eventId })
                : NotFound(new { message = "RSVP not found or already removed.", userId, eventId });
        }

        [HttpGet("admin/metrics")]
        public async Task<IActionResult> GetAdminMetrics() =>
            Ok(await _eventService.GetAdminMetrics());

        [HttpGet("{id}/attendees")]
        public async Task<IActionResult> GetUsersByEvent(int id) =>
            Ok(await _eventService.GetUsersByEvent(id));

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequestDto request)
        {
            var success = await _eventService.UpdateEvent(request);
            return success
                ? Ok(new { success = true, message = "Event updated successfully" })
                : BadRequest(new { success = false, message = "Failed to update the event" });
        }

        [HttpGet("has-rsvped")]
        public async Task<IActionResult> HasUserRsvped([FromQuery] int userId, [FromQuery] int eventId) =>
            Ok(await _eventService.HasUserRsvped(userId, eventId));

        [HttpGet("analytics/category-count")]
        public async Task<IActionResult> GetEventCountByCategory() =>
            Ok(await _eventService.GetEventCountByCategory());

        [HttpGet("analytics/daily-count")]
        public async Task<IActionResult> GetDailyEventCount() =>
            Ok(await _eventService.GetEventCountPerDay());

        [HttpGet("analytics/top-venues")]
        public async Task<IActionResult> GetTopVenues() =>
            Ok(await _eventService.GetTopVenues());

        [HttpGet("analytics/rsvp-counts")]
        public async Task<IActionResult> GetRsvpCounts() =>
            Ok(await _eventService.GetRsvpCountPerEvent());
    }
}
