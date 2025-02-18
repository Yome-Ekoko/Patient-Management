using AutoMapper;
using Patient_Management.Core.Contract;
using Patient_Management.Core.Contract.Repository;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Core.Exceptions;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.Entities;
using Patient_Management.Domain.Enum;
using Patient_Management.Domain.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Patient_Management.Core.Implementation
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IMapper _mapper;


        public AppointmentService(IAppointmentRepository appointmentRepository, IMapper mapper)
        {
            _appointmentRepository = appointmentRepository;
            _mapper = mapper;
        }

        public async Task<AppointmentResponse> BookAppointmentAsync(AppointmentRequest request)
        {
            var appointment = new Appointment
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                PatientRecordId = request.PatientRecordId,
                AppointmentDate = request.AppointmentDate,
                AppointmentTime = request.AppointmentTime,
                Notes = request.Notes,
            };

            var createdAppointment= await _appointmentRepository.CreateAppointmentAsync(appointment);
            return _mapper.Map<AppointmentResponse>(createdAppointment);

        }

        public async Task<GetAppointmentResponse> GetAppointmentAsync(string id)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);

            var response = _mapper.Map<GetAppointmentResponse>(appointment);

            response.PatientName = appointment.Patient != null
                ? appointment.Patient.User.FirstName + " " + appointment.Patient.User.LastName
                : string.Empty;

            response.DoctorName = appointment.Doctor != null
                ? appointment.Doctor.FirstName + " " + appointment.Doctor.LastName
                : string.Empty;

            return response;
        }


        public async Task<PagedResponse<List<GetAppointmentResponse>>> GetAppointmentsForPatientAsync(LogQueryParameters queryParameters, string patientId)
        {
            DateTime currentDay = DateTime.Now.Date;
            DateTime startDate = string.IsNullOrEmpty(queryParameters.StartDate) ? new DateTime(currentDay.Year, 1, 1) : DateTime.Parse(queryParameters.StartDate, null);
            DateTime endDate = string.IsNullOrEmpty(queryParameters.EndDate) ? DateTime.Now.Date : DateTime.Parse(queryParameters.EndDate, null);

            GetPagedTxnsByDateReq getLogDto = new()
            {
                StartDate = startDate,
                EndDate = endDate.AddDays(1),
                PageNumber = queryParameters.PageNumber,
                PageSize = queryParameters.PageSize,
                Query = queryParameters.Query ?? "",
                Status = queryParameters.Status,
            };


            
            var appointments= await _appointmentRepository.GetAppointmentsForPatientAsync(getLogDto,patientId);
            if (appointments.Items.Count <= 0)
            {
                throw new ApiException($"No Appointments found.");
            }
            var response = _mapper.Map<List<GetAppointmentResponse>>(appointments.Items);

            return new PagedResponse<List<GetAppointmentResponse>>(response, queryParameters.PageNumber, queryParameters.PageSize, appointments.TotalRecords, "Successfully retrieved appointments.");



        }

        public async Task<AppointmentResponse> UpdateAppointmentAsync(string id, AppointmentRequest request)
        {
            var existingAppointment = await _appointmentRepository.GetAppointmentByIdAsync(id) ?? throw new ApiException("Appointment Not Found");

            existingAppointment.DoctorId = string.IsNullOrEmpty(request.DoctorId) ? existingAppointment.DoctorId : request.DoctorId;
            existingAppointment.AppointmentDate = request.AppointmentDate == default ? existingAppointment.AppointmentDate : request.AppointmentDate;
            existingAppointment.AppointmentTime = string.IsNullOrEmpty(request.AppointmentTime) ? existingAppointment.AppointmentTime : request.AppointmentTime;
            existingAppointment.Notes = string.IsNullOrEmpty(request.Notes) ? existingAppointment.Notes : request.Notes;



            var updateAppointment =  await _appointmentRepository.UpdateAppointmentAsync(existingAppointment);
            return _mapper.Map<AppointmentResponse>(updateAppointment);
        }

        public async Task<bool> CompleteAppointmentAsync(string id)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
            if (appointment == null) return false;

            appointment.Status = AppointmentStatus.Completed;
            appointment.LastAppointmentDate = DateTime.UtcNow;

            await _appointmentRepository.UpdateAppointmentAsync(appointment);
            return true;
        }

        public async Task<bool> CancelAppointmentAsync(string id)
        {
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(id);
            if (appointment == null) return false;

            appointment.Status = AppointmentStatus.Cancelled;
            await _appointmentRepository.UpdateAppointmentAsync(appointment);
            return true;
        }
    }
}
