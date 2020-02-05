using System;

namespace VacationManagerBackend.Models
{
    public class AccessTokenPayload
    {
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string LastName { get; set; }
        public string Firstname { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsManager { get; set; }
        public bool IsAdmin { get; set; }
    }
}
