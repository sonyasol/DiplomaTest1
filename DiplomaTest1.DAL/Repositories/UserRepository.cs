using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DiplomaTest1.Core.Models;
using DiplomaTest1.DAL.Interfaces;

namespace DiplomaTest1.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var result = await _context.Users
                .Include(x => x.Role)
                .OrderBy(x => x.LastName)
                .ToListAsync();
            return result;
        } 

        public async Task<User?> GetUserByIdAsync(int id)
        {
            var result = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Id == id);
            return result;
        }

        public async Task<User?> GetUserBySnilsAsync(string snils)
        {
            var result = await _context.Users
                .Include(x => x.Role)
                .FirstOrDefaultAsync(x => x.Snils == snils);
            return result;
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}
