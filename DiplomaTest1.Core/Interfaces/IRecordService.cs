using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.Record;

namespace DiplomaTest1.Core.Interfaces
{
    public interface IRecordService
    {
        Task<IEnumerable<RecordSummaryDto>> GetRecordByUserIdAsync(int userId);
        Task<IEnumerable<RecordSummaryDto>> GetRecordByVaccineIdAsync(int vaccineId);
        Task<RecordSummaryDto> AddRecordAsync(CreateRecordDto record);
        Task<bool> DeleteRecordAsync(int id);
    }
}
