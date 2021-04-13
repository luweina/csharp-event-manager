using EventManager.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Repositories
{
    public class EventRepo
    {
        ApplicationDbContext db;

        public EventRepo(ApplicationDbContext context)
        {
            db = context;
        }

        public IQueryable<Event> GetAll()
        {
            var query = from e in db.Events
                        select new Event()
                        {
                            EventName = e.EventName,
                            Description = e.Description,
                            Date = e.Date,
                            Time = e.Time,
                            ID = e.ID
                        };

            return query;
        }

        public Event GetEvent(int eventID)
        {
            var query = GetAll().Where(x => x.ID == eventID).FirstOrDefault();
            return query;
        }


        public bool Create(string eventName, string description, DateTime date, string time)
        {
            Event newEvent = new Event()
            {
                EventName = eventName,
                Description = description,
                Date = date,
                Time = time
            };

            db.Events.Add(newEvent);
            db.SaveChanges();

            return true;
        }

        public bool Delete(int eventID)
        {
            Event Event = db.Events.Where(e => e.ID == eventID).FirstOrDefault();
            db.Remove(Event);
            db.SaveChanges();
            return true;
        }

        public bool JoinEvent(int eventID, int memberId)
        {
            UserEvent joinEvent = new UserEvent { MemberId = memberId, EventID = eventID };
            db.UserEvents.Add(joinEvent);
            db.SaveChanges();
            return true;
        }

        public bool leaveEvent(int eventID, int userID)
        {
            UserEvent joinEvent = db.UserEvents.Where(ue => ue.MemberId == userID && ue.EventID == eventID).FirstOrDefault();
            db.Remove(joinEvent);
            db.SaveChanges();
            return true;
        }
    }
}
