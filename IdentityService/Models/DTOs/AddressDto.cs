namespace IdentityService.Models.DTOs
{
    public class AddressDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Phone { get; set; }

        public int City { get; set; }

        public int District { get; set; }

        public int Ward { get; set; }

        public string Street { get; set; }

        public string UserId { get; set; }
    }
}
