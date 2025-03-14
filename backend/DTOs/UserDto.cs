﻿namespace backend.DTOs
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
    }

    public class CreateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } 
    }
    public class UpdateUserDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } 
    }
}
