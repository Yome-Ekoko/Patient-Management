using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Patient_Management.Core.Contract
{
    public interface IPatientService
    {
        Task<Response<RegisterPatientResponse>> UpdatePatient(UpdatePatientRequest request);
        Task<Response<GetPatientResponse>> GetPatientById(string Id);
        Task<PagedResponse<List<GetAllPatientResponse>>> GetPatients(LogQueryParameters queryParameters, CancellationToken cancellationToken);
        Task<Response<RegisterPatientResponse>> RegisterPatient(RegisterPatientRequest request);
    }
}
