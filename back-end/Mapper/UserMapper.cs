using AutoMapper;
using EventManagementAPI.DTOs;
using EventManagementAPI.Dtos.Requests;
using EventManagementAPI.Dtos.Responses;
using EventManagementAPI.Models;
using EventManagementAPI.Models.Requests;

namespace EventManagementAPI.Mapper
{
    public class UserMapper
    {
        private readonly IMapper _mapper;

        public UserMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        // Signup mappings
        public SignupRequest ConvertToSignUpModel(SignupRequestDto signupRequestDto)
        {
            return _mapper.Map<SignupRequest>(signupRequestDto);
        }

        public SignupRequestDto ConvertToSignUpDto(SignupRequest signupRequest)
        {
            return _mapper.Map<SignupRequestDto>(signupRequest);
        }

        // Login mappings
        public LoginRequest ConvertToLoginModel(LoginRequestDto loginRequestDto)
        {
            return _mapper.Map<LoginRequest>(loginRequestDto);
        }

        public LoginRequestDto ConvertToLoginDto(LoginRequest loginRequest)
        {
            return _mapper.Map<LoginRequestDto>(loginRequest);
        }

        // User response mappings
        public UserResponseDto ConvertToUserDto(SystemUser user)
        {
            return _mapper.Map<UserResponseDto>(user);
        }

        public IEnumerable<UserResponseDto> ConvertToUserDto(IEnumerable<SystemUser> users)
        {
            return _mapper.Map<IEnumerable<UserResponseDto>>(users);
        }

        // Event mappings
        public EventResponseDto ConvertToEventDto(Event eventModel)
        {
            return _mapper.Map<EventResponseDto>(eventModel);
        }

        public Event ConvertToEvent(EventResponseDto eventDto)
        {
            return _mapper.Map<Event>(eventDto);
        }

        public IEnumerable<EventResponseDto> ConvertToEventDtoList(IEnumerable<Event> events)
        {
            return _mapper.Map<IEnumerable<EventResponseDto>>(events);
        }

        public IEnumerable<Event> ConvertToEventList(IEnumerable<EventResponseDto> eventDtos)
        {
            return _mapper.Map<IEnumerable<Event>>(eventDtos);
        }
    }
}
