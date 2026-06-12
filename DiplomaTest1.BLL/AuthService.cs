using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.Auth;
using DiplomaTest1.Core.Interfaces;
using DiplomaTest1.DAL.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DiplomaTest1.BLL
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthService(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> LoginAcync(LoginDto dto)
        {
            var user = await _userRepository.GetUserBySnilsAsync(dto.Snils);
            if (user is null) return null;
            

            if (user.Password != dto.Password) return null;

            var token = GenerateToken(user.Id, user.Snils, user.Role?.Name ?? string.Empty);

            return new AuthResponseDto
            {
                Token = token,
                FullName = $"{user.LastName} {user.FirstName} {user.MiddleName}".Trim(),
                Role = user.Role?.Name ?? string.Empty,
                UserId = user.Id
            };
        }

        private string GenerateToken(int userId, string snils, string role)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.NameIdentifier, snils.ToString()),
                new Claim(ClaimTypes.Role, role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(
                    double.Parse(_configuration["Jwt:ExpiresInHours"]!)),
                signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
