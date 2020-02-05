using System.Collections.Generic;

namespace VacationManagerBackend.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string DepartmentName { get; set; }
        public List<User> Users { get; set; }
    }
}
