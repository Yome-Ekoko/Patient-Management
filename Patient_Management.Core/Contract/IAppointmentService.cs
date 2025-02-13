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
    public interface IAppointmentService
    {
        Task<AppointmentResponse> BookAppointmentAsync(AppointmentRequest request);
        Task<GetAppointmentResponse> GetAppointmentAsync(string id);
        Task<PagedResponse<List<GetAppointmentResponse>>> GetAppointmentsForPatientAsync(LogQueryParameters queryParameters, string patientId);
        Task<bool> CompleteAppointmentAsync(string id);
        Task<bool> CancelAppointmentAsync(string id);
        Task<AppointmentResponse> UpdateAppointmentAsync(string id, AppointmentRequest request);
    }
}
