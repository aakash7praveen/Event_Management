using AutoMapper;
using EventManagementAPI.Dtos.Requests;
using EventManagementAPI.Dtos.Responses;
using EventManagementAPI.DTOs;
using EventManagementAPI.Models;
using EventManagementAPI.Models.Analytics;
using EventManagementAPI.Models.Requests;

namespace EventManagementAPI.Mapper
{
    public class EventMapper
    {
        private readonly IMapper _mapper;

        public EventMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        // Create Event
        public CreateEventRequest ConvertToCreateEventModel(CreateEventRequestDto dto)
        {
            return _mapper.Map<CreateEventRequest>(dto);
        }

        // Update Event
        public UpdateEventRequest ConvertToUpdateEventModel(UpdateEventRequestDto dto)
        {
            return _mapper.Map<UpdateEventRequest>(dto);
        }

        // RSVP
        public RsvpRequest ConvertToRsvpModel(RsvpRequestDto dto)
        {
            return _mapper.Map<RsvpRequest>(dto);
        }

        // Single Event
        public EventResponseDto ConvertToEventDto(Event eventModel)
        {
            return _mapper.Map<EventResponseDto>(eventModel);
        }

        public IEnumerable<EventResponseDto> ConvertToEventDtoList(IEnumerable<Event> events)
        {
            return _mapper.Map<IEnumerable<EventResponseDto>>(events);
        }

        // Users per Event
        public IEnumerable<UserResponseDto> ConvertToUserDtoList(IEnumerable<SystemUser> users)
        {
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        // Metrics
        public AdminDashboardMetricsDto ConvertToMetricsDto(AdminDashboardMetrics model)
        {
            return _mapper.Map<AdminDashboardMetricsDto>(model);
        }

        public IEnumerable<CategoryCountDto> ConvertToCategoryDtoList(IEnumerable<CategoryCount> categories)
        {
            return _mapper.Map<IEnumerable<CategoryCountDto>>(categories);
        }

        public IEnumerable<DailyEventCountDto> ConvertToDailyCountDtoList(IEnumerable<DailyEventCount> data)
        {
            return _mapper.Map<IEnumerable<DailyEventCountDto>>(data);
        }

        public IEnumerable<TopVenueDto> ConvertToTopVenueDtoList(IEnumerable<TopVenue> venues)
        {
            return _mapper.Map<IEnumerable<TopVenueDto>>(venues);
        }

        public IEnumerable<RsvpCountDto> ConvertToRsvpCountDtoList(IEnumerable<RsvpCount> rsvps)
        {
            return _mapper.Map<IEnumerable<RsvpCountDto>>(rsvps);
        }
    }
}
