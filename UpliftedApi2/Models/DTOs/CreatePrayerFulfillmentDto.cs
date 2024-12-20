namespace UpliftedApi2.Models.DTOs
{
    public class CreatePrayerFulfillmentDto
    {
        public int prayerRequestId { get; set; }
        
        public int createdBy { get; set; }
        public string bodyText { get; set; }
    }
}
