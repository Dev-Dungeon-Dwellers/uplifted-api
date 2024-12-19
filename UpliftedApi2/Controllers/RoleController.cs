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
        public ActionResult<IEnumerable<Role>> GetRoles()
        {
            return _context.Roles.ToList();
        }

        // GET: api/Role/1
        [HttpGet("{id}")]
        public ActionResult<Role> GetRole(int id)
        {
            var role = _context.Roles.Find(id);
            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // POST: api/Role
        [HttpPost]
        public ActionResult<Role> CreateRole(Role role)
        {
            if (role == null)
            {
                return BadRequest();
            }
            _context.Roles.Add(role);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
        }
    }
}
