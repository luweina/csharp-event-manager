using EventManager.Data;
using EventManager.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : Controller
    {
        private ApplicationDbContext _context;

        public EventController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> OnGetAsync()
        {
            EventRepo eventRP = new EventRepo(_context);
            var query = eventRP.GetAll();
            var resultObject = new
            {
                EventArray = query
            };
            return new ObjectResult(resultObject);
        }
        [HttpGet]
        [Route("getEvent")]
        public IActionResult GetEvent(int eventID)
        {
            EventRepo eventRP = new EventRepo(_context);
            var query = eventRP.GetEvent(eventID);
            var resultObj = new
            {
                EventArray = query,
            };
            return new ObjectResult(resultObj);
        }


        [HttpGet]
        [Route("JoinEvent")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

        public IActionResult JoinEvent(int eventID)
        {
            MemberRepo memRP = new MemberRepo(_context);
            EventRepo eventRP = new EventRepo(_context);

            var claim = HttpContext.User.Claims.ElementAt(0);
            string email = claim.Value;
            var member = memRP.GetOneByEmail(email);

            try
            {
                eventRP.JoinEvent(eventID, member.Id);

                var responseObject = new
                {
                    StatusCode = "Added to the list",
                };
                return new ObjectResult(responseObject);

            }
            catch
            {
                var responseObject = new
                {
                    error = "can't added again",
                };
                return new ObjectResult(responseObject);
            }


        }

        [HttpGet]
        [Route("leaveEvent")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult leaveEvent(int eventID)
        {
            MemberRepo memRP = new MemberRepo(_context);
            EventRepo eventRP = new EventRepo(_context);
            var claim = HttpContext.User.Claims.ElementAt(0);
            string email = claim.Value;
            var member = memRP.GetOneByEmail(email);
            try
            {
                eventRP.leaveEvent(eventID, member.Id);

                var responseObject = new
                {
                    StatusCode = "removed from guest list",
                };
                return new ObjectResult(responseObject);

            }
            catch
            {
                var responseObject = new
                {
                    error = "Error",
                };
                return new ObjectResult(responseObject);
            }


        }

        [HttpGet]
        [Route("MyEvent")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult MyEvent()
        {
            UserEventRepo ueRP = new UserEventRepo(_context);
            MemberRepo cRP = new MemberRepo(_context);
            var claim = HttpContext.User.Claims.ElementAt(0);
            string email = claim.Value;
            var user = cRP.GetOneByEmail(email);
            var query = ueRP.GetMemberEvents(user.Id);
            var responseObject = new
            {
                EventArray = query,
            };
            return new ObjectResult(responseObject);
        }

        [HttpGet]
        [Route("GetAttendees")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult GetAttendees(int eventID)
        {
            UserEventRepo ueRP = new UserEventRepo(_context);
            var query = ueRP.AttendeeLookUp(eventID);
            var responseObject = new
            {
                EventArray = query,
            };
            return new ObjectResult(responseObject);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
        public async Task<IActionResult> OnPostAsync([FromBody] Event Event)
        {
            if (ModelState.IsValid)
            {
                EventRepo eventRP = new EventRepo(_context);
                var result = eventRP.Create(Event.EventName, Event.Description, Event.Date, Event.Time);
                if (result)
                {
                    var resultOBJ = new
                    {
                        StatusCode = "Ok",
                        EventName = Event.EventName
                    };

                    return new ObjectResult(resultOBJ);
                }
            }

            var InvalidObj = new
            {
                StatusCode = "Invalid",
                EventName = Event.EventName
            };

            return new ObjectResult(InvalidObj);

        }


        [HttpGet]
        [Route("Delete")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin,Manager")]
        public IActionResult Delete(int eventID)
        {
            try
            {
                EventRepo eventRP = new EventRepo(_context);
                eventRP.Delete(eventID);
                var Obj = new
                {
                    StatusCode = "ok",
                    message = "Deleted"
                };

                return new ObjectResult(Obj);
            }
            catch
            {
                var Obj = new
                {
                    StatusCode = "error",
                    message = "Somebody in the event."
                };

                return new ObjectResult(Obj);
            }

        }

    }
}
