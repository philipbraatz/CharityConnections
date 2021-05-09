using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Web;
using Doorfail.Connections.BL;
using Newtonsoft.Json;

namespace Doorfail.Connections.WebUI.Model
{
    public class VolunteerSignup : Volunteer
    {
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password required")]
        public Password password { get; set; }
        [Display(Name = "Confirm new password")]
        [Required(ErrorMessage = "Enter Confirm Password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        [DataType(DataType.Password)]
        public Password confirmPassword { get; set; }
        //maybe add location 
        //and preferences here
        public VolunteerSignup()
        {
            password = new Password();

            String AccessKey = "2cfe3db40b2d3d";
            String UserIP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(UserIP))
            {
                UserIP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            //string url = "https://api.ip2location.com/v2/?ip=" + UserIP.ToString() + "&key=" + AccessKey.ToString() + "&package=WS2&addon=continent";
            //WebClient client = new WebClient();
            //string jsonstring = client.DownloadString(url);
            string jsonstring = new WebClient().DownloadString("https://ipinfo.io/"+UserIP+"?token="+AccessKey);//"https://ipinfo.io/"+ UserIP + "&token="+ AccessKey);
            dynamic dynObj = JsonConvert.DeserializeObject(jsonstring);
            Location = new Location();
            Location.City = dynObj.city;
            Location.State = dynObj.region;
        }
    }
}