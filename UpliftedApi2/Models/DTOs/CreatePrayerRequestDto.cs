namespace UpliftedApi2.Models.DTOs
{
    public class CreatePrayerRequestDto
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
