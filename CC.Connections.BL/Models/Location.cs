using CC.Abstract;
using CC.Connections.PL;
using System;
using System.ComponentModel;

namespace CC.Connections.BL
{
    public class Location : BaseModel<PL.Location>
    {
        //public static CCEntities dc = new CCEntities();

        //id
        public new Guid ID
        {
            get { return (Guid)base.ID; }
            set { base.ID = value; }
        }
        [DisplayName("Address")]
        public string Address {
            get { return (string)base.getProperty(nameof(Address)); }
            set { setProperty(nameof(Address), value); }
        }
        [DisplayName("City")]
        public string City {
            get { return (string)base.getProperty(nameof(City)); }
            set { setProperty(nameof(City), value); }
        }
        [DisplayName("State")]
        public string State {
            get { return (string)base.getProperty(nameof(State)); }
            set { setProperty(nameof(State), value); }
        }
        [DisplayName("Zip")]
        public string Zip {
                        get { return (string)base.getProperty(nameof(Zip)); }
            set { setProperty(nameof(Zip), value); }
        }
        [DisplayName("Location")]
        public string FullLocation
        {
            //"825 Pilgrim Way, Green Bay, WI"
            get { return Address + " ," + City + " ," + State; }
        }

        public Location() :
            base(new PL.Location()){ }
        public Location(PL.Location entry) :
            base(entry)
        { }
        public Location(Guid id) :
            base(new CCEntities().Locations,id)
        {
        }

        public static implicit operator Location(PL.Location entry)
        { return new Location(entry); }

        public int Insert(){
            using (CCEntities dc = new CCEntities()){
                return base.Insert(dc, dc.Locations);
        }}
        public int Delete(){
            using (CCEntities dc = new CCEntities()){
                return base.Delete(dc, dc.Locations);
        }}

        public int Update(){
            using (CCEntities dc = new CCEntities()){
                return base.Update(dc, dc.Locations);
        }}

        public void LoadId() {
            using (CCEntities dc = new CCEntities()){
                base.LoadId(dc.Locations);
        }}

        public double distanceFrom(Location from)
        {
            return MapAPI.GetDistanceFromLocations(this.FullLocation,from.FullLocation);
        }

        public static double getDistanceBetween(Location start,Location end)
        {
            return MapAPI.GetDistanceFromLocations(start.FullLocation, end.FullLocation);
        }
    }

    public class LocationCollection : BaseList<Location, PL.Location>
    {
        public void LoadAll()
        {
            using (CCEntities dc = new CCEntities())
            {
                base.LoadAll(dc.Locations);
            }
        }
    } 
}
