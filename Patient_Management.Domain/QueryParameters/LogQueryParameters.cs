using Patient_Management.Domain.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Management.Domain.QueryParameters
{
    public class LogQueryParameters : UrlQueryParameters
    {
        public string? Query { get; set; }
        [DataType(DataType.Date)]
        public string? StartDate { get; set; }
        [DataType(DataType.Date)]
        public string? EndDate { get; set; }
        public AppointmentStatus? Status { get; set; }

    }
}
