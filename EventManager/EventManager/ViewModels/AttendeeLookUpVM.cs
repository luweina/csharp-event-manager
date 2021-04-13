using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventManager.ViewModels
{
    public class AttendeeLookUpVM
    {
        public string eventName { get; set; }
        public int eventID { get; set; }
        public DateTime eventDate { get; set; }
        public string eventTime { get; set; }
        public List<UserEventVM> attendeeList { get; set; }
    }
}
