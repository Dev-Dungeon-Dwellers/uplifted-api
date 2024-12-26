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
    public class PrayerRequestController : ControllerBase
    {
        private readonly GlobalService _globalService;

        private readonly UpliftedApiContext _context;
        public PrayerRequestController(UpliftedApiContext context, GlobalService globalService)
        {
            _context = context;
            _globalService = globalService;
        }

        /// <summary>
        /// Get all prayer requests
        /// </summary>
        /// <returns>All prayer requests</returns>
        //GET: api/PrayerRequest
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<PrayerRequest>>> GetPrayerRequests()
        {
            var prayerRequests = await _context.PrayerRequests.ToListAsync();
            return Ok(prayerRequests);
        }

        /// <summary>
        /// Get prayer request by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Requested prayer request</returns>
        // GET: api/PrayerRequest/1 BY ID
        [HttpGet("{id}")]
        public async Task<ActionResult<PrayerRequest>> GetPrayerRequestById(int id)
        {
            var prayerRequest = await _context.PrayerRequests.FindAsync(id);

            if (prayerRequest == null)
            {
                return NotFound($"Prayer request with ID {id} not found.");
            }

            return Ok(prayerRequest);
        }

        /// <summary>
        /// Get prayer requests by group id
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns>All prayer requests for the provided group</returns>
        //Get api/PrayerRequest?group=1
        [HttpGet("by-group")]
        public async Task<IActionResult> GetPrayerRequestsByGroupId([FromQuery] int groupid)
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

            // Fetch prayer requests for the group
            var prayerRequests = await _globalService.GetPrayerRequestsByGroupAsync(groupid);

            if (prayerRequests == null || !prayerRequests.Any())
            {
                return NotFound("No prayer requests found for the specified group.");
            }

            return Ok(prayerRequests);
        }

        /// <summary>
        /// Create new prayer request
        /// </summary>
        /// <param name="prayerRequestDto"></param>
        /// <returns>Status code from creating prayer request</returns>
        //POST: api/PrayerRequest
        [HttpPost]
        public async Task<IActionResult> CreatePrayerRequest([FromBody] CreatePrayerRequestDto prayerRequestDto)
        {
            if (prayerRequestDto == null)
            {
                return BadRequest("Prayer request data is required");
            }

            //group validation
            var groupExists = await _context.Groups.AnyAsync(g => g.Id == prayerRequestDto.GroupId);
            if (!groupExists)
            {
                return NotFound($"Group with ID {prayerRequestDto.GroupId} does not exist.");
            }

            //user validation
            var userExists = await _context.Users.AnyAsync(u => u.Id == prayerRequestDto.UserId);
            if(!userExists)
            {
                return NotFound($"User with ID {prayerRequestDto.UserId} does not exist.");
            }

            //DTO mapping
            var prayerRequest = new PrayerRequest
            {
                groupId = prayerRequestDto.GroupId,
                userId = prayerRequestDto.UserId,
                title = prayerRequestDto.Title,
                description = prayerRequestDto.Description,
                created_at = DateTime.UtcNow
            };

            //add to db
            _context.PrayerRequests.Add(prayerRequest);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPrayerRequestById), new { id = prayerRequest.Id }, prayerRequest);
        }

        //DELETE prayer request
        [HttpDelete]
        public async Task<IActionResult> DeletePrayerRequest([FromQuery] int prayerRequestId)
        {
            //validate id input
            if(prayerRequestId <= 0)
            {
                return BadRequest("Invalid prayer request ID. Must be a positive integer");
            }

            //check if prayer request exists
            var prayerRequest = await _context.PrayerRequests.FirstOrDefaultAsync(pr => pr.Id == prayerRequestId);
            if(prayerRequest == null)
            {
                return NotFound($"Prayer request with ID {prayerRequestId} not found.");
            }

            //check if there are any prayer fulfillments
            var prayerFulfillments = await _context.PrayerFulfillments.Where(pf => pf.prayerRequestId == prayerRequestId).ToListAsync();
            
            if(prayerFulfillments.Any())
            {
                //delete any prayer fulfillments
                _context.PrayerFulfillments.RemoveRange(prayerFulfillments);
            }

            //delete prayer request
            _context.PrayerRequests.Remove(prayerRequest);

            //sanity check
            try
            {
                await _context.SaveChangesAsync();
                return Ok($"Prayer request with the ID {prayerRequestId} deleted successfully");
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, $"An error occured while deleting the prayer request: {ex.Message}");
            }
        }
    }
}
