using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiplomaTest1.Core.DTOs.Record
{
    public class CreateRecordDto
    {
        [Required(ErrorMessage = "Введите данные пациента")] // надо переделать чтобы при новой записи врач или админ вводил не ID пациента, а внутри окошка автоматически был ID пациента и его ФИО, дата рождения
        public int UserId { get; set; }

        [Required(ErrorMessage = "Введите вакцину")] // так же как с пациентом, вводить не ID, а название выбирать из списка
        public int VaccineId { get; set; }

        public DateOnly Date {  get; set; }
        public bool Reaction { get; set; }
    }
}
