using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Patient_Management.Core.Contract.Repository;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.Exceptions;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.Entities;
using Patient_Management.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Patient_Management.Test.Repositories
{
    public class AppointmentRepositoryTest
    {
        private readonly Mock<IServiceScopeFactory> _serviceScopeFactoryMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IServiceScope> _serviceScopeMock;
        private readonly Mock<IApplicationDbContext> _dbContextMock;
        private readonly Mock<DbSet<Appointment>> _appointmentDbSetMock;
        private readonly AppointmentRepository _appointmentRepository;

        public AppointmentRepositoryTest()
        {
            _serviceScopeFactoryMock = new Mock<IServiceScopeFactory>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _serviceScopeMock = new Mock<IServiceScope>();
            _dbContextMock = new Mock<IApplicationDbContext>();
            _appointmentDbSetMock = new Mock<DbSet<Appointment>>();

            _dbContextMock.Setup(x => x.Appointments).Returns(_appointmentDbSetMock.Object);

            _serviceScopeFactoryMock.Setup(x => x.CreateScope()).Returns(_serviceScopeMock.Object);
            _serviceScopeMock.Setup(x => x.ServiceProvider.GetService(typeof(IApplicationDbContext))).Returns(_dbContextMock.Object);

            _appointmentRepository = new AppointmentRepository(_serviceScopeFactoryMock.Object, _httpContextAccessorMock.Object);
        }

        //[Fact]
        //public async Task CreateAppointmentAsync_ShouldReturnAppointment_WhenAppointmentIsCreated()
        //{
        //    // Arrange
        //    var appointment = new Appointment { Id = "1", PatientId = "1", DoctorId = "1" };

        //    // Mock the AddAsync method on the DbSet for Appointment
        //    var mockDbSet = new Mock<DbSet<Appointment>>();
        //    mockDbSet.Setup(x => x.AddAsync(It.IsAny<Appointment>(), It.IsAny<CancellationToken>()))
        //        .ReturnsAsync(appointment);  // Returns the same appointment as result

        //    // Mock the context
        //    var dbContextMock = new Mock<ApplicationDbContext>();
        //    dbContextMock.Setup(x => x.Appointments).Returns(mockDbSet.Object);  // Use the mocked DbSet

        //    // Mock SaveChangesAsync
        //    dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        //    var appointmentRepository = new AppointmentRepository(dbContextMock.Object);

        //    // Act
        //    var result = await appointmentRepository.CreateAppointmentAsync(appointment);

        //    // Assert
        //    Assert.NotNull(result);
        //    Assert.Equal("1", result.Id); // Check if the ID is returned correctly
        //}


        [Fact]
        public async Task GetAppointmentByIdAsync_ShouldReturnAppointment_WhenAppointmentExists()
        {
            // Arrange
            var appointmentId = "1";
            var appointment = new Appointment
            {
                Id = appointmentId,
                Patient = new Patient { User = new User { IsActive = true } },
                Doctor = new User()
            };

            var appointments = new List<Appointment> { appointment }.AsQueryable();
            _appointmentDbSetMock.As<IQueryable<Appointment>>().Setup(m => m.Provider).Returns(appointments.Provider);
            _appointmentDbSetMock.As<IQueryable<Appointment>>().Setup(m => m.Expression).Returns(appointments.Expression);
            _appointmentDbSetMock.As<IQueryable<Appointment>>().Setup(m => m.ElementType).Returns(appointments.ElementType);
            _appointmentDbSetMock.As<IQueryable<Appointment>>().Setup(m => m.GetEnumerator()).Returns(appointments.GetEnumerator());

            _dbContextMock.Setup(x => x.Appointments).Returns(_appointmentDbSetMock.Object);

            // Act
            var result = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(appointmentId, result.Id);
        }

        [Fact]
        public async Task GetAppointmentsForPatientAsync_ShouldReturnPagedList_WhenAppointmentsExist()
        {
            // Arrange
            var request = new GetPagedTxnsByDateReq
            {
                PageNumber = 1,
                PageSize = 10,
                StartDate = DateTime.Now.AddMonths(-1),
                EndDate = DateTime.Now
            };

            var appointments = new List<Appointment>
            {
                new Appointment { Id = "1", PatientId = "1", CreatedAt = DateTime.Now }
            }.AsQueryable();

            // Mock IQueryable for the DbSet
            _appointmentDbSetMock.As<IQueryable<Appointment>>().Setup(m => m.Provider).Returns(appointments.Provider);
            _appointmentDbSetMock.As<IQueryable<Appointment>>().Setup(m => m.Expression).Returns(appointments.Expression);
            _appointmentDbSetMock.As<IQueryable<Appointment>>().Setup(m => m.ElementType).Returns(appointments.ElementType);
            _appointmentDbSetMock.As<IQueryable<Appointment>>().Setup(m => m.GetEnumerator()).Returns(appointments.GetEnumerator());

            // Mock CountAsync and Skip/Take for pagination
            _appointmentDbSetMock.Setup(x => x.CountAsync(It.IsAny<CancellationToken>())).ReturnsAsync(appointments.Count());
            _appointmentDbSetMock.Setup(x => x.Skip(It.IsAny<int>()).Take(It.IsAny<int>()).ToListAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(appointments.ToList());

            // Act
            var result = await _appointmentRepository.GetAppointmentsForPatientAsync(request, "1");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalRecords);
            Assert.Single(result.Items);
        }

        [Fact]
        public async Task UpdateAppointmentAsync_ShouldReturnUpdatedAppointment_WhenAppointmentIsUpdated()
        {
            // Arrange
            var appointment = new Appointment
            {
                Id = "1",
                PatientId = "1",
                DoctorId = "1",
                Patient = new Patient { User = new User { IsActive = true } }
            };

            // Mock Update to return a valid EntityEntry
            _appointmentDbSetMock.Setup(x => x.Update(It.IsAny<Appointment>()))
                .Returns(Mock.Of<EntityEntry<Appointment>>);
            _dbContextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

            // Act
            var result = await _appointmentRepository.UpdateAppointmentAsync(appointment);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("1", result.Id);
        }
    }
}
