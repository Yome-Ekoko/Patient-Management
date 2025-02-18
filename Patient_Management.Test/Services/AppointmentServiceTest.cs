
using AutoMapper;
using Moq;
using Patient_Management.Core.Contract.Repository;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Core.Exceptions;
using Patient_Management.Core.Implementation;
using Patient_Management.Domain.Entities;
using Patient_Management.Domain.Enum;
using Patient_Management.Domain.QueryParameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Patient_Management.Test.Services
{
    public class AppointmentServiceTest
    {
        private readonly Mock<IAppointmentRepository> _appointmentRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AppointmentService _appointmentService;

        public AppointmentServiceTest()
        {
            _appointmentRepositoryMock = new Mock<IAppointmentRepository>();
            _mapperMock = new Mock<IMapper>();

            _appointmentService = new AppointmentService(
                _appointmentRepositoryMock.Object,
                _mapperMock.Object
            );
        }

        [Fact]
        public async Task BookAppointmentAsync_ShouldReturnAppointmentResponse_WhenAppointmentIsCreated()
        {
            var request = new AppointmentRequest
            {
                PatientId = "1",
                DoctorId = "1",
                PatientRecordId = "1",
                AppointmentDate = DateTime.Now,
                AppointmentTime = "10:00 AM",
                Notes = "Initial Consultation"
            };

            var appointment = new Appointment
            {
                Id = "1",
                PatientId = "1",
                DoctorId = "1",
                PatientRecordId = "1",
                AppointmentDate = request.AppointmentDate,
                AppointmentTime = request.AppointmentTime,
                Notes = request.Notes
            };

            _appointmentRepositoryMock.Setup(x => x.CreateAppointmentAsync(It.IsAny<Appointment>())).ReturnsAsync(appointment);
            _mapperMock.Setup(x => x.Map<AppointmentResponse>(appointment)).Returns(new AppointmentResponse { Id = "1" });

            var result = await _appointmentService.BookAppointmentAsync(request);

            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
        }

        [Fact]
        public async Task GetAppointmentAsync_ShouldReturnGetAppointmentResponse_WhenAppointmentExists()
        {
            var appointmentId = "1";
            var appointment = new Appointment
            {
                Id = appointmentId,
                Patient = new Patient { User = new User { FirstName = "John", LastName = "Doe" } },
                Doctor = new User { FirstName = "Dr.", LastName = "Smith" }
            };

            _appointmentRepositoryMock.Setup(x => x.GetAppointmentByIdAsync(appointmentId, It.IsAny<CancellationToken>())).ReturnsAsync(appointment);
            _mapperMock.Setup(x => x.Map<GetAppointmentResponse>(appointment)).Returns(new GetAppointmentResponse { Id = appointmentId });

            var result = await _appointmentService.GetAppointmentAsync(appointmentId);

            Assert.NotNull(result);
            Assert.Equal(appointmentId, result.Id);
            Assert.Equal("John Doe", result.PatientName);
            Assert.Equal("Dr. Smith", result.DoctorName);
        }


        [Fact]
        public async Task UpdateAppointmentAsync_ShouldReturnAppointmentResponse_WhenAppointmentIsUpdated()
        {
            var request = new AppointmentRequest
            {
                PatientId = "1",
                DoctorId = "2",
                AppointmentDate = DateTime.Now,
                AppointmentTime = "11:00 AM",
                Notes = "Follow-up Consultation"
            };

            var existingAppointment = new Appointment { Id = "1", PatientId = "1", DoctorId = "1" };

            _appointmentRepositoryMock.Setup(x => x.GetAppointmentByIdAsync("1", It.IsAny<CancellationToken>())).ReturnsAsync(existingAppointment);
            _appointmentRepositoryMock.Setup(x => x.UpdateAppointmentAsync(existingAppointment)).ReturnsAsync(existingAppointment);
            _mapperMock.Setup(x => x.Map<AppointmentResponse>(existingAppointment)).Returns(new AppointmentResponse { Id = "1" });

            var result = await _appointmentService.UpdateAppointmentAsync("1", request);

            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
        }

        [Fact]
        public async Task CompleteAppointmentAsync_ShouldReturnTrue_WhenAppointmentIsCompleted()
        {
            var appointmentId = "1";
            var appointment = new Appointment { Id = appointmentId, Status = AppointmentStatus.Scheduled };

            _appointmentRepositoryMock.Setup(x => x.GetAppointmentByIdAsync(appointmentId, It.IsAny<CancellationToken>())).ReturnsAsync(appointment);
            _appointmentRepositoryMock.Setup(x => x.UpdateAppointmentAsync(appointment)).ReturnsAsync(appointment);

            var result = await _appointmentService.CompleteAppointmentAsync(appointmentId);

            Assert.True(result);
            Assert.Equal(AppointmentStatus.Completed, appointment.Status);
        }

        [Fact]
        public async Task CancelAppointmentAsync_ShouldReturnTrue_WhenAppointmentIsCancelled()
        {
            var appointmentId = "1";
            var appointment = new Appointment { Id = appointmentId, Status = AppointmentStatus.Scheduled };

            _appointmentRepositoryMock.Setup(x => x.GetAppointmentByIdAsync(appointmentId, It.IsAny<CancellationToken>())).ReturnsAsync(appointment);
            _appointmentRepositoryMock.Setup(x => x.UpdateAppointmentAsync(appointment)).ReturnsAsync(appointment);

            
            var result = await _appointmentService.CancelAppointmentAsync(appointmentId);

            Assert.True(result);
            Assert.Equal(AppointmentStatus.Cancelled, appointment.Status);
        }
    }
}