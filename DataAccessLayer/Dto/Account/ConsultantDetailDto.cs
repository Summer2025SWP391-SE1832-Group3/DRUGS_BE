namespace DataAccessLayer.Dto.Account
{
    public class ConsultantDetailDto
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string? Description { get; set; } // Có thể mở rộng thêm các trường khác
    }
} 