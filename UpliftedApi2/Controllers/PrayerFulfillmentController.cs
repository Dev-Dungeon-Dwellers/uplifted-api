using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpliftedApi2.Models;
using UpliftedApi2.Models.DTOs;
using UpliftedApi2.Services;

namespace UpliftedApi2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrayerFulfillmentController : ControllerBase
    {
        private readonly GlobalService _globalService;

        private readonly UpliftedApiContext _context;

        public PrayerFulfillmentController(UpliftedApiContext context, GlobalService globalService)
        {
            _context = context;
            _globalService = globalService;
        }

        // GET: api/PrayerFulfillment
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PrayerFulfillment>>> GetPrayerFulfillments()
        {
            var prayerFulfillments = await _context.PrayerFulfillments.ToListAsync();
            return Ok(prayerFulfillments);
        }

        // GET: api/PrayerFulfillment/1 BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PrayerFulfillment>> GetPrayerFulfillmentById(int id)
        {
            var prayerFulfillment = await _context.PrayerFulfillments.FindAsync(id);

            if (prayerFulfillment == null)
            {
                return NotFound($"Prayer fulfillment with ID {id} not found.");
            }

            return Ok(prayerFulfillment);
        }

        // GET: api/prayerFulfillment?group=1
        [HttpGet("by-group")]
        public async Task<IActionResult> GetPrayerFulfillmentsByGroupId([FromQuery] int groupid)
        {
            if (groupid < 0)
            {
                return BadRequest("Invalid group ID");
            }

            // Check if the group exists
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == groupid);
            if (!groupExists)
            {
                return NotFound($"Group with ID {groupid} does not exist.");
            }

            var prayerFulfillments = await _globalService.GetPrayerFulfillmentsByGroupAsync(groupid);
            if (prayerFulfillments == null || !prayerFulfillments.Any())
            {
                return NotFound("No prayer fulfilment found for the specified group.");
            }

            return Ok(prayerFulfillments);
        }

        // GET api/prayerFulfillment?user=1
        [HttpGet("by-user")]
        public async Task<IActionResult> GetPrayerFulfillmentByUserId([FromQuery] int userid)
        {
            if (userid < 0)
            {
                return BadRequest("Invalid user ID");
            }

            //Check if the group exists
            var userExists = await _context.Users.AnyAsync(u => u.Id == userid);
            if (!userExists)
            {
                return NotFound($"User with ID {userid} does not exist.");
            }

            var prayerFulfillments = await _globalService.GetPrayerFulfillmentByUserAsync(userid);
            if (prayerFulfillments == null || !prayerFulfillments.Any())
            {
                return NotFound("No prayer fulfillment found for the specified group.");
            }
            return Ok(prayerFulfillments);
        }

        // GET: api/prayerFulfillment/by-created-by
        [HttpGet("by-created-by")]
        public async Task<IActionResult> GetPrayerFulfillmentByCreatedByUserId([FromQuery] int createdbyuserid)
        {
            if(createdbyuserid < 0)
            {
                return BadRequest("Invalid creatd by user ID");
            }

            var createdByUserExists = await _context.Users.AnyAsync(u => u.Id == createdbyuserid);
            if(!createdByUserExists)
            {
                return NotFound($"Created by user with ID {createdbyuserid} does not exist.");
            }

            var prayerFulfillments = await _globalService.GetPrayerFulfillmentByCreatedByUserAsync(createdbyuserid);
            if(prayerFulfillments == null || !prayerFulfillments.Any())
            {
                return NotFound("No prayer fulfillment found for the user.");
            }

            return Ok(prayerFulfillments);
        }

        [HttpGet("by-prayer-request")]
        public async Task<IActionResult> GetPrayerFulfillmentByPrayerRequestId([FromQuery] int prayerrequestid)
        {
            if (prayerrequestid < 0)
            {
                return BadRequest("Invalid prayer request ID");
            }

            //Check if the group exists
            var prayerRequestExists = await _context.PrayerRequests.AnyAsync(u => u.Id == prayerrequestid);
            if (!prayerRequestExists)
            {
                return NotFound($"User with ID {prayerrequestid} does not exist.");
            }

            var prayerFulfillments = await _globalService.GetPrayerFulfillmentByPrayerRequestAsync(prayerrequestid);
            if (prayerFulfillments == null || !prayerFulfillments.Any())
            {
                return NotFound("No prayer fulfillment found for the specified group.");
            }
            return Ok(prayerFulfillments);
        }

        // POST: api/PrayerFulfillment
        [HttpPost]
        public async Task<IActionResult> CreatePrayerFulfillment([FromBody] CreatePrayerFulfillmentDto prayerFulfillmentDto)
        {
            if (prayerFulfillmentDto == null)
            {
                return BadRequest("Prayer request data is required");
            }

            //prayer request validation
            var prayerRequestExists = await _context.PrayerRequests.AnyAsync(pr => pr.Id == prayerFulfillmentDto.prayerRequestId);
            if(!prayerRequestExists)
            {
                return NotFound($"Prayer request with ID {prayerFulfillmentDto.prayerRequestId} does not exist.");
            }

            //created by user validation
            var createdByUserExists = await _context.Users.AnyAsync(u => u.Id == prayerFulfillmentDto.createdBy);
            if(!createdByUserExists)
            {
                return NotFound($"Created by user with ID {prayerFulfillmentDto.createdBy} does not exist.");
            }

            var prayerFulfillment = new PrayerFulfillment
            {
                prayerRequestId = prayerFulfillmentDto.prayerRequestId,
                createdBy = prayerFulfillmentDto.createdBy,
                bodyText = prayerFulfillmentDto.bodyText,
                createdAt = DateTime.UtcNow
            };

            //add to db
            _context.PrayerFulfillments.Add(prayerFulfillment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrayerFulfillmentById), new { id = prayerFulfillment.Id }, prayerFulfillment);
        }
    }
}
