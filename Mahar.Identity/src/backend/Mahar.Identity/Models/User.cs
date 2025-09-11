using Microsoft.AspNetCore.Identity;
using System;

namespace Mahar.Identity.Models
{
    public class User : IdentityUser<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public string RefreshToken { get; set; }
    }
}