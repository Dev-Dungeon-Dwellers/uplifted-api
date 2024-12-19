using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;

namespace UpliftedApi2.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string userName { get; set; }

        [Required]
        [MaxLength(50)]
        public string password { get; set; }

        [Required]
        [MaxLength(100)]
        public string email { get; set; }

        [Required]
        public int phoneNumber { get; set; }

        [Required]
        [MaxLength(1000)]
        public string profilePicture { get; set; }

        [Required]
        public DateTime createdAt { get; set; } = DateTime.Now;

        public DateTime updatedAt { get; set; }

        [Required]
        public DateTime lastLogin { get; set; }

        [Required]
        public int status { get; set; }



    }
}
