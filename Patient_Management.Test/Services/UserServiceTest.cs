using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MockQueryable;
using Moq;
using Patient_Management.Core.Contract;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Core.Exceptions;
using Patient_Management.Core.Implementation;
using Patient_Management.Domain.Entities;
using Patient_Management.Domain.Settings;
using Xunit;

namespace Patient_Management.Test.Services
{
    public class UserServiceTest
    {
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UserService>> _loggerMock;
        private readonly Mock<IOptions<JWTSettings>> _jwtSettingsMock;
        private readonly Mock<UserManager<User>> _userManagerMock;
        private readonly Mock<SignInManager<User>> _signInManagerMock;
        private readonly Mock<RoleManager<IdentityRole>> _roleManagerMock;
        private readonly Mock<IAppSessionService> _appSessionMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<IOptions<ApiResourceUrls>> _apiResourceUrlsMock;
        private readonly UserService _userService;

        public UserServiceTest()
        {
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UserService>>();
            _jwtSettingsMock = new Mock<IOptions<JWTSettings>>();
            _userManagerMock = MockUserManager<User>();
            _signInManagerMock = MockSignInManager<User>();
            _roleManagerMock = MockRoleManager<IdentityRole>();
            _appSessionMock = new Mock<IAppSessionService>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _apiResourceUrlsMock = new Mock<IOptions<ApiResourceUrls>>();

            _jwtSettingsMock.Setup(x => x.Value).Returns(new JWTSettings
            {
                Key = "test_key_1234567890",
                Issuer = "test_issuer",
                Audience = "test_audience",
                DurationInMinutes = 60
            });

            _userService = new UserService(
                _mapperMock.Object,
                _loggerMock.Object,
                _jwtSettingsMock.Object,
                _userManagerMock.Object,
                _signInManagerMock.Object,
                _roleManagerMock.Object,
                _appSessionMock.Object,
                _httpContextAccessorMock.Object,
                _apiResourceUrlsMock.Object
            );
        }

        [Fact]
        public async Task AuthenticateAsync_ShouldReturnAuthenticationResponse_WhenCredentialsAreValid()
        {
            var request = new AuthenticationRequest { Email = "test@example.com", Password = "Password123!" };
            var user = new User
            {
                Id = "1",
                Email = "test@example.com",
                NormalizedEmail = "TEST@EXAMPLE.COM",  
                UserName = "testuser",
                IsActive = true
            };

            var users = new List<User> { user }.AsQueryable().BuildMock();

            _userManagerMock.Setup(x => x.Users).Returns(users);

            _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email.ToUpper()))
                .ReturnsAsync(user);
        }

        [Fact]
        public async Task RegisterUser_ShouldReturnUserId_WhenRegistrationIsSuccessful()
        {
            var request = new RegisterRequest
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890"
            };

            _userManagerMock.Setup(x => x.FindByEmailAsync(request.Email)).ReturnsAsync((User)null);
            _userManagerMock.Setup(x => x.CreateAsync(It.IsAny<User>())).ReturnsAsync(IdentityResult.Success);

            var result = await _userService.RegisterUser(request);

            Assert.NotNull(result);
            Assert.Equal("User Registered.", result.Message);
        }

        private static Mock<UserManager<TUser>> MockUserManager<TUser>() where TUser : class
        {
            var store = new Mock<IUserStore<TUser>>();
            return new Mock<UserManager<TUser>>(store.Object, null, null, null, null, null, null, null, null);
        }

        private static Mock<SignInManager<TUser>> MockSignInManager<TUser>() where TUser : class
        {
            var userManager = MockUserManager<TUser>().Object;
            var contextAccessor = new Mock<IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<TUser>>();
            return new Mock<SignInManager<TUser>>(userManager, contextAccessor.Object, claimsFactory.Object, null, null, null, null);
        }

        private static Mock<RoleManager<TRole>> MockRoleManager<TRole>() where TRole : class
        {
            var store = new Mock<IRoleStore<TRole>>();
            return new Mock<RoleManager<TRole>>(store.Object, null, null, null, null);
        }
    }
}