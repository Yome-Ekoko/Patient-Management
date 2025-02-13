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
    public interface IPatientRepository
    {
        Task<Patient> AddPatient(Patient patient);
        Task<PagedList<Patient>> GetPagedByDateRange(GetPagedTxnsByDateReq request, CancellationToken cancellationToken = default);
        Task<Patient> GetById(string id);
        Task<Patient> GetPatient(string userId);
        Task<Patient> Update(Patient patient);
    }
}
