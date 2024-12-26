namespace UpliftedApi2.Models.DTOs
{
    public class CreateUserDto
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public int phoneNumber { get; set; }
        public string profilePicture { get; set; }
    }
}
