using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.Models;
using DiplomaTest1.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DiplomaTest1.DAL.Repositories
{
    public class RecordRepository : IRecordRepository
    {
        private readonly AppDbContext _context;

        public RecordRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Record>> GetRecordByUserIdAsync(int userId)
        {
            return await _context.Records
                .Include(r => r.Vaccine)
                .ThenInclude(r => r.Infection)
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        public async Task<IEnumerable<Record>> GetRecordByVaccineIdAsync(int vaccineId)
        {
            return await _context.Records
                .Include(r => r.Vaccine)
                .ThenInclude(r => r.Infection)
                .Where(r => r.VaccineId == vaccineId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }

        public async Task<Record?> GetRecordByIdAsync(int id)
        {
            return await _context.Records
                .Include(r => r.Vaccine)
                .ThenInclude(r => r.Infection)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Record> AddRecordAsync(Record record)
        {
            await _context.Records.AddAsync(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<bool> DeleteRecordAsync(int id)
        {
            var record = await _context.Records.FindAsync(id);
            if (record is null) return false;

            _context.Records.Remove(record);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
