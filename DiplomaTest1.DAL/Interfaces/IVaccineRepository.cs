using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.Models;

namespace DiplomaTest1.DAL.Interfaces
{
    public interface IVaccineRepository
    {
        Task<List<Vaccine>> GetAllVaccinesAsync();
        Task<Vaccine?> GetVaccineByIdAsync(int id);
        Task AddVaccineAsync(Vaccine vaccine);
    }
}
