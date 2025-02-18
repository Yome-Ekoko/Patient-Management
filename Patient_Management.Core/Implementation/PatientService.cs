using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Org.BouncyCastle.Crypto.Prng;
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
    public class PatientService: IPatientService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IPatientRepository _patientRepository;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public PatientService(
            IHttpContextAccessor httpContextAccessor,
            IPatientRepository patientRepository,
            UserManager<User> userManager,
            IMapper mapper
           )
        {
            _httpContextAccessor = httpContextAccessor;
            _patientRepository = patientRepository;
            _userManager = userManager;
            _mapper = mapper;
        }
        public async Task<Response<RegisterPatientResponse>> RegisterPatient(RegisterPatientRequest request)
        {
           // var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? "";
            var userId = request.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ApiException("Please enter UserId");
            }
            var user = await _userManager.Users.Where(x => x.Id == userId).Include(c => c.Patient).FirstOrDefaultAsync();
            if (user == null)
            {
                throw new ApiException("User not found.");
            }
            if (user.Patient != null && user.Patient.UserId == userId)
            {
                throw new ApiException("Patient already exists");
            }

            var newPatient = new Patient
            {
                NextOfKinPhone = request.NextOfKinPhone,
                Nationality = request.Nationality,
                NextOfKin = request.NextOfKin,
                NickName = request.NickName,
                HouseNumber = request.HouseNumber,
                State = request.StateOfResidence,
                StateOfOrigin = request.StateOfOrigin,
                StreetName = userId,
                CreatedAt = DateTime.Now,
                CityOrTown = request.CityOrTown,
                DateOfBirth = request.DateOfBirth,
                Landmark = request.Landmark,
                Lga = request.Lga,
                UserId=userId

            };

            var response = await _patientRepository.AddPatient(newPatient) ?? throw new ApiException("Unable to save Patient");
            user.UserRole = UserRole.Patient;

            var patient = await _userManager.UpdateAsync(user);

            var result = _mapper.Map<RegisterPatientResponse>(response);

            return new Response<RegisterPatientResponse>(result, message: "Success");
        }

        public async Task<PagedResponse<List<GetAllPatientResponse>>> GetPatients(LogQueryParameters queryParameters, CancellationToken cancellationToken)
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

            var logResponse = await _patientRepository.GetPagedByDateRange(getLogDto, cancellationToken);

            if (logResponse.Items.Count <= 0)
            {
                throw new ApiException($"No Patient found.");
            }

            var response = _mapper.Map<List<GetAllPatientResponse>>(logResponse.Items);

            return new PagedResponse<List<GetAllPatientResponse>>(response, queryParameters.PageNumber, queryParameters.PageSize, logResponse.TotalRecords, $"Successfully retrieved logs");
        }

        public async Task<Response<GetPatientResponse>> GetPatientById(string Id)
        {
            Patient patient = await _patientRepository.GetById(Id) ?? throw new ApiException($"No Patient found.");


            var response = _mapper.Map<GetPatientResponse>(patient);
            if (patient.User != null)
            {
                response.User = new UserResponse
                {
                    Id = patient.User.Id,
                    FirstName = patient.User.FirstName,
                    Email = patient.User.Email,
                    LastName = patient.User.LastName,
                    UserName = patient.User.UserName,

                };
            }

                return new Response<GetPatientResponse>(response, $"Successfully retrieved Transaction details.");
        }
        public async Task<Response<RegisterPatientResponse>> UpdatePatient(UpdatePatientRequest request)
        {
            var UserId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? "";

            var existingPatient = await _patientRepository.GetById(request.Id) ?? throw new ApiException("Patient Not Found");

            existingPatient.HomeAddress = string.IsNullOrEmpty(request.HomeAddress) ? existingPatient.HomeAddress : request.HomeAddress;
            existingPatient.HouseNumber = string.IsNullOrEmpty(request.HouseNumber) ? existingPatient.HouseNumber : request.HouseNumber;
            existingPatient.NextOfKinPhone = string.IsNullOrEmpty(request.NextOfKinPhone) ? existingPatient.NextOfKinPhone : request.NextOfKinPhone;
            existingPatient.NextOfKin = string.IsNullOrEmpty(request.NextOfKin) ? existingPatient.NextOfKin : request.NextOfKin;
            existingPatient.Landmark = string.IsNullOrEmpty(request.Landmark) ? existingPatient.Landmark : request.Landmark;
            existingPatient.Nationality = string.IsNullOrEmpty(request.Nationality) ? existingPatient.Nationality : request.Nationality;
            existingPatient.CityOrTown = string.IsNullOrEmpty(request.CityOrTown) ? existingPatient.CityOrTown : request.CityOrTown;
            existingPatient.State = string.IsNullOrEmpty(request.StateOfResidence) ? existingPatient.State : request.StateOfResidence;
            existingPatient.Lga = string.IsNullOrEmpty(request.Lga) ? existingPatient.Lga : request.Lga;
            existingPatient.StreetName = string.IsNullOrEmpty(request.StreetName) ? existingPatient.StreetName : request.StreetName;


            var result = await _patientRepository.Update(existingPatient) ?? throw new ApiException("Unable to update Patient");

            var response = _mapper.Map<RegisterPatientResponse>(result);

            return new Response<RegisterPatientResponse>(response);
        }
    }
}
