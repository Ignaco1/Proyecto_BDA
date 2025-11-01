using Application.Interfaces;
using AutoMapper;
using Business.Interfaces;
using Domain.DTOs.Requests.User;
using Domain.DTOs.Responses;
using Domain.Entities;
using Domain.Intefaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class AuthService(ITokenService tokenService, IUserRepository userRepository, IMapper mapper) : IAuthService
    {
        private readonly ITokenService _tokenService = tokenService;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<LoginResponseDto> Login(LoginRequestDto loginRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginRequest.Email);
            var isAuthenticated = user != null && BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.PasswordHash);
            if (isAuthenticated && user!.IsActive)
            {
                var token = _tokenService.GenerateToken(user!);
                return new LoginResponseDto { IsAuthenticated = true, Token = token, Message = "Inicio de sesion exitoso" };
            }
            else
            {
                return new LoginResponseDto { IsAuthenticated = false, Token = string.Empty, Message = "Credenciales incorrectas" };
            }
        }

        public async Task<UserResponseDto> Register(CreateUserDto createUser)
        {
            var user = new User
            {
                Name = createUser.Name,
                Email = createUser.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(createUser.Password),
                PhoneNumber = createUser.PhoneNumber,
                Role = createUser.Role
            };

            var createdUser = await _userRepository.CreateAsync(user);
            return _mapper.Map<UserResponseDto>(createdUser);
        }
    }
}
