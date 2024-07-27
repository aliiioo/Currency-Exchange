namespace Application.Dtos.RegistrationDto
{
    public class UserDto
    {
        public string Email { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsAdmin { get; set; }=false;

    }
}
