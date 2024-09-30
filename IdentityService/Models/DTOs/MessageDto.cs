using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace IdentityService.Models.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public string SenderId { get; set; }
        public string SenderName { get; set; }
        public string Avatar { get; set; }
        public string ReceiverId { get; set; }
    }
}
