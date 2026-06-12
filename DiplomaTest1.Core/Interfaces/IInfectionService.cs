using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.Infection;
using DiplomaTest1.Core.DTOs.User;

namespace DiplomaTest1.Core.Interfaces
{
    public interface IInfectionService
    {
        Task<List<InfectionDto>> GetAllAsync();
    }
}
