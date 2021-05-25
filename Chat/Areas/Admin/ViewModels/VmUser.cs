using Chat.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Areas.Admin.ViewModels
{
    public class VmUser
    {
        public List<CustomUser> CustomUsers { get; set; }
        public List<IdentityRole> Roles { get; set; }

    }
}
