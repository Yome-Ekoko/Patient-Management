using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.Entities;
using Patient_Management.Domain.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Management.Core.Contract
{
    public interface IPatientRecordService
    {
        Task<RecordResponse> UpdateRecordAsync(string id, RecordRequest request);
        Task<PagedResponse<List<GetRecordResponse>>> GetRecordForPatientAsync(LogQueryParameters queryParameters, string patientId);
        Task<GetRecordResponse> GetRecordByIdAsync(string recordId);
        Task<RecordResponse> CreateRecordAsync(RecordRequest request);
    }
}
