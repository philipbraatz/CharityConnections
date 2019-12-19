using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Model
{
    public class CharityEvent_WithTime : CharityEvent
    {
        private string time_start;//"12/11/2019 12:00:00 AM"
        private string time_end;

        public CharityEvent_WithTime() {
            this.time_start = TimeUtils.ToInt(this.StartTime).ToString();
            this.time_end = TimeUtils.ToInt(this.EndTime).ToString();
        }
        public CharityEvent_WithTime(CharityEvent evnt)
        {

            this.Event_ID = evnt.Event_ID;
            this.Charity_ID = evnt.Charity_ID;
            this.setEventInfo(evnt);//charity id is set
            this.time_start = TimeUtils.ToInt(this.StartTime).ToString();
            this.time_end = TimeUtils.ToInt(this.EndTime).ToString();
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