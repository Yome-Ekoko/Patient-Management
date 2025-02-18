using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Patient_Management.Core.Contract.Repository;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Core.Exceptions;
using Patient_Management.Core.Implementation;
using Patient_Management.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Moq.EntityFrameworkCore;
using Xunit;

namespace Patient_Management.Test.Services
{
    public class PatientServiceTest
    {
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IPatientRepository> _patientRepositoryMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly PatientService _patientService;

        public PatientServiceTest()
        {
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _patientRepositoryMock = new Mock<IPatientRepository>();
            _userManagerMock = MockUserManager();
            _mapperMock = new Mock<IMapper>();

            _patientService = new PatientService(
                _httpContextAccessorMock.Object,
                _patientRepositoryMock.Object,
                _userManagerMock.Object,
                _mapperMock.Object
            );
        }


        [Fact]
        public async Task GetPatientById_ShouldReturnPatientResponse_WhenPatientExists()
        {
            var patientId = "1";
            var patient = new Patient
            {
                Id = patientId,
                UserId = "1",
                User = new User { Id = "1", FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", UserName = "johndoe" }
            };

            _patientRepositoryMock.Setup(x => x.GetById(patientId)).ReturnsAsync(patient);
            _mapperMock.Setup(x => x.Map<GetPatientResponse>(patient)).Returns(new GetPatientResponse { Id = patientId });

            var result = await _patientService.GetPatientById(patientId);

            Assert.NotNull(result);
            Assert.Equal(patientId, result.Data.Id);
            Assert.Equal("John", result.Data.User.FirstName);
            Assert.Equal("john.doe@example.com", result.Data.User.Email);

            _patientRepositoryMock.Verify(x => x.GetById(patientId), Times.Once);
        }

        [Fact]
        public async Task UpdatePatient_ShouldReturnSuccessResponse_WhenPatientIsUpdated()
        {
            var request = new UpdatePatientRequest
            {
                Id = "1",
                HomeAddress = "New Address"
            };

            var patient = new Patient { Id = "1", UserId = "1" };

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(new DefaultHttpContext
            {
                User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[] { new Claim("userId", "1") }))
            });

            _patientRepositoryMock.Setup(x => x.GetById(request.Id)).ReturnsAsync(patient);
            _patientRepositoryMock.Setup(x => x.Update(It.IsAny<Patient>())).ReturnsAsync(patient);
            _mapperMock.Setup(x => x.Map<RegisterPatientResponse>(It.IsAny<Patient>())).Returns(new RegisterPatientResponse { Id = "1" });

            var result = await _patientService.UpdatePatient(request);

            Assert.NotNull(result);
            Assert.Equal("1", result.Data.Id);

            _patientRepositoryMock.Verify(x => x.Update(It.IsAny<Patient>()), Times.Once);
        }

        private static Mock<UserManager<User>> MockUserManager()
        {
            var store = new Mock<IUserStore<User>>();
            var options = new Mock<IOptions<IdentityOptions>>();
            var passwordHasher = new Mock<IPasswordHasher<User>>();
            var userValidators = new List<IUserValidator<User>> { new Mock<IUserValidator<User>>().Object };
            var passwordValidators = new List<IPasswordValidator<User>> { new Mock<IPasswordValidator<User>>().Object };
            var keyNormalizer = new Mock<ILookupNormalizer>();
            var errorDescriber = new Mock<IdentityErrorDescriber>();
            var services = new Mock<IServiceProvider>();
            var logger = new Mock<ILogger<UserManager<User>>>();

            return new Mock<UserManager<User>>(
                store.Object,
                options.Object,
                passwordHasher.Object,
                userValidators,
                passwordValidators,
                keyNormalizer.Object,
                errorDescriber.Object,
                services.Object,
                logger.Object);
        }
    }
}
