using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Management.Core.DTO.Request
{
    public class AppointmentRequest
    {
        public string PatientId { get; set; } = string.Empty;
        public string? DoctorId { get; set; }
        public string? PatientRecordId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } = string.Empty;
        public string? Notes { get; set; }
    }
}
