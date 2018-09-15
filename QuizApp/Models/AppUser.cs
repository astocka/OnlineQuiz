using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace QuizApp.Models
{
    public class AppUser : IdentityUser<int>
    {
        public AppUser(string userName) : base(userName)
        {
        }

        public AppUser()
        {
        }

        public string Name { get; set; }
    }
}
