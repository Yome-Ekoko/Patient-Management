using System.ComponentModel.DataAnnotations;

namespace Patient_Management.Core.DTO.Request
{
    public class DeleteUserRequest
    {
        [Required]
        [DataType(DataType.Text)]
        public string Email { get; set; }
    }
}
