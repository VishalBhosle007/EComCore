using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EComCore.Application.DTOs
{
    public class ApplicationUserDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }
        public string[] Roles { get; set; }
    }
}
