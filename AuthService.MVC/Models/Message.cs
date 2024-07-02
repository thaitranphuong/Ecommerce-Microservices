using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.MVC.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        
        [DataType(DataType.Text)]
        public string Content { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedTime { get; set; }

        [ForeignKey("Sender")]
        public string SenderId { get; set; }
        public AppUser Sender { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public AppUser Receiver { get; set; }

        public Message()
        {
            CreatedTime = DateTime.Now;
        }
    }
}
