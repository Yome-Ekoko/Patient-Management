using Patient_Management.Domain.Enum;
using System.ComponentModel.DataAnnotations;

namespace Patient_Management.Core.DTO.Request
{
    public class EditUserRequest
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [DataType(DataType.Text)]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        public string LastName { get; set; }
        [DataType(DataType.Text)]
        public string Gender { get; set; }

        [DataType(DataType.Text)]
        public UserRole? Role { get; set; }

        [DataType(DataType.Text)]
        public string ParticipantId { get; set; }
    }
}
