using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiplomaTest1.Core.DTOs.Record;

namespace DiplomaTest1.Core.DTOs.User
{
    public class UserDetailDto
    {
        public int Id { get; set; }
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string MiddleName {  get; set; } = string.Empty;
        public DateOnly DateOfBirth {  get; set; }
        public string Snils { get; set; }
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role {  get; set; } 

        public List<RecordSummaryDto> Records { get; set; } = new();
    }
}
