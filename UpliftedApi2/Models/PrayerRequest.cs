using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpliftedApi2.Models
{
    public class PrayerRequest
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int groupId { get; set; }

        [Required]
        public int userId { get; set; }

        [Required]
        [MaxLength(100)]
        public string title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string description { get; set; }

        [Required]
        public DateTime created_at { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(groupId))]
        public Group MyGroup { get; set; }

        [ForeignKey(nameof(userId))]
        public User MyUser { get; set; }
    }
}
