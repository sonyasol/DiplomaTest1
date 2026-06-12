using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.User;

namespace DiplomaTest1.Core.Interfaces
{
    public interface IUserService
    {
        Task<List<UserSummaryDto>> GetAllAsync();
        Task<UserDetailDto?> GetByIdAsync(int id);
        Task<UserDetailDto?> GetUserBySnilsAsync(string snils);
        Task<UserDetailDto?> CreateAsync(CreateUserDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
