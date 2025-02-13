using Patient_Management.Core.DTO.Request;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Patient_Management.Core.Contract.Repository
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAppointmentAsync(Appointment appointment);
        Task<Appointment> GetAppointmentByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<PagedList<Appointment>> GetAppointmentsForPatientAsync(GetPagedTxnsByDateReq request, string patientId, CancellationToken cancellationToken = default);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment);

    }
}
