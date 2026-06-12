using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.Record;
using DiplomaTest1.Core.DTOs.User;
using DiplomaTest1.Core.Interfaces;
using DiplomaTest1.Core.Models;
using DiplomaTest1.DAL.Interfaces;

namespace DiplomaTest1.BLL
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserSummaryDto>> GetAllAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            return users.Select(ToSummaryDto).ToList();
        }

        public async Task<UserDetailDto?> GetByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user is null) return null;
            return ToDetailDto(user);
        }

        public async Task<UserDetailDto?> GetUserBySnilsAsync(string snils)
        {
            var user = await _userRepository.GetUserBySnilsAsync(snils);
            if (user is null) return null;
            return ToDetailDto(user);
        }

        public async Task<UserDetailDto?> CreateAsync(CreateUserDto dto)
        {
            var existing = await _userRepository.GetUserBySnilsAsync(dto.Snils);
            if (existing is not null) 
                throw new InvalidOperationException("Пользователь с таким СНИЛС уже существует");

            if (dto.DateOfBirth > DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("Дата рождения не может быть в будущем");

            var user = new User
            {
                LastName = dto.LastName,
                FirstName = dto.FirstName,
                MiddleName = dto.MiddleName,
                DateOfBirth = dto.DateOfBirth,
                Snils = dto.Snils,
                Phone = dto.Phone,
                Email = dto.Email,
                Password = dto.Password,
                RoleId = dto.RoleId
            };

            await _userRepository.AddAsync(user);
            return await GetUserBySnilsAsync(user.Snils);
        }

        public async Task<bool> DeleteAsync(int id)   /// добавить удаление
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user is null) return false;
            return true;
        }

        private static UserSummaryDto ToSummaryDto(User user) => new()
        {
            Id = user.Id,
            FullName = $"{user.LastName} {user.FirstName} {user.MiddleName}".Trim(),
            DateOfBirth = user.DateOfBirth,
            Snils = user.Snils
        };

        private static UserDetailDto ToDetailDto(User user) => new()
        {
            Id = user.Id,
            LastName = user.LastName,
            FirstName = user.FirstName,
            MiddleName = user.MiddleName,
            DateOfBirth = user.DateOfBirth,
            Snils = user.Snils,
            Phone = user.Phone,
            Email = user.Email,
            Role = user.Role?.Name ?? string.Empty,
            Records = user.Records?.Select(r => new RecordSummaryDto
            {
                Id = r.Id,
                Date = r.Date,
                Reaction = r.Reaction,
                VaccineName = r.Vaccine?.Name ?? string.Empty,
                Series = r.Vaccine?.Series ?? string.Empty,
                InfectionName = r.Vaccine?.Infection?.InfectionName ?? string.Empty
            }).ToList() ?? new List<RecordSummaryDto>()
        };
    }
}
