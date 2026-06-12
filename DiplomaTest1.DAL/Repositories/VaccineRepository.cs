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
    public class VaccineRepository : IVaccineRepository
    {
        private readonly AppDbContext _context;
        public VaccineRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Vaccine>> GetAllVaccinesAsync()
        {
            var result = await _context.Vaccines
                .Include(v => v.Infection)
                .OrderBy(v => v.Name)
                .ToListAsync();
            return result;
        }

        public async Task<Vaccine?> GetVaccineByIdAsync(int id)
        {
            var result = await _context.Vaccines
                .Include(v => v.Infection)
                .FirstOrDefaultAsync(v => v.Id == id);
            return result;
        }

        public async Task AddVaccineAsync(Vaccine vaccine)
        {
            await _context.Vaccines.AddAsync(vaccine);
            await _context.SaveChangesAsync();
        }
    }
}
