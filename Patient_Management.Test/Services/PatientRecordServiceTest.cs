using Moq;
using AutoMapper;
using Patient_Management.Core.Contract.Repository;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Core.Exceptions;
using Patient_Management.Core.Implementation;
using Patient_Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.QueryParameters;
using Patient_Management.Domain.Enum;

namespace Patient_Management.Test.Services
{
    public class PatientRecordServiceTest
    {
        private readonly Mock<IPatientRecordRepository> _recordRepositoryMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientRecordService _patientRecordService;

        public PatientRecordServiceTest()
        {
            _recordRepositoryMock = new Mock<IPatientRecordRepository>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _mapperMock = new Mock<IMapper>();

            _patientRecordService = new PatientRecordService(
                _recordRepositoryMock.Object,
                _patientRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task CreateRecordAsync_ShouldReturnRecordResponse_WhenRecordIsCreated()
        {
            var request = new RecordRequest
            {
                PatientId = "1",
                UnderlyingIllness = "Illness",
                Treatment = "Treatment",
                Diagnosis = "Diagnosis",
                MedicalHistory = "History"
            };

            var patient = new Patient { Id = "1", User = new User { IsActive = true } };
            var record = new PatientRecord { Id = "1", PatientId = "1" };

            _patientRepositoryMock.Setup(x => x.GetById(request.PatientId)).ReturnsAsync(patient);
            _recordRepositoryMock.Setup(x => x.CreateRecordAsync(It.IsAny<PatientRecord>())).ReturnsAsync(record);
            _mapperMock.Setup(x => x.Map<RecordResponse>(record)).Returns(new RecordResponse { Id = "1" });

            var result = await _patientRecordService.CreateRecordAsync(request);

            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
        }

        [Fact]
        public async Task GetRecordByIdAsync_ShouldReturnGetRecordResponse_WhenRecordExists()
        {
            var recordId = "1";
            var record = new PatientRecord
            {
                Id = recordId,
                Appointments = new List<Appointment>
                {
                    new Appointment { Id = "1", AppointmentDate = DateTime.Now, AppointmentTime = DateTime.Now.TimeOfDay.ToString(), Notes = "Notes", DoctorId = "1", Status = AppointmentStatus.Scheduled }
                }
            };

            _recordRepositoryMock.Setup(x => x.GetPatientRecordByIdAsync(recordId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(record);
            _mapperMock.Setup(x => x.Map<GetRecordResponse>(record)).Returns(new GetRecordResponse { Id = recordId });

            var result = await _patientRecordService.GetRecordByIdAsync(recordId);

            Assert.NotNull(result);
            Assert.Equal(recordId, result.Id);
            Assert.Single(result.Appointments);
        }

        [Fact]
        public async Task UpdateRecordAsync_ShouldReturnRecordResponse_WhenRecordIsUpdated()
        {
            var request = new RecordRequest
            {
                PatientId = "1",
                UnderlyingIllness = "Updated Illness",
                Treatment = "Updated Treatment",
                Diagnosis = "Updated Diagnosis",
                MedicalHistory = "Updated History"
            };

            var existingRecord = new PatientRecord { Id = "1", PatientId = "1" };

            _recordRepositoryMock.Setup(x => x.GetPatientRecordByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync(existingRecord);
            _recordRepositoryMock.Setup(x => x.UpdateRecordAsync(existingRecord)).ReturnsAsync(existingRecord);
            _mapperMock.Setup(x => x.Map<RecordResponse>(existingRecord)).Returns(new RecordResponse { Id = "1" });

            var result = await _patientRecordService.UpdateRecordAsync("1", request);

            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
        }

        [Fact]
        public async Task CreateRecordAsync_ShouldThrowApiException_WhenPatientIsInactive()
        {
            var request = new RecordRequest
            {
                PatientId = "1",
                UnderlyingIllness = "Illness",
                Treatment = "Treatment",
                Diagnosis = "Diagnosis",
                MedicalHistory = "History"
            };

            var patient = new Patient { Id = "1", User = new User { IsActive = false } };

            _patientRepositoryMock.Setup(x => x.GetById(request.PatientId)).ReturnsAsync(patient);

            await Assert.ThrowsAsync<ApiException>(() => _patientRecordService.CreateRecordAsync(request));
        }

        [Fact]
        public async Task GetRecordByIdAsync_ShouldThrowApiException_WhenRecordNotFound()
        {
            var recordId = "1";

            _recordRepositoryMock.Setup(x => x.GetPatientRecordByIdAsync(recordId, It.IsAny<CancellationToken>())).ReturnsAsync((PatientRecord)null);

            await Assert.ThrowsAsync<ApiException>(() => _patientRecordService.GetRecordByIdAsync(recordId));
        }
    }
}
