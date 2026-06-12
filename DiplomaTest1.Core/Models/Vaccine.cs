using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.Models
{
    public class Vaccine
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Series { get; set; } = string.Empty;
        public DateOnly ExpiryDate { get; set; }
        public int InfectionId { get; set; }

        public Infection Infection { get; set; } = null!;
        public ICollection<Record> Records { get; set; } = new List<Record>();
    }
}
