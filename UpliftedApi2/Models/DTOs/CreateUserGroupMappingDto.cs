namespace UpliftedApi2.Models.DTOs
{
    public class CreateUserGroupMappingDto
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public int GroupId { get; set; }
        public DateTime joinedAt { get; set; }
    }
}
