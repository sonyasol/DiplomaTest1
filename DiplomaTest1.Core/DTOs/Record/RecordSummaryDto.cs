using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.DTOs.Record
{
    public class RecordSummaryDto
    {
        public int Id { get; set; }
        public DateOnly Date { get; set; }
        public bool Reaction { get; set; }

        public string VaccineName { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;

        public string InfectionName {  get; set; } = string.Empty;
    }
}
