﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Patient_Management.Core.DTO.Response
{
    public class RegisterPatientResponse
    {
        public DateTime DateOfBirth { get; set; }
        public string HouseNumber { get; set; }
        public string StreetName { get; set; }
        public string Landmark { get; set; }
        public string CityOrTown { get; set; }
        public string StateOfOrigin { get; set; }
        public string NickName { get; set; }
        public string Lga { get; set; }
        public string State { get; set; }
        public string NextOfKin { get; set; }
        public string Nationality { get; set; }
        public string NextOfKinPhone { get; set; }
        public string HomeAddress { get; set; }
        public string Id { get; set; }
    }
}
