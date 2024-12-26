using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpliftedApi2.Models;
using UpliftedApi2.Models.DTOs;

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
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            var groups = await _context.Groups.ToListAsync();

            if(groups.Count == 0)
            {
                return NotFound("No groups were found");
            }

            return Ok(groups);
        }


        // GET: api.Group/1
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            var group = await _context.Groups.FindAsync(id);

            if(group == null)
            {
                return NotFound($"No group with the ID {id} was found.");
            }

            return Ok(group);
        }

        //POST: api/Group
        [HttpPost]
        public async Task<ActionResult<Group>> CreateGroup(CreateGroupDto groupDto)
        {
            if(groupDto == null)
            {
                return BadRequest("Group data is required.");
            }

            //DTO mapping
            var group = new Group
            {
                Name = groupDto.name,
                Description = groupDto.description
            };

            //add to db
            _context.Groups.Add(group);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetGroup), new {id = group.Id}, group);
        }

    }
}
