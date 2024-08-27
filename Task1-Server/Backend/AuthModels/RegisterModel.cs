namespace Backend.AuthModels
{
    public class RegisterModel
    {
        public string Name { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Role { get; set; }
        public int Age { get; set; }
        public string Race { get; set; }
        public string Gender { get; set; }
    }
}
