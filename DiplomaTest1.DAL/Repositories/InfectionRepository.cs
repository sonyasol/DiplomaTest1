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
    public class InfectionRepository : IInfectionRepository
    {
        private readonly AppDbContext _context;

        public InfectionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Infection>> GetAllInfectionsAsync()
        {
            var result = await _context.Infections
                .OrderBy(x => x.InfectionName)
                .ToListAsync();
            return result;
        }
    }
}
