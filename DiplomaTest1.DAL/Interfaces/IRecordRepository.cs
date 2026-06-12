using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.Models;

namespace DiplomaTest1.DAL.Interfaces
{
    public interface IRecordRepository
    {
        Task<IEnumerable<Record>> GetRecordByUserIdAsync(int userId);
        Task<IEnumerable<Record>> GetRecordByVaccineIdAsync(int vaccineId);
        Task<Record?> GetRecordByIdAsync(int id);
        Task<Record> AddRecordAsync(Record record);
        Task<bool> DeleteRecordAsync(int id);
    }
}
