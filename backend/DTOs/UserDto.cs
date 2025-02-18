namespace backend.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        // Do not expose Password in DTOs for security reasons
    }

    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class UpdateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
