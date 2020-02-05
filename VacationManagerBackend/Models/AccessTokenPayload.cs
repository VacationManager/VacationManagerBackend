using System;

namespace VacationManagerBackend.Models
{
    public class AccessTokenPayload
    {
        public int UserId { get; set; }
        public int DepartmentId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsManager { get; set; }
        public bool IsAdmin { get; set; }

        public AccessTokenPayload(User user)
        {
            UserId = user.Id;
            DepartmentId = user.DepartmentId;
            LastName = user.LastName;
            FirstName = user.FirstName;
            ExpirationDate = DateTime.UtcNow.AddDays(7);
            IsManager = user.IsManager;
            IsAdmin = user.IsAdmin;
        }
    }
}
