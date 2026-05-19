using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.Models
{
    public class User
    {
        public int ID { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName {  get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public int Snils { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password {  get; set; } = string.Empty;
        public int RoleId { get; set; }

        public Role Role { get; set; } = null!;
        public ICollection<Record> Records { get; set; } = new List<Record>();
    }
}
