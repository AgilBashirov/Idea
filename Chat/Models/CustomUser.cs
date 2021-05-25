using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class CustomUser: IdentityUser
    {
        public string Name { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [NotMapped]
        public string RoleId { get; set; }

        [NotMapped]
        public List<SelectListItem> Roles { get; set; }
    }
}
