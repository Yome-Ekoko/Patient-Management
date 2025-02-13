using Patient_Management.Domain.Common;
using Patient_Management.Domain.Entities.Base;
using Patient_Management.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Management.Domain.Entities
{
    
    public class Appointment : EntityBase
    {
        public Appointment()
        {
            SetNewId();
        }

        public string PatientId { get; set; } = string.Empty;
        public string? DoctorId { get; set; } 
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } = string.Empty;  
        public AppointmentStatus Status { get; set; } = AppointmentStatus.Scheduled;
        public string? Notes { get; set; }
        public DateTime? LastAppointmentDate { get; set; } 

        public Patient? Patient { get; set; }
        public User? Doctor { get; set; }

        public string PatientRecordId { get; set; } = string.Empty; 
        public PatientRecord PatientRecord { get; set; }  

       
        public override void SetNewId()
        {
            Id = $"APT{CoreHelpers.CreateUlid(DateTimeOffset.Now)}";
        }
    }

}
