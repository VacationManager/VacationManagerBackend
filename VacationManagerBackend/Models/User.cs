using System.Collections.Generic;

namespace VacationManagerBackend.Models
{
    public class User
    {
        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public bool IsManager { get; set; }
        public bool IsAdmin { get; set; }
        public double DaysLeft { get; set; }
        public int VacationDayCount { get; set; }
        public List<VacationSlot> ConfirmedVacationSlots { get; set; }

        public User Copy()
        {
            var cpy = (User)MemberwiseClone();
            return cpy;
        }

        public User GetSecureUser()
        {
            var cpy = Copy();
            cpy.Password = null;

            return cpy;
        }
    }
}
