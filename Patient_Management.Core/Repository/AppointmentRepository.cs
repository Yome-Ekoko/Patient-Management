using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Patient_Management.Core.Contract.Repository;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Core.Exceptions;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.Entities;
using Patient_Management.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

public class AppointmentRepository : IAppointmentRepository
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AppointmentRepository(IServiceScopeFactory serviceScopeFactory, IHttpContextAccessor httpContextAccessor)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Appointment> CreateAppointmentAsync(Appointment appointment)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            if (appointment == null)
            {
                throw new ApiException("Appointment not found ");
            }

            var result = await dbContext.Appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();
            return result.Entity;
        }
    }

    public async Task<Appointment> GetAppointmentByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            var appointment = await dbContext.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User)  
                .Include(a => a.Doctor)
                .FirstOrDefaultAsync(a => a.Id == id && a.Patient.User.IsActive, cancellationToken);

            if (appointment == null)
            {
                throw new ApiException("Appointment not found");
            }

            return appointment;
        }
    }

    public async Task<PagedList<Appointment>> GetAppointmentsForPatientAsync(GetPagedTxnsByDateReq request, string patientId, CancellationToken cancellationToken = default)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            IQueryable<Appointment> query = dbContext.Appointments
                .Include(a => a.Patient)
                .ThenInclude(p => p.User) 
                .Where(a => a.PatientId == patientId && a.Patient.User.IsActive)
                .Where(a => a.CreatedAt >= request.StartDate && a.CreatedAt <= request.EndDate);

            if (!string.IsNullOrEmpty(request.Query))
            {
                query = query.Where(a => a.Id.Contains(request.Query));
            }

            if (request.Status.HasValue)
            {
                query = query.Where(a => a.Status == request.Status);
            }

            int totalRecords = await query.CountAsync(cancellationToken);

            List<Appointment> response = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedList<Appointment>(response, totalRecords);
        }
    }

    public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment)
    {
        using (var scope = _serviceScopeFactory.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();

            if (!appointment.Patient.User.IsActive)
            {
                throw new ApiException("Cannot update appointment for a deleted user.");
            }

            var result = dbContext.Appointments.Update(appointment);
            await dbContext.SaveChangesAsync();

            return result.Entity;
        }
    }

}

