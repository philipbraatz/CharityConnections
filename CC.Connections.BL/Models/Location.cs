using CC.DataConnection;
using CC.Connections.PL;
using System;
using System.ComponentModel;

namespace CC.Connections.BL
{
    public class Location : CrudModel_Json<PL.Location>
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
            base(JsonDatabase.Locations,id)
        {
        }

        public static implicit operator Location(PL.Location entry)
        { return new Location(entry); }

        public int Insert(){
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    return base.Insert(dc, dc.Locations);
                }
            else
                base.Insert(JsonDatabase.Locations);
            return 1;
        }
        public int Delete(){
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    return base.Delete(dc, dc.Locations);
                }
            else
                base.Delete(JsonDatabase.Locations);
            return 1;
        }

        public int Update(){
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    return base.Update(dc, dc.Locations);
                }
            else
                base.Update(JsonDatabase.Locations);
            return 1;
        }

        public void LoadId() {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.LoadId(JsonDatabase.Locations);
                }
            else
                base.LoadId(JsonDatabase.Locations);
        }

        public double distanceFrom(Location from)
        {
            return MapAPI.GetDistanceFromLocations(this.FullLocation,from.FullLocation);
        }

        public static double getDistanceBetween(Location start,Location end)
        {
            return MapAPI.GetDistanceFromLocations(start.FullLocation, end.FullLocation);
        }
    }

    public class LocationCollection : CrudModelList<Location, PL.Location>
    {
        public void LoadAll()
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.LoadAll(JsonDatabase.Locations);
                }
            else
                base.LoadAll(JsonDatabase.Locations);
        }
    } 
}
