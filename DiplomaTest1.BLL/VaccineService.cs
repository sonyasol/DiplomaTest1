using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.Vaccine;
using DiplomaTest1.Core.Interfaces;
using DiplomaTest1.Core.Models;
using DiplomaTest1.DAL.Interfaces;

namespace DiplomaTest1.BLL
{
    public class VaccineService : IVaccineService
    {
        private readonly IVaccineRepository _vaccineRepository;
        public VaccineService(IVaccineRepository vaccineRepository)
        {
            _vaccineRepository = vaccineRepository;
        }
        public async Task<List<VaccineSummaryDto>> GetAllVaccinesAsync()
        {
            var vaccines = await _vaccineRepository.GetAllVaccinesAsync();
            return vaccines.Select(ToVaccineSummaryDto).ToList();
        }

        public async Task<VaccineSummaryDto> GetVaccineByIdAsync(int id)
        {
            var vaccine = await _vaccineRepository.GetVaccineByIdAsync(id);
            if (vaccine == null) return null;
            return ToVaccineSummaryDto(vaccine);
        }

        public async Task<VaccineSummaryDto> CreateVaccineAsync(CreateVaccineDto dto)
        {
            var vaccine = new Vaccine
            {
                Name = dto.Name,
                Series = dto.Series,
                ExpiryDate = dto.ExpiryDate,
                InfectionId = dto.InfectionId
            };

            await _vaccineRepository.AddVaccineAsync(vaccine);
            
            var created = await _vaccineRepository.GetVaccineByIdAsync(vaccine.Id);
            return ToVaccineSummaryDto(created);
        }

        private static VaccineSummaryDto ToVaccineSummaryDto(Vaccine vaccine) => new()
        {
            Id = vaccine.Id,
            Name = vaccine.Name,
            Series = vaccine.Series,
            ExpiryDate = vaccine.ExpiryDate,
            InfectionName = vaccine.Infection?.InfectionName ?? string.Empty
        };
    }
}
