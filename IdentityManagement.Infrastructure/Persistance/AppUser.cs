using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IdentityManagement.Infrastructure.Persistance
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateOfBirth { get; set; }

    }
}
