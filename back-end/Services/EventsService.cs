﻿using AutoMapper;
using Azure.Core;
using EventManagementAPI.Dtos.Requests;
using EventManagementAPI.Dtos.Responses;
using EventManagementAPI.Mapper;
using EventManagementAPI.Models;
using EventManagementAPI.Models.Requests;
using EventManagementAPI.Repositories;

namespace EventManagementAPI.Services
{
    //Create interfaces that will be consumed by the controller
    public class EventsService : IEventService
    {
        private readonly EventRepository _eventRepo;
        private readonly EventMapper _mapper;
        public EventsService(EventRepository eventRepository, EventMapper mapper)
        {
            _eventRepo = eventRepository;
            _mapper = mapper;
        }
        public async Task<int> CreateEvent(CreateEventRequestDto dto)
        {
            var request = _mapper.ConvertToCreateEventModel(dto);
            return await _eventRepo.CreateEventAsync(request);
        }

        public async Task<EventResponseDto?> GetEventById(int id)
        {
            var ev = await _eventRepo.GetEventByIdAsync(id);
            return ev == null ? null : _mapper.ConvertToEventDto(ev);
        }

        public async Task<IEnumerable<EventResponseDto>> GetAllEvents()
        {
            var events = await _eventRepo.GetAllEventsAsync();
            return _mapper.ConvertToEventDtoList(events);
        }

        public async Task<bool> CancelEvent(int eventId)
        {
            return await _eventRepo.CancelEventAsync(eventId) > 0;
        }

        public async Task<IEnumerable<EventResponseDto>> GetTodaysEvents()
        {
            var events = await _eventRepo.GetTodaysEventsAsync();
            return _mapper.ConvertToEventDtoList(events);
        }

        public async Task<bool> RSVPToEvent(RsvpRequestDto request)
        {
            var rsvpModel = _mapper.ConvertToRsvpModel(request);
            return await _eventRepo.RSVPToEventAsync(rsvpModel);
        }

        public async Task<bool> UpdateEvent(UpdateEventRequestDto request)
        {
            var requestModel = _mapper.ConvertToUpdateEventModel(request);
            return await _eventRepo.UpdateEventAsync(requestModel);
        }

        public async Task<bool> RemoveRsvp(int userId, int eventId)
        {
            return await _eventRepo.RemoveRsvpAsync(userId, eventId);
        }

        public async Task<AdminDashboardMetricsDto> GetAdminMetrics()
        {
            var metrics = await _eventRepo.GetAdminMetricsAsync();
            return _mapper.ConvertToMetricsDto(metrics);
        }

        public async Task<IEnumerable<UserResponseDto>> GetUsersByEvent(int eventId)
        {
            var users = await _eventRepo.GetUsersByEventAsync(eventId);
            return _mapper.ConvertToUserDtoList(users);
        }

        public async Task<bool> HasUserRsvped(int userId, int eventId)
        {
            return await _eventRepo.HasUserRsvpedAsync(userId, eventId);
        }

        public async Task<IEnumerable<CategoryCountDto>> GetEventCountByCategory()
        {
            var data = await _eventRepo.GetEventCountByCategoryAsync();
            return _mapper.ConvertToCategoryDtoList(data);
        }

        public async Task<IEnumerable<DailyEventCountDto>> GetEventCountPerDay()
        {
            var data = await _eventRepo.GetEventCountPerDayAsync();
            return _mapper.ConvertToDailyCountDtoList(data);
        }

        public async Task<IEnumerable<TopVenueDto>> GetTopVenues()
        {
            var data = await _eventRepo.GetTopVenuesAsync();
            return _mapper.ConvertToTopVenueDtoList(data);
        }

        public async Task<IEnumerable<RsvpCountDto>> GetRsvpCountPerEvent()
        {
            var data = await _eventRepo.GetRsvpCountPerEventAsync();
            return _mapper.ConvertToRsvpCountDtoList(data);
        }
    }
}
