using Patient_Management.Domain.Constant;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Patient_Management.Persistence.Seeds
{
    public static class MappingUserRole
    {
        public static List<IdentityUserRole<string>> IdentityUserRoleList()
        {
            return new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId = RoleConstants.Patient,
                    UserId = RoleConstants.PatientUser
                },
                new IdentityUserRole<string>
                {
                    RoleId = RoleConstants.Administrator,
                    UserId = RoleConstants.AdministratorUser
                },
                new IdentityUserRole<string>
                {
                    RoleId = RoleConstants.Doctor,
                    UserId = RoleConstants.DoctorUser
                }
            };
        }
    }
}
