using System.ComponentModel.DataAnnotations;

namespace VacationManagerBackend.Models
{
    public class SetupData
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string MailAddress { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string DepartmentName { get; set; }
        public int? DefaultDayCount { get; set; }
    }
}
