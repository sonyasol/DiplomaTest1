using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.Models
{
    public class Record
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateOnly Date {  get; set; }
        public int VaccineId { get; set; }
        public bool Reaction {  get; set; }

        public User User { get; set; } = null!;
        public Vaccine Vaccine { get; set; } = null!;
    }
}
