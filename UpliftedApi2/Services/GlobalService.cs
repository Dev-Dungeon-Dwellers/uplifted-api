using Microsoft.EntityFrameworkCore;
using UpliftedApi2.Models;

namespace UpliftedApi2.Services
{
    public class GlobalService
    {
        private readonly UpliftedApiContext _context;

        public GlobalService(UpliftedApiContext context)
        {
            _context = context;
        }

        //PRAYER REQUEST SERVICES
        public async Task<List<PrayerRequest>> GetPrayerRequestsByGroupAsync(int groupId)
        {
            return await _context.PrayerRequests
                .Where(pr => pr.groupId == groupId)
                .ToListAsync();
        }
    }
}
