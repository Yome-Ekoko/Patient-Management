using Patient_Management.Domain.Common;
using Patient_Management.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Management.Domain.Entities
{

    public class PatientRecord : EntityBase
    {
        public PatientRecord()
        {
            SetNewId();
        }

        public string PatientId { get; set; } = string.Empty;
        public string Diagnosis { get; set; } = string.Empty;
        public string UnderlyingIllness { get; set; } = string.Empty;
        public string MedicalHistory { get; set; } = string.Empty;
        public string Treatment { get; set; } = string.Empty;

        public Patient? Patient { get; set; }
        public List<Appointment> Appointments { get; set; } = new();

        public override void SetNewId()
        {
            Id = $"REC{CoreHelpers.CreateUlid(DateTimeOffset.Now)}";
        }
    }
}
