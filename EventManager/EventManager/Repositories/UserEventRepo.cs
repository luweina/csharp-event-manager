using EventManager.Data;
using EventManager.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Repositories
{
    public class UserEventRepo
    {
        ApplicationDbContext db;

        public UserEventRepo(ApplicationDbContext context)
        {
            db = context;
        }
        public IQueryable<UserEventVM> GetAll()
        {
            var query = from ue in db.UserEvents
                        from mem in db.Members
                        where ue.MemberId == mem.Id
                        select new
                        {
                            memberId = mem.Id,
                            userName = mem.firstName + " " + mem.lastName,
                            eventID = ue.EventID
                        };
            var join = from Ev in db.Events
                         from q in query
                         where Ev.ID == q.eventID
                         select new UserEventVM()
                         {
                             userName = q.userName,
                             MemberID = q.memberId,
                             EventID = Ev.ID,
                             Date = Ev.Date,
                             Time = Ev.Time,
                             Description = Ev.Description,
                             EventName = Ev.EventName
                         };

            return join;

        }
        public IQueryable<UserEventVM> GetMemberEvents(int memberId)
        {
            var query = GetAll().Where(q => q.MemberID == memberId);
            return query;
        }
        public AttendeeLookUpVM AttendeeLookUp(int eventID)
        {
            var all = GetAll();
            var query = all.AsEnumerable()
                .GroupBy(x => new { x.EventName, x.EventID, x.Date, x.Time })
                .Select(x => new AttendeeLookUpVM()
                {
                    eventName = x.Key.EventName,
                    eventID = x.Key.EventID,
                    eventDate = x.Key.Date,
                    eventTime = x.Key.Time,
                    attendeeList = x.ToList()
                }
                );
            var filter = query.Where(x => x.eventID == eventID).FirstOrDefault();
            return filter;
        }
    }
}
