using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
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
    }
}
