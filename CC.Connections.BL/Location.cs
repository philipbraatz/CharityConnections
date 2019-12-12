using CC.Abstract;
using CC.Connections.PL;
using System.ComponentModel;

namespace CC.Connections.BL
{
    public class Location : ColumnEntry<PL.Location>
    {
        public static CCEntities dc = new CCEntities();

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

        public Location() :
            base(new PL.Location()){ }
        public Location(PL.Location entry) :
            base(entry)
        { }
        public Location(int id) :
            base(new CCEntities().Locations,id)
        {
            Clear();
            ID = id;
            LoadId();
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
            return MapAPI.GetDistanceFromLocations(this.Full_Location,from.Full_Location);
        }

        public static double getDistanceBetween(Location start,Location end)
        {
            return MapAPI.GetDistanceFromLocations(start.Full_Location, end.Full_Location);
        }
    }

    public class LocationList : AbsList<Location, PL.Location>
    {
        public new void LoadAll()
        {
            using (CCEntities dc = new CCEntities())
            {
                base.LoadAll(dc.Locations);
            }
        }
    } 
}
