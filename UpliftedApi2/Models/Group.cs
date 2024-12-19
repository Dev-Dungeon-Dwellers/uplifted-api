using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace UpliftedApi2.Models
{
    public class Group
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        [Required]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        public DateTime uploadedAt { get; set; }

        [Required]
        public int status { get; set; } = 1;
    }
}
