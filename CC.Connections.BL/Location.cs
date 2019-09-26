using CC.Connections.PL;
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

        public Location() { }
        public Location(int location_ID)
        {
            this.ID = location_ID;
            LoadId();
        }

        public int Insert()
        {
            try
            {
                //if (Email == string.Empty)
                //    throw new Exception("Email cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    //if (dc.Locations.Where(c => c.Location_ID == ID).Count() != 0)
                    //    throw new Exception(Email + " is already in use, please use a different one");
                    if (dc.Locations.ToList().Count > 0)
                        ID = dc.Locations.Max(c => c.Location_ID) + 1;//unique id
                    else
                        ID = 0;

                    PL.Location entry = new PL.Location
                    {
                        Location_ID = ID,
                        ContactInfoAddress = Address,
                        ContactInfoCity = City,
                        ContactInfoState = State,
                        ContactInfoZip = Zip
                    };

                    dc.Locations.Add(entry);
                    dc.SaveChanges();
                    return ID;
                }
            }
            catch (Exception e) { throw e; }
        }
        public int Delete()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == string.Empty)
                    //    throw new Exception("Email is invaild");

                    dc.Locations.Remove(dc.Locations.Where(c => c.Location_ID == ID).FirstOrDefault());
                    this.Address = string.Empty;
                    this.City = string.Empty;
                    this.State = string.Empty;
                    this.Zip = string.Empty;
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        public int Update()
        {
            try
            {
                //if (Email == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Location entry = dc.Locations.Where(c => c.Location_ID == this.ID).FirstOrDefault();
                    entry.Location_ID = ID;
                    entry.ContactInfoAddress = Address;
                    entry.ContactInfoCity = City;
                    entry.ContactInfoState = State;
                    entry.ContactInfoZip = Zip;

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        public void LoadId()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.Email == string.Empty)
                    //    throw new Exception("Email is not set");
                    PL.Location entry = dc.Locations.FirstOrDefault(c => c.Location_ID == this.ID);
                    if (entry == null)
                        throw new Exception("Location does not exist: Key '" + this.ID + "'"); ;

                    ID = (int)entry.Location_ID;
                    Address = entry.ContactInfoAddress;
                    City = entry.ContactInfoCity;
                    State = entry.ContactInfoState;
                    Zip = entry.ContactInfoZip;        
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public double distanceFrom(Location from)
        {
            return MapAPI.GetDistanceFromLocations(this.Full_Location,from.Full_Location);
        }

        public static double getDistanceBetween(Location start,Location end)
        {
            return MapAPI.GetDistanceFromLocations(start.Full_Location, end.Full_Location);
        }
    }

    public class LocationList
        : List<Location>
    {
        public void Load()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    dc.Locations.ToList().ForEach(c => this.Add(new Location
                    {
                        ID = (int)c.Location_ID,
                        Address = c.ContactInfoAddress,
                        City = c.ContactInfoCity,
                        State = c.ContactInfoState,
                        Zip = c.ContactInfoZip,
                }));
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
