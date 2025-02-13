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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Patient_Management.Core.Repository
{
    public class PatientRepository : IPatientRepository
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public PatientRepository(IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<Patient> AddPatient(Patient patient)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                if (patient == null)
                {
                    throw new ApiException("No Patient added");
                }

                var result = await dbContext.Patients.AddAsync(patient);
                await dbContext.SaveChangesAsync();
                return result.Entity;
            }

        }
   
        public async Task<Patient> Update(Patient patient)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                var existingPatient = await dbContext.Patients
                    .Include(p => p.User)
                    .FirstOrDefaultAsync(p => p.Id == patient.Id);

                if (existingPatient == null)
                {
                    throw new ApiException("Patient not found");
                }

                if (existingPatient.User != null && !existingPatient.User.IsActive)
                {
                    throw new ApiException("Cannot update a soft-deleted patient.");
                }

                var result = dbContext.Patients.Update(existingPatient);
                await dbContext.SaveChangesAsync();

                return result.Entity;
            }
        }

        public async Task<Patient> GetPatient(string userId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var patient = await dbContext.Patients
                    .Include(p => p.User)
                    .Include(p => p.Records)  
                    .ThenInclude(r => r.Appointments)
                    .Where(p => p.User != null && p.User.IsActive)
                    .FirstOrDefaultAsync(x => x.UserId == userId);

                if (patient == null)
                {
                    throw new ApiException("Patient not found");
                }

                return patient;
            }
        }
        public async Task<Patient> GetById(string id)
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
                var res = await dbContext.Patients
                    .Include(p => p.User)
                    .Include(p => p.Records)  
                    .ThenInclude(r => r.Appointments)
                    .Where(p => p.User != null && p.User.IsActive)
                    .FirstOrDefaultAsync(u => u.Id == id) ?? throw new ApiException("Patient not found");

                return res;

            }
        }
        public async Task<PagedList<Patient>> GetPagedByDateRange(GetPagedTxnsByDateReq request, CancellationToken cancellationToken = default)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

                IQueryable<Patient> query = dbContext.Patients
                .Where(x => x.CreatedAt >= request.StartDate)
                .Where(x => x.CreatedAt <= request.EndDate)
                .Include(p => p.User)
                .Where(p => p.User != null && p.User.IsActive);


                if (!string.IsNullOrEmpty(request.Query))
                {
                    var normalizedQuery = request.Query.ToUpper();

                    query = query.Where(x =>
                        x.Id.Contains(request.Query) ||
                        x.UserId.Contains(request.Query));
                }

                List<Patient> response = await query
                    .OrderByDescending(x => x.CreatedAt)
                    .Skip((request.PageNumber - 1) * request.PageSize)
                    .Take(request.PageSize)
                    .ToListAsync(cancellationToken);

                int totalRecords = await query.CountAsync(cancellationToken);

                return new PagedList<Patient>(response, totalRecords);
            }

        }
    
    }
}
