using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using UpliftedApi2.Models;
using UpliftedApi2.Models.DTOs;

namespace UpliftedApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UpliftedApiContext _context;
        public UserController(UpliftedApiContext context)
        {
            _context = context;
        }

        // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            if (users.Count == 0)
            {
                return NotFound("No users were found.");
            }

            return Ok(users);
        }

        // GET: api/User/1
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound($"No user with the ID {id} was found.");
            }

            return Ok(user);
        }

        //POST: api/User
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser(CreateUserDto userDto)
        {
            if(userDto == null)
            {
                return BadRequest("User data is required.");
            }

            //check if user already exists (must have unique email)
            var userExists = await _context.Users.AnyAsync(u => u.email == userDto.email);

            if(userExists)
            {
                return BadRequest($"A user with the email {userDto.email} already exists");
            }

            //DTO mapping
            var user = new User
            {
                userName = userDto.userName,
                password = userDto.password,
                email = userDto.email,
                phoneNumber = userDto.phoneNumber,
                profilePicture = userDto.profilePicture
            };

            //add to db
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        //DELETE user
        [HttpDelete]
        public async Task<ActionResult> DeleteUser([FromQuery] int userId)
        {
            //check if user id is valid
            if(userId < 0)
            {
                return BadRequest("Invalid userId.");
            }

            //check if user exists
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if(user == null)
            {
                return NotFound($"User with ID {userId} not found.");
            }
            
            //check if user is in any groups
            var userGroupMappings = await _context.UserGroupMappings.Where(ugm => ugm.userId == userId).ToListAsync();

            //delete group mappings
            _context.UserGroupMappings.RemoveRange(userGroupMappings);

            //check if user has any prayer request history
            var prayerRequests = await _context.PrayerRequests.Where(pr => pr.userId == userId).ToListAsync();

            //delete prayer request history
            _context.PrayerRequests.RemoveRange(prayerRequests);

            //delete user
            _context.Users.Remove(user);

            //save changes
            try
            {
                await _context.SaveChangesAsync();
                return Ok($"User with ID {userId} deleted successfully");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"An error occured while deleting the user: {ex.Message}");
            }
        }
        //PUT change user status

    }
}
