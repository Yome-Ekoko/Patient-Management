using Patient_Management.Domain.Constant;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Patient_Management.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static List<IdentityRole> IdentityRoleList()
        {
            return new List<IdentityRole>()
            {
                new IdentityRole
                {
                    Id = RoleConstants.Administrator,
                    Name = Roles.Administrator,
                    NormalizedName = Roles.Administrator.ToUpper(),
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a23"
                },
                new IdentityRole
                {
                    Id = RoleConstants.Patient,
                    Name = Roles.Patient,
                    NormalizedName = Roles.Patient.ToUpper(),
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a56"
                },
                new IdentityRole
                {
                    Id = RoleConstants.Doctor,
                    Name = Roles.Doctor,
                    NormalizedName = Roles.Doctor.ToUpper(),
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a87"
                }
            };
        }
    }
}
