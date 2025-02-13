using Patient_Management.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Patient_Management.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? Gender { get; set; }
        public string ContactAddress { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool IsLoggedIn { get; set; }
        public int FailedLoginAttempts { get; set; }
        public UserRole UserRole { get; set; } 
        public DateTime LastLoginTime { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string? PatientId { get; set; }
        public Patient? Patient { get; set; }
    }
}