using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Model
{
    public class CharityEvent_WithTime : CharityEvent
    {
        private string time_start;
        private string time_end;

        public CharityEvent_WithTime() { }
        public CharityEvent_WithTime(CharityEvent evnt)
        {
            this.Event_ID = evnt.Event_ID;
            this.Charity_ID = evnt.Charity_ID;
            this.setEventInfo(evnt);
            this.time_start = this.StartTime.ToShortTimeString();
            this.time_end = this.EndTime.ToShortTimeString();
        }

        public string strTimeStart
        {
            get { return time_start; }
            set
            {
                time_start = value;
                StartTime = TimeUtils.ToTime(int.Parse(value));
            }
        }

        public string strTimeEnd
        {
            get { return time_end; }
            set
            {
                time_end = value;
                EndTime = TimeUtils.ToTime(int.Parse(value));
            }
        }
    }
}