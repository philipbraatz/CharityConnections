using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Connections.BL
{
    public class Location
    {
        private int? location_ID;

        public Location(int? location_ID)
        {
            this.location_ID = location_ID;
        }

        public int ID { get; internal set; }
        [DisplayName("Address")]
        public string Address { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [DisplayName("State")]
        public string State { get; set; }
        [DisplayName("Zip")]
        public string Zip { get; set; }
        [DisplayName("Location")]
        public string Full_Location
        {
            //"825 Pilgrim Way, Green Bay, WI"
            get { return Address + " ," + City + " ," + State; }
        }
    }
}
