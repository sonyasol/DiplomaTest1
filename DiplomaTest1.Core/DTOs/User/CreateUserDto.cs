using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.DTOs.User
{
    public class CreateUserDto
    {
        [Required(ErrorMessage = "Фамилия обязательна")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Имя обязательно")]
        public string FirstName { get; set; } = string.Empty;

        public string MiddleName { get; set; } = string.Empty;

        public DateOnly DateOfBirth { get; set; }


        // Проблемы с int:
        // 1. СНИЛС начинается с нуля — int отбросит его: 001-234 станет 1234
        // 2. [MinLength] не работает с int — только со строками
        // 3. Дефисы и пробелы в формате "123-456-789 00" не влезут в int

        [Required(ErrorMessage = "СНИЛС обязателен")]
        [MinLength(11, ErrorMessage = "СНИЛС должен содержать 11 исмволов")] //проверить нужно сделать снилс строкой или инт
        public string Snils { get; set; }

        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Пароль минимум 6 символов")]
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
