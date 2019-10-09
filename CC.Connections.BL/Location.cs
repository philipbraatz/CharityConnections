using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Connections.BL
{
    public class BLLocation : PL.Location
    {
        [DisplayName("Address")]
        public new string ContactInfoAddress { get; set; }
        [DisplayName("City")]
        public new string ContactInfoCity { get; set; }
        [DisplayName("State")]
        public new string ContactInfoState { get; set; }
        [DisplayName("Zip")]
        public new string ContactInfoZip { get; set; }
        [DisplayName("Location")]
        public string Full_Location
        {
            //"825 Pilgrim Way, Green Bay, WI"
            get { return ContactInfoAddress + " ," + ContactInfoCity + " ," + ContactInfoState; }
        }

        public BLLocation() { }
        public BLLocation(int location_ID)
        {
            this.Location_ID = location_ID;
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
                        Location_ID = dc.Locations.Max(c => c.Location_ID) + 1;//unique id
                    else
                        Location_ID = 0;

                    PL.Location entry = new PL.Location
                    {
                        Location_ID = Location_ID,
                        ContactInfoAddress = ContactInfoAddress,
                        ContactInfoCity = ContactInfoCity,
                        ContactInfoState = ContactInfoState,
                        ContactInfoZip = ContactInfoZip
                    };

                    dc.Locations.Add(entry);
                    dc.SaveChanges();
                    return Location_ID;
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

                    dc.Locations.Remove(dc.Locations.Where(c => c.Location_ID == Location_ID).FirstOrDefault());
                    this.ContactInfoAddress = string.Empty;
                    this.ContactInfoCity = string.Empty;
                    this.ContactInfoState = string.Empty;
                    this.ContactInfoZip = string.Empty;
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

                    PL.Location entry = dc.Locations.Where(c => c.Location_ID == this.Location_ID).FirstOrDefault();
                    entry.Location_ID = Location_ID;
                    entry.ContactInfoAddress = ContactInfoAddress;
                    entry.ContactInfoCity = ContactInfoCity;
                    entry.ContactInfoState = ContactInfoState;
                    entry.ContactInfoZip = ContactInfoZip;

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
                    PL.Location entry = dc.Locations.FirstOrDefault(c => c.Location_ID == this.Location_ID);
                    if (entry == null)
                        throw new Exception("Location does not exist: Key '" + this.Location_ID + "'"); ;

                    Location_ID = (int)entry.Location_ID;
                    ContactInfoAddress = entry.ContactInfoAddress;
                    ContactInfoCity = entry.ContactInfoCity;
                    ContactInfoState = entry.ContactInfoState;
                    ContactInfoZip = entry.ContactInfoZip;        
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public double distanceFrom(BLLocation from)
        {
            return MapAPI.GetDistanceFromLocations(this.Full_Location,from.Full_Location);
        }

        public static double getDistanceBetween(BLLocation start,BLLocation end)
        {
            return MapAPI.GetDistanceFromLocations(start.Full_Location, end.Full_Location);
        }
    }

    public class LocationList
        : List<BLLocation>
    {
        public void Load()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    dc.Locations.ToList().ForEach(c => this.Add(new BLLocation
                    {
                        Location_ID = (int)c.Location_ID,
                        ContactInfoAddress = c.ContactInfoAddress,
                        ContactInfoCity = c.ContactInfoCity,
                        ContactInfoState = c.ContactInfoState,
                        ContactInfoZip = c.ContactInfoZip,
                }));
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
