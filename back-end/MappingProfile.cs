using AutoMapper;
using EventManagementAPI.Dtos.Requests;
using EventManagementAPI.Dtos.Responses;
using EventManagementAPI.Models;
using EventManagementAPI.Models.Requests;


namespace EventManagementAPI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateEventRequestDto, CreateEventRequest>();
            CreateMap<AdminDashboardMetrics, AdminDashboardMetricsDto>();
            CreateMap<SystemUser, UserResponseDto>();
            CreateMap<UpdateEventRequestDto, UpdateEventRequest>();
            CreateMap<RsvpRequestDto, RsvpRequest>();
        }
    }
}
