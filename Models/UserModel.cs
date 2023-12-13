using Microsoft.AspNetCore.Identity;
using SchiftPlanner.Models.Subs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchiftPlanner.Models
{
    public class UserModel : IdentityUser
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }

        public List<Subscriptions> Subscriptions { get; set; }

    }
}
