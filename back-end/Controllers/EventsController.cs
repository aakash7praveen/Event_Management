using Microsoft.AspNetCore.Mvc;
using EventManagementAPI.Models;
using EventManagementAPI.Repositories;
using EventManagementAPI.Models.Requests;
using EventManagementAPI.Repositories.Interfaces;
using EventManagementAPI.Models.Analytics;

namespace EventManagementAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepo;

        public EventsController(IEventRepository eventRepo)
        {
            _eventRepo = eventRepo;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateEvent([FromBody] CreateEventRequest request)
        {
            var result = await _eventRepo.CreateEventAsync(request);

            if (result > 0)
            {
                return Ok(new
                {
                    success = true,
                    message = "Event created successfully."
                });
            }

            return StatusCode(500, new
            {
                success = false,
                message = "Failed to create event."
            });
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetAllEvents()
        {
            var events = await _eventRepo.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpDelete("{eventId}/cancel")]
        public async Task<IActionResult> CancelEvent(int eventId)
        {
            var result = await _eventRepo.CancelEventAsync(eventId);
            if (result > 0)
                return Ok(new { success = true, message = "Event cancelled successfully." });

            return StatusCode(500, new { success = false, message = "Failed to cancel event." });
        }

        [HttpGet("todays-events")]
        public async Task<IActionResult> GetTodaysEvents()
        {
            var events = await _eventRepo.GetTodaysEventsAsync();
            return Ok(events);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEventById(int id)
        {
            var evt = await _eventRepo.GetEventByIdAsync(id);
            return evt != null ? Ok(evt) : NotFound("Event not found.");
        }

        [HttpPost("rsvp")]
        public async Task<IActionResult> RSVPToEvent([FromBody] RsvpRequest request)
        {
            var response = await _eventRepo.RsvpToEventAsync(request);
            return response
                ? Ok(new { message = "RSVP successful" })
                : BadRequest(new { message = "Failed to RSVP." });
        }

        [HttpDelete("rsvp/remove")]
        public async Task<IActionResult> RemoveRsvp([FromQuery] int userId, [FromQuery] int eventId)
        {
            if (userId <= 0 || eventId <= 0)
            {
                return BadRequest(new
                {
                    message = "Invalid userId or eventId.",
                    userId,
                    eventId
                });
            }

            var success = await _eventRepo.RemoveRsvpAsync(userId, eventId);

            if (success)
            {
                return Ok(new
                {
                    message = "RSVP removed successfully.",
                    userId,
                    eventId
                });
            }

            return NotFound(new
            {
                message = "RSVP not found or already removed.",
                userId,
                eventId
            });
        }


        [HttpGet("admin/metrics")]
        public async Task<IActionResult> GetAdminMetrics()
        {
            var metrics = await _eventRepo.GetAdminMetricsAsync();
            return Ok(metrics);
        }

        [HttpGet("{id}/attendees")]
        public async Task<IActionResult> GetUsersByEvent(int id)
        {
            var users = await _eventRepo.GetUsersByEventAsync(id);
            return Ok(users);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateEvent([FromBody] UpdateEventRequest request)
        {
            var success = await _eventRepo.UpdateEventAsync(request);

            if (success)
            {
                return Ok(new
                {
                    success = true,
                    message = "Event updated successfully"
                });
            }
            else
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Failed to update the event"
                });
            }
        }


        [HttpGet("has-rsvped")]
        public async Task<IActionResult> HasUserRsvped([FromQuery] int userId, [FromQuery] int eventId)
        {
            var hasRsvped = await _eventRepo.HasUserRsvpedAsync(userId, eventId);
            return Ok(hasRsvped);
        }

        [HttpGet("analytics/category-count")]
        public async Task<IActionResult> GetEventCountByCategory() =>
            Ok(await _eventRepo.GetEventCountByCategoryAsync());

        [HttpGet("analytics/daily-count")]
        public async Task<IActionResult> GetDailyEventCount() =>
            Ok(await _eventRepo.GetEventCountPerDayAsync());

        [HttpGet("analytics/top-venues")]
        public async Task<IActionResult> GetTopVenues() =>
            Ok(await _eventRepo.GetTopVenuesAsync());

        [HttpGet("analytics/rsvp-counts")]
        public async Task<IActionResult> GetRsvpCounts() =>
            Ok(await _eventRepo.GetRsvpCountPerEventAsync());

    }
}
