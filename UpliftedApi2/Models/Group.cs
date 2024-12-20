using Microsoft.EntityFrameworkCore.Update.Internal;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace UpliftedApi2.Models
{
    public class Group
    {
        /// <summary>
        /// Group ID
        /// </summary>
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// Group Name
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        /// <summary>
        /// Group Description
        /// </summary>
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Group Created At Timestamp
        /// </summary>
        [Required]
        public DateTime createdAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Most Recent Updated Timestamp
        /// </summary>
        public DateTime uploadedAt { get; set; }

        /// <summary>
        /// Group Status ID
        /// </summary>
        [Required]
        public int status { get; set; } = 1;
    }
}
