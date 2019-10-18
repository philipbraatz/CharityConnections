using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Connections.BL
{
    public class AbsLocation : ColumnEntry<PL.Location>
    {
        public static fvtcEntities dc = new fvtcEntities();

        //id
        public new int ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }
        [DisplayName("Address")]
        public new string ContactInfoAddress {
            get { return (string)base.getProperty("ContactInfoAddress"); }
            set { setProperty("ContactInfoAddress", value); }
        }
        [DisplayName("City")]
        public new string ContactInfoCity {
            get { return (string)base.getProperty("ContactInfoCity"); }
            set { setProperty("ContactInfoCity", value); }
        }
        [DisplayName("State")]
        public new string ContactInfoState {
            get { return (string)base.getProperty("ContactInfoState"); }
            set { setProperty("ContactInfoState", value); }
        }
        [DisplayName("Zip")]
        public new string ContactInfoZip {
                        get { return (string)base.getProperty("ContactInfoZip"); }
            set { setProperty("ContactInfoZip", value); }
        }
        [DisplayName("Location")]
        public string Full_Location
        {
            //"825 Pilgrim Way, Green Bay, WI"
            get { return ContactInfoAddress + " ," + ContactInfoCity + " ," + ContactInfoState; }
        }

        public AbsLocation() :
            base(new Location()){ }
        public AbsLocation(PL.Location entry) :
            base(entry)
        { }
        public AbsLocation(int id) :
            base(new fvtcEntities().Locations,id)
        {
            Clear();
            ID = id;
            LoadId();
        }

        public static implicit operator AbsLocation(PL.Location entry)
        { return new AbsLocation(entry); }

        public int Insert(){
            using (fvtcEntities dc = new fvtcEntities()){
                return base.Insert(dc, dc.Locations);
        }}
        public int Delete(){
            using (fvtcEntities dc = new fvtcEntities()){
                return base.Delete(dc, dc.Locations);
        }}

        public int Update(){
            using (fvtcEntities dc = new fvtcEntities()){
                return base.Update(dc, dc.Locations);
        }}

        public void LoadId() {
            using (fvtcEntities dc = new fvtcEntities()){
                base.LoadId(dc.Locations);
        }}

        public double distanceFrom(AbsLocation from)
        {
            return MapAPI.GetDistanceFromLocations(this.Full_Location,from.Full_Location);
        }

        public static double getDistanceBetween(AbsLocation start,AbsLocation end)
        {
            return MapAPI.GetDistanceFromLocations(start.Full_Location, end.Full_Location);
        }
    }

    public class AbsLocationList : AbsList<AbsLocation, Location>
    {
        public new void LoadAll()
        {
            using (fvtcEntities dc = new fvtcEntities())
            {
                base.LoadAll(dc.Locations);
            }
        }
    } 
}
