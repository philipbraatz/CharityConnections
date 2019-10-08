using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CC.Connections.BL;

namespace CC.Connections.WebUI.Model
{
    public class ContactInfoSignup : ContactInfo
    {
        public Password password { get; set; }
        //maybe add location 
        //and preferences here
        public ContactInfoSignup()
        {
            password = new Password();
        }
    }
}