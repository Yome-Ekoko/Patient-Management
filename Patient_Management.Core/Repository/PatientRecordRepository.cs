using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Patient_Management.Core.Contract.Repository;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.Exceptions;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.Entities;
using Patient_Management.Persistence;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Patient_Management.Core.Repository.PatientRecordRepository;

namespace Patient_Management.Core.Repository
{
 
        public class PatientRecordRepository : IPatientRecordRepository
        {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PatientRecordRepository(IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<PatientRecord> CreateRecordAsync(PatientRecord record)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                await dbContext.PatientRecords.AddAsync(record);
                await dbContext.SaveChangesAsync();
                return record;
            }
        }
        public async Task<PatientRecord> GetPatientRecordByIdAsync(string id, CancellationToken cancellationToken = default)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                    var patientRecord = await dbContext.PatientRecords
                        .Include(r => r.Patient)
                        .ThenInclude(p => p.User)
                        .Include(r => r.Appointments)
                    .FirstOrDefaultAsync(r => r.Id == id && r.Patient.User.IsActive);

                    if (patientRecord == null)
                    {
                        throw new ApiException("Patient Record not found");
                    }

                    return patientRecord;
                }
            }

        public async Task<PagedList<PatientRecord>> GetPatientRecordsAsync(GetPagedTxnsByDateReq request, string patientId, CancellationToken cancellationToken = default)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                IQueryable<PatientRecord> query = dbContext.PatientRecords
                    .Include(r => r.Patient)
                     .ThenInclude(p => p.User)
                     .Include(r => r.Appointments)
                    .Where(r => r.PatientId == patientId && r.Patient.User.IsActive)
                    .OrderByDescending(r => r.CreatedAt);
                
                if (!string.IsNullOrEmpty(request.Query))
                {
                    query = query.Where(a => a.Id.Contains(request.Query));
                }

                int totalRecords = await query.CountAsync(cancellationToken);

                List<PatientRecord> response = await query
                    .OrderByDescending(a => a.CreatedAt)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                return new PagedList<PatientRecord>(response, totalRecords);
            }
        }


    public async Task<PatientRecord> UpdateRecordAsync(PatientRecord record)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            var existingRecord = await dbContext.PatientRecords
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)
                .FirstOrDefaultAsync(a => a.Id == record.Id);

            if (existingRecord == null)
            {
                throw new ApiException("Patient record not found");
            }

            if (!existingRecord.Patient.User.IsActive)
            {
                throw new ApiException("Cannot update patient for a deleted user.");
            }

            var result = dbContext.PatientRecords.Update(record);
            await dbContext.SaveChangesAsync();

            return result.Entity;
        }
    }

}
}
