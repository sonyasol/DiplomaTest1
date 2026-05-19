using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.DTOs.User
{
    public class UserSummaryDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public DateOnly DateOfBirth { get; set; }
        public int Snils { get; set; }
        public string Phone { get; set; } = string.Empty;
    }
}
