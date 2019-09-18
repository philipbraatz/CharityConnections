using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class ContactInfo
    {
        private int? contactID;

        public int ID { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        [DisplayName("Address")]
        public string Address { get; set; }
        [DisplayName("City")]
        public string City { get; set; }
        [DisplayName("State")]
        public string State { get; set; }
        [DisplayName("Zip")]
        public string Zip { get; set; }
        [DisplayName("Phone Number")]
        public string Phone { get; set; }
        [DisplayName("Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        [DisplayName("Location")]
        public string Location
        {
            //"825 Pilgrim Way, Green Bay, WI"
            get { return Address + " ,"+City+" ,"+State; }
        }

        public ContactInfo()
        { }
        public ContactInfo(string email)
        {
            Email = email;
            LoadId();
        }

        public ContactInfo(int? contactID)
        {
            this.contactID = contactID;
        }

        internal void Insert(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        //public ContactInfo(string email,string firstname,string lastname,
        //    string address, string city, string state,string zip,
        //    string phonenumber )
        //{
        //    Email = email;
        //    FirstName = firstname;
        //    LastName = lastname;
        //    Address = address;
        //    City = city;
        //    State = state;
        //    Zip = zip;
        //    Phone = phonenumber;
        //}

        public int Insert()
        {
            try
            {
                if (Email == string.Empty)
                    throw new Exception("Email cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    PL.Contact_Info entry = new PL.Contact_Info
                    {
                        Contact_Info_Email = Email,
                        Contact_Info_FName = this.FirstName,
                        Contact_Info_LName =this.LastName,
                        Contact_Info_Address =this.Address,
                        Contact_Info_City =this.City,
                        Contact_Info_State =this.State,
                        Contact_Info_Zip =this.Zip,
                        Contact_Info_Phone =this.Phone
                    };

                    dc.Contact_Info.Add(entry);
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public int Delete(DBconnections dc)
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    if (this.Email == string.Empty)
                        throw new Exception("Email is invaild");

                    dc.Contact_Info.Remove(dc.Contact_Info.Where(c => c.Contact_Info_Email == Email).FirstOrDefault());
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        internal void Delete(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        public int Update(DBconnections dc)
        {
            try
            {
                if (Email == string.Empty)
                    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Contact_Info entry = dc.Contact_Info.Where(c => c.Contact_Info_Email == this.Email).FirstOrDefault();
                    entry.Contact_Info_Email = Email;
                    entry.Contact_Info_FName = this.FirstName;
                    entry.Contact_Info_LName = this.LastName;
                    entry.Contact_Info_Address = this.Address;
                    entry.Contact_Info_City = this.City;
                    entry.Contact_Info_State = this.State;
                    entry.Contact_Info_Zip = this.Zip;
                    entry.Contact_Info_Phone = this.Phone;

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        internal void Update(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        public void LoadId()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Contact_Info entry = dc.Contact_Info.FirstOrDefault(c => c.Contact_Info_Email == this.Email);
                    if (entry == null)
                        throw new Exception("Genre does not exist");

                    this.Email = entry.Contact_Info_Email;
                    this.FirstName = entry.Contact_Info_FName;
                    this.LastName = entry.Contact_Info_LName;
                    this.Address = entry.Contact_Info_Address;
                    this.City = entry.Contact_Info_City;
                    this.State = entry.Contact_Info_State;
                    this.Zip = entry.Contact_Info_Zip;
                    this.Phone = entry.Contact_Info_Phone;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class ContactList
        : List<ContactInfo>
    {
        public void Load()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    dc.Contact_Info.ToList().ForEach(c => this.Add(new ContactInfo
                    {
                        Email = c.Contact_Info_Email,
                        FirstName = c.Contact_Info_FName,
                        LastName = c.Contact_Info_LName,
                        Address = c.Contact_Info_Address,
                        City = c.Contact_Info_City,
                        State = c.Contact_Info_State,
                        Zip = c.Contact_Info_Zip,
                        Phone = c.Contact_Info_Phone
                }));
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
