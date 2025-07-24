using AutoMapper;
using EventManagementAPI.Dtos.Requests;
using EventManagementAPI.Dtos.Responses;
using EventManagementAPI.DTOs;
using EventManagementAPI.Mapper;
using EventManagementAPI.Models;
using EventManagementAPI.Models.Requests;
using EventManagementAPI.Repositories;
using EventManagementAPI.Repositories.Interfaces;

namespace EventManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepo;
        private readonly UserMapper _mapper;
        public UserService(IUserRepository userRepo,UserMapper mapper)
        {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<bool> SignupAsync(SignupRequestDto req)
        {
            if (req.Password != req.ConfirmPassword)
                throw new ArgumentException("Passwords do not match");

            bool emailExists = await _userRepo.EmailExistsAsync(req.Email);
            if (emailExists)
                throw new ArgumentException("Email already exists");

            var userModel = _mapper.ConvertToSignUpModel(req);
            return await _userRepo.RegisterUserAsync(userModel);
        }

        public async Task<UserResponseDto?> LoginAsync(LoginRequestDto req)
        {
            var user = await _userRepo.GetUserByEmailAsync(req.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.Enc_Password))
                return null;

            return _mapper.ConvertToUserDto(user);
        }

        public async Task<bool> RegisterUserAsync(SignupRequestDto req)
        {
            var request = _mapper.ConvertToSignUpModel(req);
            return await _userRepo.RegisterUserAsync(request);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _userRepo.EmailExistsAsync(email);
        }

        public async Task<UserResponseDto?> GetUserByEmailAsync(string email)
        {
            var user = await _userRepo.GetUserByEmailAsync(email);
            return user == null ? null : _mapper.ConvertToUserDto(user);
        }

        public async Task<IEnumerable<UserResponseDto>> GetAllUsersAsync()
        {
            var users = await _userRepo.GetAllUsersAsync();
            return _mapper.ConvertToUserDto(users);
        }

        public async Task<string> SaveProfilePictureAsync(IFormFile file)
        {
            return await _userRepo.SaveProfilePictureAsync(file);
        }

        public async Task<IEnumerable<EventResponseDto>> GetEventsByUserAsync(int userId)
        {
            var events = await _userRepo.GetEventsByUserAsync(userId);
            return _mapper.ConvertToEventDtoList(events);
        }

        public async Task<IEnumerable<EventResponseDto>> GetUpcomingEventsAsync(int userId)
        {
            var events = await _userRepo.GetUpcomingEventsAsync(userId);
            return _mapper.ConvertToEventDtoList(events);
        }

        public async Task<IEnumerable<EventResponseDto>> GetPastEventsAsync(int userId)
        {
            var events = await _userRepo.GetPastEventsAsync(userId);
            return _mapper.ConvertToEventDtoList(events);
        }

        public async Task<IEnumerable<EventResponseDto>> GetAcceptedEventsAsync(int userId)
        {
            var events = await _userRepo.GetAcceptedEventsAsync(userId);
            return _mapper.ConvertToEventDtoList(events);
        }
    }
}
