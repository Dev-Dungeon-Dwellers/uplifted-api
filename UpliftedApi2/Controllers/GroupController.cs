using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpliftedApi2.Models;

namespace UpliftedApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : ControllerBase
    {
        private readonly UpliftedApiContext _context;
        public GroupController(UpliftedApiContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Gets all groups from database
        /// </summary>
        /// <returns></returns>
        // Get api/Group
        [HttpGet]
        public ActionResult<IEnumerable<Group>> GetGroups()
        {
            return _context.Groups.ToList();
        }


        // GET: api.Group/1
        [HttpGet("{id}")]
        public ActionResult<Group> GetGroup(int id)
        {
            var group = _context.Groups.Find(id);
            if(group == null)
            {
                return NotFound();
            }
            return group;
        }

        //POST: api/Group
        [HttpPost]
        public ActionResult<Group> CreateGroup(Group group)
        {
            if(group == null)
            {
                return BadRequest();
            }
            _context.Groups.Add(group);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetGroup), new { id = group.Id }, group);
        }

    }
}
