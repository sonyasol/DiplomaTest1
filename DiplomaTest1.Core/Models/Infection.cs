using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.Models
{
    public class Infection
    {
        public int Id { get; set; }
        public string InfectionName { get; set; } = string.Empty;

        public ICollection<Vaccine> Vaccines { get; set; } = new List<Vaccine>();
    }
}
