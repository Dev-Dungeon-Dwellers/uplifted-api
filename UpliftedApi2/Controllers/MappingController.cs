using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpliftedApi2.Models;
using UpliftedApi2.Services;
using UpliftedApi2.Models.DTOs;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace UpliftedApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MappingController : ControllerBase
    {
        private readonly GlobalService _globalService;

        private readonly UpliftedApiContext _context;
        public MappingController(UpliftedApiContext context, GlobalService globalService)
        {
            _context = context;
            _globalService = globalService;
        }

        //GET: api/UserGroupMapping
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<UserGroupMapping>>> GetUserGroupMapping()
        {
            var userGroupMapping = await _context.UserGroupMappings.ToListAsync();
            return Ok(userGroupMapping);
        }
        //GET: api/UserGroupMapping/1
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserGroupMappingById([FromQuery] int usergroupmappingid)
        {
            if(usergroupmappingid < 0)
            {
                return BadRequest("Invalid user group mapping ID");
            }

            var userGroupMappingExists = await _context.UserGroupMappings.AnyAsync(ugm => ugm.id == usergroupmappingid);
            if(!userGroupMappingExists)
            {
                return NotFound($"User group mapping with ID {usergroupmappingid} does not exist.");
            }

            var userGroupMappings = await _globalService.GetUserGroupMappingsByIdAsync(usergroupmappingid);

            if(userGroupMappings == null || !userGroupMappings.Any())
            {
                return NotFound($"No user group mappings found with the id {usergroupmappingid}.");
            }

            return Ok(userGroupMappings);
        }
        //POST: api/UserGroupMapping
        //GENERAL CRUD FUNCTION
        [HttpPost]
        public async Task<IActionResult> CreateUserGroupMapping([FromBody] CreateUserGroupMappingDto userGroupMappingDto)
        {
            //check group exists
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == userGroupMappingDto.GroupId);
            if (!groupExists)
            {
                return NotFound($"A group with the id {userGroupMappingDto.GroupId} does not exist.");
            }

            //check role exists
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == userGroupMappingDto.RoleId);
            if (!roleExists)
            {
                return NotFound($"A role with the id {userGroupMappingDto.RoleId} does note exist.");
            }

            //check user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == userGroupMappingDto.UserId);
            if (!userExists)
            {
                return NotFound($"A user with the id {userGroupMappingDto.UserId} does not exist");
            }

            //if mapping already exists, don't execute
            var userGroupMappingExists = _context.UserGroupMappings
                .FirstOrDefaultAsync(ugm => ugm.userId == userGroupMappingDto.UserId && ugm.groupId == userGroupMappingDto.GroupId);

            if(userGroupMappingExists.Result != null)
            {
                return BadRequest($"This user group mapping already exists");
            }

            //DTO Mapping
            var userGroupMapping = new UserGroupMapping
            {
                userId = userGroupMappingDto.UserId,
                roleId = userGroupMappingDto.RoleId,
                groupId = userGroupMappingDto.GroupId,
                joinedAt = DateTime.UtcNow
            };

            //Add to DB
            _context.UserGroupMappings.Add(userGroupMapping);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserGroupMappingById), new { id = userGroupMapping.id }, userGroupMapping);
        }

        //POST: api/AddUserToGroup
        //NOTE: This will make the user the default user role
        [HttpPost("add-user-to-group")]
        public async Task<IActionResult> AddUserToGroup([FromBody] CreateUserGroupMappingDto userGroupMappingDto)
        {
            //check group exists
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == userGroupMappingDto.GroupId);
            if (!groupExists)
            {
                return NotFound($"A group with the id {userGroupMappingDto.GroupId} does not exist.");
            }

            //check role exists
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == userGroupMappingDto.RoleId);
            if (!roleExists)
            {
                return NotFound($"A role with the id {userGroupMappingDto.RoleId} does note exist.");
            }

            //check user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == userGroupMappingDto.UserId);
            if (!userExists)
            {
                return NotFound($"A user with the id {userGroupMappingDto.UserId} does not exist");
            }

            //if mapping already exists, don't execute
            var userGroupMappingExists = _context.UserGroupMappings
                .FirstOrDefaultAsync(ugm => ugm.userId == userGroupMappingDto.UserId && ugm.groupId == userGroupMappingDto.GroupId);

            if (userGroupMappingExists.Result != null)
            {
                return BadRequest($"This user group mapping already exists");
            }

            //DTO Mapping
            var userGroupMapping = new UserGroupMapping
            {
                userId = userGroupMappingDto.UserId,
                roleId = 1, //default member role
                groupId = userGroupMappingDto.GroupId,
                joinedAt = DateTime.UtcNow
            };

            //Add to DB
            _context.UserGroupMappings.Add(userGroupMapping);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserGroupMappingById), new { id = userGroupMapping.id }, userGroupMapping);
        }
        //DELETE: api/RemoveUserFromGroup
        [HttpDelete("remove-user-from-group")]
        public async Task<IActionResult> RemoveUserFromGroup([FromQuery] int userId, [FromQuery] int groupId)
        {
            //check if group exists
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == groupId);
            if(!groupExists)
            {
                return NotFound($"A group with the id {groupId} does not exist.");
            }

            //check if user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound($"A user with the id {userId} does not exist.");
            }

            //find the user group mapping
            var userGroupMapping = await _context.UserGroupMappings
                .FirstOrDefaultAsync(ugm => ugm.userId == userId && ugm.groupId == groupId);

            if(userGroupMapping == null)
            {
                return NotFound($"No mapping exists for user with id {userId} in group with id {groupId}.");
            }

            //remove mapping from the database
            _context.UserGroupMappings.Remove(userGroupMapping);
            await _context.SaveChangesAsync();

            //return 204 if successful execution
            return NoContent();
        }

        //PUT: api/AssignRole
        [HttpPut]
        public async Task<IActionResult> AssignRoleToUser([FromQuery] int userId, [FromQuery] int groupId, [FromQuery] int roleId)
        {
            //check if user exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == userId);
            if(!userExists)
            {
                return NotFound($"A user with the id {userId} does not exist.");
            }

            //check if group exists
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == groupId);
            if(!groupExists)
            {
                return NotFound($"A group with the id {groupId} does not exist.");
            }

            //check if role exists
            var roleExists = await _context.Roles.AnyAsync(r => r.Id == roleId);
            if(!roleExists)
            {
                return NotFound($"A role with the id {roleId} does not exist.");
            }

            //check if user group mapping exists
            var userGroupMapping = await _context.UserGroupMappings
                .FirstOrDefaultAsync(ugm => ugm.userId == userId && ugm.groupId == groupId);

            if(userGroupMapping == null)
            {
                return NotFound($"No mapping exists for user with id {userId} in group with id {groupId}.");
            }

            //if role already exists for this user for this group don't execute
            if(userGroupMapping.roleId == roleId)
            {
                return BadRequest($"The user with the id {userId} already has the role with id {roleId} applied.");
            }

            //update user group mapping with new role
            userGroupMapping.roleId = roleId;
            _context.UserGroupMappings.Update(userGroupMapping);
            await _context.SaveChangesAsync();

            return Ok($"Role for user with id {userId} in group with id {groupId} successfully updated to role id {roleId}");
        }
    }
}
