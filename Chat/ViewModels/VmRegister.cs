using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.ViewModels
{
    public class VmRegister
    {
        [Required, MaxLength(30)]
        public string Name { get; set; }

        [Required, MaxLength(30)]
        public string Email { get; set; }

        [Required, MaxLength(30)]
        public string Password { get; set; }

        [Required, MaxLength(30)]
        public string ConfirmPassword { get; set; }

    }
}
