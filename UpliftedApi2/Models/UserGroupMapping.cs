using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UpliftedApi2.Models
{
    public class UserGroupMapping
    {
        [Key]
        public int id { get; set; }

        [Required]
        public int userId { get; set; }

        [Required]
        public int roleId { get; set; }

        [Required]
        public int groupId { get; set; }

        [Required]
        public DateTime joinedAt { get; set; }

        [ForeignKey(nameof(userId))]
        public User MyUser { get; set; }

        [ForeignKey(nameof(roleId))]
        public Role MyRole { get; set; }

        [ForeignKey(nameof(groupId))]
        public Group MyGroup { get; set; }
    }
}
