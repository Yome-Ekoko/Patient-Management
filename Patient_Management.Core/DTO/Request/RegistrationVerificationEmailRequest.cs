﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Management.Core.DTO.Request
{
    public class RegistrationVerificationEmailRequest
    {
        public string FirstName { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
    }
}
