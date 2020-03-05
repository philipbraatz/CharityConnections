using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Model
{
    public class CharityEventWithTime : CharityEvent
    {
        private string time_start;//"12/11/2019 12:00:00 AM"
        private string time_end;

        public CharityEventWithTime() {
            this.time_start = TimeUtils.ToInt(this.StartTime).ToString();
            this.time_end = TimeUtils.ToInt(this.EndTime).ToString();
        }
        public CharityEventWithTime(CharityEvent evnt)
        {

            this.ID = evnt.ID;
            this.CharityEmail = evnt.CharityEmail;
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