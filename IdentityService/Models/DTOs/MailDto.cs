namespace IdentityService.Models.DTOs
{
    public class MailDto
    {
        public string To { get; set; } = string.Empty;

        public string From { get; set; } = "addminmail7@gmail.com";

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;
    }
}
