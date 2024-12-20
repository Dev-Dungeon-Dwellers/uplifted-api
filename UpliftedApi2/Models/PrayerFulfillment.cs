using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpliftedApi2.Models
{
    public class PrayerFulfillment
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int prayerRequestId { get; set; }

        [Required]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        [Required]
        public int createdBy { get; set; }

        [Required]
        [MaxLength(1000)]
        public string bodyText { get; set; }

        [ForeignKey(nameof(prayerRequestId))]
        public PrayerRequest myPrayerRequest { get; set; }

        [ForeignKey (nameof(createdBy))]
        public User myCreatedBy { get; set; }
    }
}
