using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpliftedApi2.Models;

namespace UpliftedApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly UpliftedApiContext _context;
        public RoleController(UpliftedApiContext context)
        {
            _context = context;
        }

        //vGET: api/Role
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            var roles = await _context.Roles.ToListAsync();

            if(roles.Count == 0)
            {
                return NotFound("No roles were found");
            }

            return Ok(roles);
        }

        // GET: api/Role/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound($"No role with the ID {id} was found.");
            }

            return Ok(role);
        }

        // POST: api/Role
        [HttpPost]
        public async Task<ActionResult<Role>> CreateRole(Role role)
        {
            if (role == null)
            {
                return BadRequest("Role data required.");
            }

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }
    }
}
