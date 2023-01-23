namespace projekt_programowanie.DTOs
{
    public class UpdateDataDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? Password { get; set; }
        public bool ChangePasswordChecked { get; set; }
    }
}
