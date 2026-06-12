using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.Infection;
using DiplomaTest1.Core.DTOs.User;
using DiplomaTest1.Core.Interfaces;
using DiplomaTest1.Core.Models;
using DiplomaTest1.DAL.Interfaces;

namespace DiplomaTest1.BLL
{
    public class InfectionService : IInfectionService
    {
        private readonly IInfectionRepository _infectionRepository;

        public InfectionService(IInfectionRepository infectionRepository)
        {
            _infectionRepository = infectionRepository;
        }

        public async Task<List<InfectionDto>> GetAllAsync()
        {
            var infections = await _infectionRepository.GetAllInfectionsAsync();
            return infections.Select(ToSummaryDto).ToList();
        }

        private static InfectionDto ToSummaryDto(Infection infec) => new()
        {
            Id = infec.Id,
            InfectionName = infec.InfectionName
        };
    }
}
