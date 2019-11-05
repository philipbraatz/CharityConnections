using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Model
{
    public class CharityEvent_with_Charity :CharityEvent
    {

        public Charity Charity { get; set; }
        [DisplayName("Charity")]
        public string CharityName { get; set; }

        public CharityEvent_with_Charity(CharityEvent evnt)
        {
            this.setEventInfo(evnt);
            setContactInfo(new AbsContact(evnt.ContactInfo_Email));
            Charity = new Charity(evnt.Charity_ID);
            CharityName = Charity.FullName;
        }


    }
}