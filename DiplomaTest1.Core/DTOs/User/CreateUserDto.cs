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

        [Required(ErrorMessage = "СНИЛС обязателен")]
        [MinLength(11, ErrorMessage = "СНИЛС должен содержать 11 исмволов")] //проверить нужно сделать снилс строкой или инт
        public int Snils { get; set; }

        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Пароль обязателен")]
        [MinLength(6, ErrorMessage = "Пароль минимум 6 символов")]
        public string Password { get; set; } = string.Empty;
        public int RoleId { get; set; }
    }
}
