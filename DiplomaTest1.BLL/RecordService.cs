using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.Record;
using DiplomaTest1.Core.Interfaces;
using DiplomaTest1.Core.Models;
using DiplomaTest1.DAL.Interfaces;


namespace DiplomaTest1.BLL
{
    public class RecordService : IRecordService
    {
        private readonly IRecordRepository _recordRepository;
        private readonly IUserRepository _userRepository;
        private IVaccineRepository _vaccineRepository;

        public RecordService(
            IRecordRepository recordRepository,
            IUserRepository userRepository,
            IVaccineRepository vaccineRepository)
        {
            _recordRepository = recordRepository;
            _userRepository = userRepository;
            _vaccineRepository = vaccineRepository;
        }

        public async Task<IEnumerable<RecordSummaryDto>> GetRecordByUserIdAsync(int userId)
        {
            var result = await _recordRepository.GetRecordByUserIdAsync(userId);
            return result.Select(ToRecordDto);
        }

        public async Task<IEnumerable<RecordSummaryDto>> GetRecordByVaccineIdAsync(int vaccineId)
        {
            var result = await _recordRepository.GetRecordByVaccineIdAsync(vaccineId);
            return result.Select(ToRecordDto);
        }

        public async Task<RecordSummaryDto> AddRecordAsync(CreateRecordDto dto)
        {
            var user = await _userRepository.GetUserByIdAsync(dto.UserId);
            if (user is null)
                throw new ArgumentException("Пациент не найден");

            var vaccine = await _vaccineRepository.GetVaccineByIdAsync(dto.VaccineId);
            if (vaccine is null)
                throw new ArgumentException("Вакцина не найдена");

            if (vaccine.ExpiryDate <= DateOnly.FromDateTime(DateTime.Today))
                throw new InvalidOperationException("Срок годности вакцины истек");

            var record = new Record
            {
                UserId = dto.UserId,
                VaccineId = dto.VaccineId,
                Date = dto.Date == default ? DateOnly.FromDateTime(DateTime.Today) : dto.Date,
                Reaction = dto.Reaction
            };

            var created = await _recordRepository.AddRecordAsync(record);

            var full = await _recordRepository.GetRecordByIdAsync(created.Id);
            return ToRecordDto(full!);
        }

        public async Task<bool> DeleteRecordAsync(int id)
        {
            return await _recordRepository.DeleteRecordAsync(id);
        }

        private static RecordSummaryDto ToRecordDto(Record r) => new()
        {
            Id = r.Id,
            Date = r.Date,
            Reaction = r.Reaction,
            VaccineName = r.Vaccine?.Name ?? string.Empty,
            Series = r.Vaccine?.Series ?? string.Empty,
            InfectionName = r.Vaccine?.Infection?.InfectionName ?? string.Empty
        };


    }
}
