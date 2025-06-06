﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Model
{
    public class ApplicationUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; } = null!;
        [PersonalData]
        public DateTime DateOfBirth { get; set; }
        [PersonalData]
        public string Gender { get; set; } = null!;
        [PersonalData]
        public DateTime CreatedAt { get; set; } 
    }
}
