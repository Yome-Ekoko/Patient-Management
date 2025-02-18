using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Management.Core.DTO.Response
{
    public class RecordResponse
    {
        public string PatientId { get; set; } 
        public string Id { get; set; } 
        public string Diagnosis { get; set; } 
        public string UnderlyingIllness { get; set; } 
        public string MedicalHistory { get; set; } 
        public string Treatment { get; set; } 
    }
}
