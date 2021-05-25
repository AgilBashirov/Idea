using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Chat.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public IdentityUser Sender { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public IdentityUser Receiver { get; set; }

        public string MessageText { get; set; }
        public DateTime Date { get; set; }
        public bool IsRead { get; set; }


    }
}
