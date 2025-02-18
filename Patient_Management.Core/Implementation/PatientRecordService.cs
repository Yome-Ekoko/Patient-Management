using AutoMapper;
using Azure;
using Patient_Management.Core.Contract;
using Patient_Management.Core.Contract.Repository;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Core.Exceptions;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.Entities;
using Patient_Management.Domain.QueryParameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Patient_Management.Core.Implementation.PatientRecordService;

namespace Patient_Management.Core.Implementation
{
   
        public class PatientRecordService : IPatientRecordService
        {
            private readonly IPatientRecordRepository _recordRepository;
            private readonly IPatientRepository _patientRepository;
            private readonly IMapper _mapper;


            public PatientRecordService(IPatientRecordRepository recordRepository, IPatientRepository patientRepository, IMapper mapper)
            {
                _recordRepository = recordRepository;
                _patientRepository = patientRepository;
                _mapper = mapper;
            }

          
        public async Task<RecordResponse> CreateRecordAsync(RecordRequest request)
        {
            var patient = await _patientRepository.GetById(request.PatientId);
            if (patient == null || !patient.User.IsActive)
            {
                throw new ApiException("Patient not found or has been deactivated.");
            }

            var record = new PatientRecord
            {
                PatientId = request.PatientId,
                UnderlyingIllness = request.UnderlyingIllness,
                Treatment = request.Treatment,
                Diagnosis = request.Diagnosis,
                MedicalHistory = request.MedicalHistory,
               
            };

            var createdRecord = await _recordRepository.CreateRecordAsync(record);
            return _mapper.Map<RecordResponse>(createdRecord);

        }


        public async Task<GetRecordResponse> GetRecordByIdAsync(string recordId)
        {
            var record = await _recordRepository.GetPatientRecordByIdAsync(recordId);
            if (record == null)
            {
                throw new ApiException("Record not found or patient is inactive.");
            }

            var response = _mapper.Map<GetRecordResponse>(record);

            response.Appointments = record.Appointments?.Select(a => new AppointmentResponse
            {
                Id = a.Id,
                AppointmentDate = a.AppointmentDate,
                AppointmentTime = a.AppointmentTime,
                Notes = a.Notes,
                DoctorId = a.DoctorId,
                Status = a.Status
            }).ToList();

            return response;
        }


        public async Task<PagedResponse<List<GetRecordResponse>>> GetRecordForPatientAsync(LogQueryParameters queryParameters, string patientId)
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



            var records = await _recordRepository.GetPatientRecordsAsync(getLogDto, patientId);
            ;
            if (records.Items.Count <= 0)
            {
                throw new ApiException($"No Appointments found.");
            }
            var response = _mapper.Map<List<GetRecordResponse>>(records.Items);

            return new PagedResponse<List<GetRecordResponse>>(response, queryParameters.PageNumber, queryParameters.PageSize, records.TotalRecords, "Successfully retrieved Patient record.");

        }

           public async Task<RecordResponse> UpdateRecordAsync(string id, RecordRequest request)
        {
            var existingRecord = await _recordRepository.GetPatientRecordByIdAsync(id) ?? throw new ApiException("Appointment Not Found");

            existingRecord.UnderlyingIllness = string.IsNullOrEmpty(request.UnderlyingIllness) ? existingRecord.UnderlyingIllness : request.UnderlyingIllness;
            existingRecord.MedicalHistory = request.MedicalHistory == default ? existingRecord.MedicalHistory : request.MedicalHistory;
            existingRecord.Treatment = string.IsNullOrEmpty(request.Treatment) ? existingRecord.Treatment : request.Treatment;
            existingRecord.Diagnosis = string.IsNullOrEmpty(request.Diagnosis) ? existingRecord.Diagnosis : request.Diagnosis;
            

            var updateAppointment = await _recordRepository.UpdateRecordAsync(existingRecord);
            return _mapper.Map<RecordResponse>(updateAppointment);
        }
    }

 }

