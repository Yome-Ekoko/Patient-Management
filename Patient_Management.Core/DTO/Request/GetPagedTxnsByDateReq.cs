using Patient_Management.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Patient_Management.Core.DTO.Request
{
    public class GetPagedTxnsByDateReq
    {
        public string? Query { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public AppointmentStatus? Status { get; set; }

    }
}
