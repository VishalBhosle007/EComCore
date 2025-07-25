using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Application.DTOs
{
    public class RegisterRequestDto
    {
        [Required]
        public string FirstName { get; set; }

        public string? LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        public string Password { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public string[] Roles { get; set; }
    }
}
