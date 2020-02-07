namespace VacationManagerBackend.Models.Input
{
    public class LoginInputData
    {
        public string EMailAddress { get; set; }
        public string Password { get; set; }

        public bool IsValid()
        {
            if (string.IsNullOrEmpty(EMailAddress) || string.IsNullOrWhiteSpace(EMailAddress)) return false;
            if (string.IsNullOrEmpty(Password) || string.IsNullOrWhiteSpace(Password)) return false;

            return true;
        }
    }
}
