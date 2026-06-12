using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.Vaccine;

namespace DiplomaTest1.Core.Interfaces
{
    public interface IVaccineService
    {
        Task<List<VaccineSummaryDto>> GetAllVaccinesAsync();
        Task<VaccineSummaryDto> GetVaccineByIdAsync(int id);
        Task<VaccineSummaryDto> CreateVaccineAsync(CreateVaccineDto dto);
    }
}
