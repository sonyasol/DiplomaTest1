using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.DTOs.Vaccine
{
    public class CreateVaccineDto
    {
        [Required(ErrorMessage = "Название обязательно")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Серия обязательна")]
        public string Series {  get; set; } = string.Empty;

        [Required(ErrorMessage = "Срок годности обязателен")]
        public DateOnly ExpiryDate { get; set; }

        [Required(ErrorMessage = "Название инфекции обязательно")]
        public int InfectionId { get; set; }

    }
}
