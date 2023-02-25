﻿using System.ComponentModel.DataAnnotations;

namespace QardlessAPI.Data.Dtos.Employee
{
    public class EmployeeCreateDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public string? ContactNumber { get; set; }

        [Required]
        public int PrivilegeLevel { get; set; }

        [Required]
        public Guid BusinessId { get; set; }
    }
}
