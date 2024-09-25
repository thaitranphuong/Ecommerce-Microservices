using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Models
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
        public User Sender { get; set; }

        [ForeignKey("Receiver")]
        public string ReceiverId { get; set; }
        public User Receiver { get; set; }

        public Message()
        {
            CreatedTime = DateTime.Now;
        }
    }
}
