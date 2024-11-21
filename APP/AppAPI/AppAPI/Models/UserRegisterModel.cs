using System;
using System.ComponentModel.DataAnnotations;

namespace TodoAPI.Models
{
    public class UserRegisterModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
    }
}
