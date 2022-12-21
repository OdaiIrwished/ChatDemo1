using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace ChatDemo1.Data
{
    public class ApplicationUser : IdentityUser
    {
        public int FirstName { get; set; }
        public int LastName { get; set; }

    }

}