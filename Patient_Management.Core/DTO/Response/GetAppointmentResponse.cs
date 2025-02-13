using Patient_Management.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Management.Core.DTO.Response
{
    public class GetAppointmentResponse
    {
        public string Id { get; set; }
        public string PatientId { get; set; }
        public string DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? Notes { get; set; }
        public DateTime? LastAppointmentDate { get; set; }

        // Additional properties for patient and doctor names
        public string PatientName { get; set; } = string.Empty;
        public string DoctorName { get; set; } = string.Empty;
    }
}
