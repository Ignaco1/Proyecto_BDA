using Domain.DTOs.Responses;
using Domain.DTOs.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto> Login(LoginRequestDto loginRequest);

        Task<UserResponseDto> Register(CreateUserDto createUser);
    }
}
