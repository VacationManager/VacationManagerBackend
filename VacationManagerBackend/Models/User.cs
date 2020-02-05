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
    }
}
