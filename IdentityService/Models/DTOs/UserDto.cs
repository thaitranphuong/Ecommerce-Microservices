using System.Collections.Generic;
using System;

namespace IdentityService.Models.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public DateTime BirthDay { get; set; }
        public bool Gender { get; set; }
        public bool Enabled { get; set; }
        public List<string> Roles { get; set; }
        public bool IsAdmin { get; set; }
    }
}
