using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.DTOs.Vaccine
{
    public class VaccineSummaryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;
        public DateOnly ExpiryDate { get; set; }
        public bool IsValid {  get; set; } //чтобы потом проверять не вышел ли срок годности
        public string InfectionName { get; set; } = string.Empty;
    }
}
