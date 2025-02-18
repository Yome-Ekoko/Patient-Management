using Patient_Management.Core.Contract;
using Patient_Management.Core.Contract.Repository;
using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Core.Exceptions;
using Patient_Management.Domain.Common;
using Patient_Management.Domain.Entities;
using Patient_Management.Domain.Enum;
using Patient_Management.Domain.QueryParameters;
using Patient_Management.Domain.Settings;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json.Linq;
using Patient_Management.Core.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Patient_Management.Core.Implementation
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly JWTSettings _jwtSettings;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAppSessionService _appSession;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ApiResourceUrls _apiResourceUrls;

        public UserService(IMapper mapper,
            ILogger<UserService> logger,
            IOptions<JWTSettings> jwtSettings,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAppSessionService appSession,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ApiResourceUrls> apiResourceUrls)
        {
            _mapper = mapper;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appSession = appSession;
            _httpContextAccessor = httpContextAccessor;
            _apiResourceUrls = apiResourceUrls.Value;

        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken)
        {
            User user = await _userManager.Users
                .Where(x => x.NormalizedEmail == request.Email)
                .FirstOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                throw new ApiException($"No Accounts Registered with {request.Email}.");
            }

            if (!user.IsActive)
            {
                throw new ApiException($"Inactive account.");
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    throw new ApiException($"This user has been locked. Kindly contact the administrator.");
                }
                throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, cancellationToken);

            user.IsLoggedIn = true;
            AuthenticationResponse response = _mapper.Map<User, AuthenticationResponse>(user);

            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.ExpiresIn = _jwtSettings.DurationInMinutes * 60;
            response.ExpiryDate = DateTime.Now.AddSeconds(_jwtSettings.DurationInMinutes * 60);

            user.LastLoginTime = DateTime.Now;
            await _userManager.UpdateAsync(user);

            return new Response<AuthenticationResponse>(response, $"Authenticated {user.Email}");
        }

        

        private async Task<JwtSecurityToken> GenerateJWToken(User user, CancellationToken cancellationToken)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            DateTime utcNow = DateTime.UtcNow;
            string sessionKey = Guid.NewGuid().ToString();
            await _appSession.CreateSession(sessionKey, user.UserName);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Iat, utcNow.ToString()),
                new Claim("uname", user.UserName),
                new Claim("userId", user.Id),
                new Claim("firstName", user.FirstName),
                new Claim("phone_number", user?.PhoneNumber ?? ""),
                new Claim("lastName", user.LastName),
                new Claim("emailAddress", user.Email),
                new Claim("username", user.UserName)
             

            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<Response<string>> LogoutAsync()
        {
            var username = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;

            _appSession.DeleteSession(username);

            // [TODO] Handle situations where the JWT token is expired
            User user = await _userManager.FindByNameAsync(username);
            user.IsLoggedIn = false;
            await _userManager.UpdateAsync(user);
            return new Response<string>($"Successfully logged out user with username - {username}", (int)HttpStatusCode.OK, true);
        }

        public async Task<PagedResponse<List<UserResponse>>> GetUsersAsync(UserQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            IQueryable<User> pagedData = _userManager.Users;

            string query = queryParameters.Query;
            UserRole? role = queryParameters.Role;
            UserStatus? status = queryParameters.Status;

            if (!string.IsNullOrEmpty(query))
            {
                pagedData = pagedData.Where(x => x.Id.ToLower().Contains(query.ToLower())
                   || x.UserName.ToLower().Contains(query.ToLower())
                   || x.Email.ToLower().Contains(query.ToLower())
                   || x.FirstName.ToLower().Contains(query.ToLower())
                   || x.LastName.ToLower().Contains(query.ToLower()));
            }

            if (role.HasValue)
            {
                pagedData = pagedData.Where(x => x.UserRole == role.Value);
            }

            if (status.HasValue)
            {
                bool isActive = status.Value == UserStatus.Active;
                pagedData = pagedData.Where(x => x.IsActive == isActive);
            }

            List<User> userList = await pagedData
                .OrderByDescending(x => x.CreatedAt)
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync(cancellationToken);

            List<UserResponse> response = _mapper.Map<List<User>, List<UserResponse>>(userList);

            int totalRecords = await pagedData.CountAsync(cancellationToken);

            return new PagedResponse<List<UserResponse>>(response, queryParameters.PageNumber, queryParameters.PageSize, totalRecords, $"Successfully retrieved users");
        }
      
        public async Task<Response<UserResponse>> GetUserById(string id, CancellationToken cancellationToken)
        {
            User userData = await _userManager.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken);

            if (userData == null)
            {
                throw new ApiException($"No user found for User ID - {id}.");
            }

            UserResponse response = _mapper.Map<User, UserResponse>(userData);
            if (response.Password != null)
            {
                response.Patient = new GetPatientResponse
                {
                    Id = response.Patient.Id,
                    HomeAddress = response.Patient.HomeAddress,
                    HouseNumber = response.Patient.HouseNumber,
                    CityOrTown = response.Patient.CityOrTown,
                    DateOfBirth = response.Patient.DateOfBirth,
                    Landmark = response.Patient.Landmark,
                    Nationality = response.Patient.Nationality,
                    NextOfKin = response.Patient.NextOfKin,
                    NextOfKinPhone = response.Patient.NextOfKinPhone,
                    NickName = response.Patient.NickName,
                    StateOfOrigin = response.Patient.StateOfOrigin,
                    State = response.Patient.State,
                    Lga = response.Patient.Lga,
                    StreetName = response.Patient.StreetName
                };
            }

                return new Response<UserResponse>(response, $"Successfully retrieved user details for user with Id - {id}");
        }

      
        public async Task<Response<string>> EditUserAsync(EditUserRequest request, CancellationToken cancellationToken)
        {
            User user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"Username '{request.Email}' could not be found.");
            }

            var roleName = Enum.GetName(typeof(UserRole), request.Role) ?? throw new ApiException($"Invalid role specified.");

            if (!await _roleManager.RoleExistsAsync(roleName))
            {
                throw new ApiException($"This role doesn't exists. Please check your roles and try again");
            }

            string email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "emailAddress")?.Value;

            user.FirstName = string.IsNullOrEmpty(request.FirstName) ? user.FirstName : request.FirstName;
            user.LastName = string.IsNullOrEmpty(request.LastName) ? user.LastName : request.LastName;
            user.Gender = string.IsNullOrEmpty(request.Gender) ? user.Gender : request.Gender;
            user.UpdatedAt = DateTime.Now;
            user.UserRole = request.Role ?? user.UserRole;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new ApiException(updateResult.Errors.FirstOrDefault()?.Description ?? "An error occured while updating users.");
            }

            IList<string> existingRoles = await _userManager.GetRolesAsync(user);

            if (!existingRoles.Contains(roleName))
            {
                if (existingRoles.Count > 0)
                {
                    IdentityResult roleResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);

                    if (!roleResult.Succeeded)
                    {
                        throw new ApiException(roleResult.Errors.FirstOrDefault().Description);
                    }
                }

                IdentityResult addRoleResult = await _userManager.AddToRoleAsync(user, roleName);

                if (!addRoleResult.Succeeded)
                {
                    throw new ApiException(addRoleResult.Errors.FirstOrDefault().Description);
                }
            }

            return new Response<string>(user.Id, message: $"Successfully edited user.");
        }

        //public async Task<Response<string>> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken)
        //{
        //    User user = await _userManager.FindByEmailAsync(request.Email) ?? throw new ApiException($"The user does not exist.");

        //       string email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "emailAddress")?.Value;

        //    var existingRoles = await _userManager.GetRolesAsync(user);

        //    var roleResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);
        //    var result = await _userManager.DeleteAsync(user);
        //    if (!result.Succeeded)
        //    {
        //        throw new ApiException(result.Errors.FirstOrDefault().Description);
        //    }

        //    return new Response<string>(user.Id, message: $"Successfully deleted the user.");
        //}

        public async Task<string> SoftDeleteUserAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ApiException("User not found");
            }

            user.IsActive = false;
            await _userManager.UpdateAsync(user);
            return ($"User {user.Id} is Successfully deleted");
        }

     
        public async Task<Response<string>> ChangePassword(PasswordUpdateRequest req)
        {
            var userId = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "uid")?.Value;
            var user = await _userManager.FindByIdAsync(userId);

            var result = await _userManager.ChangePasswordAsync(user, req.CurrentPassword, req.NewPassword);
            if (!result.Succeeded)
            {
                throw new ApiException($"{result.Errors.ToList()[0].Description}");
            }

            await _signInManager.RefreshSignInAsync(user);
            return new Response<string>(message: $"Password updated successfully");
        }

       

        public async Task<Response<string>> RegisterUser(RegisterRequest request)
        {

            if (!await IsEmailAllowedAsync(request.Email))
            {
                throw new ApiException("Please enter a valid email address.");
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new Exception("User already exists.");
            }
            var newUser = new User
            {

                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                CreatedAt = DateTime.Now,
                IsActive = true,
                UserName=request.Email
            };

            var response = await _userManager.CreateAsync(newUser) ?? throw new ApiException("Unable to save user");

            if (response.Succeeded == false)
            {
                throw new ApiException($"Unable to save user.");
            }

            return new Response<string>(newUser.Id, message: $"User Registered.");

        }

        public async Task<bool> IsEmailAllowedAsync(string email)
        {
            bool isEmail = Regex.IsMatch(
                email,
                @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase
            );

            if (!isEmail)
            {
                return false;
            }

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                var hasPassword = await _userManager.HasPasswordAsync(existingUser);

                if (!existingUser.EmailConfirmed && !hasPassword)
                {
                    throw new ApiException(message: "User is not verified.", errorCode: 91);
                }
                else if (existingUser.EmailConfirmed && hasPassword)
                {
                    throw new ApiException(message: "User already exists. Reset your password.", errorCode: 92);
                }
                else if (existingUser.EmailConfirmed && !hasPassword)
                {
                    throw new ApiException(message: "User's email is verified. Create a password.", errorCode: 93);
                }
                else
                {
                    throw new ApiException("User already exists.");
                }
            }

            return true;
        }

     
        public async Task<UserResponse> CreatePassword(CreatePasswordRequest request)
        {
            var validPassword = Regex.IsMatch(request.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$");
            if (!validPassword)
            {
                throw new ApiException("Password must have at least one uppercase letter, one lowercase letter, one number, one special character, and be at least 6 characters long.");
            }
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException("User not found.");
            }
            if (await _userManager.HasPasswordAsync(user))
            {
                throw new ApiException("User already has a password.");
            }
            var result = await _userManager.AddPasswordAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApiException($"Failed to add password: {errors}");
            }
 
            var userResponse = _mapper.Map<UserResponse>(user);

            
            return userResponse;
        }

    }

}

