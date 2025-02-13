using Patient_Management.Core.DTO.Request;
using Patient_Management.Core.DTO.Response;
using Patient_Management.Domain.Entities;
using AutoMapper;

namespace Patient_Management.Infrastructure.Configs
{
    public class MappingProfileConfiguration : Profile
    {
        public MappingProfileConfiguration()
        {
            CreateMap<User, AuthenticationResponse>(MemberList.None);
            CreateMap<User, UserResponse>(MemberList.None);
            CreateMap<Appointment, AppointmentResponse>(MemberList.None);
            CreateMap<Appointment, GetAppointmentResponse>(MemberList.None);
            CreateMap<Patient, GetPatientResponse>(MemberList.None);
            CreateMap<Patient, GetAllPatientResponse>(MemberList.None);
            CreateMap<Patient, RegisterPatientResponse>(MemberList.None);
            CreateMap<PatientRecord, RecordResponse>(MemberList.None);
            CreateMap<PatientRecord, GetRecordResponse>(MemberList.None);
        }
    }
}
