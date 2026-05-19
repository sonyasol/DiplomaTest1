using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.DTOs.Infection
{
    public class CreateInfectionDto
    {
        [Required(ErrorMessage = "Название инфекции обязательно")]
        public string InfectionName { get; set; } = string.Empty; //возможно не понадобится, а может быть будет страница с описанием инфекции
    }
}
