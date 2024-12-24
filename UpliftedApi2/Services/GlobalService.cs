using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices.Marshalling;
using System.Text.RegularExpressions;
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

        internal async Task<List<PrayerFulfillment>> GetPrayerFulfillmentsByGroupAsync(int groupid)
        {
            return await _context.PrayerFulfillments
                .Where(pf => pf.myPrayerRequest.groupId == groupid)
                .ToListAsync();
        }

        internal async Task<List<PrayerFulfillment>> GetPrayerFulfillmentByUserAsync(int userid)
        {
            return await _context.PrayerFulfillments
                .Where(pf => pf.myPrayerRequest.userId == userid)
                .ToListAsync();
        }

        internal async Task<List<PrayerFulfillment>> GetPrayerFulfillmentByPrayerRequestAsync(int prayerrequestid)
        {
            return await _context.PrayerFulfillments
                .Where(pf => pf.myPrayerRequest.Id == prayerrequestid)
                .ToListAsync();
        }

        internal async Task<List<PrayerFulfillment>> GetPrayerFulfillmentByCreatedByUserAsync(int createdbyuserid)
        {
            return await _context.PrayerFulfillments
                .Where(cbu => cbu.myCreatedBy.Id == createdbyuserid)
                .ToListAsync();
        }

        internal async Task<List<UserGroupMapping>> GetUserGroupMappingsByIdAsync(int usergroupmappingid)
        {
            return await _context.UserGroupMappings
                .Where(ugm => ugm.id == usergroupmappingid)
                .ToListAsync();
        }
    }
}
