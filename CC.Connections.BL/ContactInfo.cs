using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CC.Connections.BL
{
    public class ContactInfo
    {
        public int ID { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("First Name")]
        public string FirstName { get; set; }
        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string Phone { get; set; }
        [DisplayName("Name")]
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        [DisplayName("Location")]
        public Location Location { get; set; }
        public DateTime BirthDate { get; set; }

        public ContactInfo(){ clear(); }

        private void clear()
        {
            Email =string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            Phone = string.Empty;
            Location = new Location();
        }

        public ContactInfo(string email)
        {
            Email = email;
            LoadId();
        }
        
        //public ContactInfo(int? contactID)
        //{
        //    this.ID = (int)contactID;
        //    LoadId();
        //}

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
                    if (dc.Contact_Info.ToList().Count > 0)
                        ID = dc.Contact_Info.Max(c => c.Contact_Info_ID) + 1;//unique id
                    else
                        ID = 0;

                    PL.Contact_Info entry = new PL.Contact_Info
                    {
                        Contact_Info_ID = ID,
                        ContactInfo_Email = Email,
                        ContactInfo_FName = this.FirstName,
                        ContactInfo_LName =this.LastName,
                        Location_ID =this.Location.ID,
                        ContactInfo_Phone =this.Phone,
                        DateOfBirth =this.BirthDate
                    };

                    dc.Contact_Info.Add(entry);
                    return dc.SaveChanges();
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
                    if (this.Email == string.Empty)
                        throw new Exception("Email is invaild");

                    dc.Contact_Info.Remove(dc.Contact_Info.Where(c => c.ContactInfo_Email == Email).FirstOrDefault());
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        public int Update()
        {
            try
            {
                if (Email == string.Empty)
                    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Contact_Info entry = dc.Contact_Info.Where(c => c.ContactInfo_Email == this.Email).FirstOrDefault();
                    entry.ContactInfo_Email = Email;
                    entry.ContactInfo_FName = this.FirstName;
                    entry.ContactInfo_LName = this.LastName;
                    entry.Location_ID = this.Location.ID;
                    entry.ContactInfo_Phone = this.Phone;
                    entry.DateOfBirth = this.BirthDate;

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
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    if (this.Email == string.Empty)
                        throw new Exception("Email is not set");
                    PL.Contact_Info entry = dc.Contact_Info.FirstOrDefault(c => c.ContactInfo_Email == this.Email);
                    if (entry == null)
                        throw new Exception("Contact_Info does not exist: Key '" + this.Email + "'"); ;

                    this.Email = entry.ContactInfo_Email;
                    this.FirstName = entry.ContactInfo_FName;
                    this.LastName = entry.ContactInfo_LName;
                    this.Location = new Location(entry.Location_ID);
                    this.Phone = entry.ContactInfo_Phone;
                    this.BirthDate = (DateTime)entry.DateOfBirth;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //TODO depreciate
        internal static ContactInfo fromNumID(int? memberContact_ID)
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");
                    ContactInfo ret = new ContactInfo();
                    if (ret.Email == string.Empty)
                        throw new Exception("Email is not set");
                    PL.Contact_Info entry = dc.Contact_Info.FirstOrDefault(c => c.Contact_Info_ID == memberContact_ID);
                    if (entry == null)
                        throw new Exception("Contact_Info does not exist: Key '" + ret.Email + "'"); ;

                    ret.Email = entry.ContactInfo_Email;
                    ret.FirstName = entry.ContactInfo_FName;
                    ret.LastName = entry.ContactInfo_LName;
                    ret.Location = new Location(entry.Location_ID);
                    ret.Phone = entry.ContactInfo_Phone;
                    ret.BirthDate = (DateTime)entry.DateOfBirth;
                    return ret;
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
                        Email = c.ContactInfo_Email,
                        FirstName = c.ContactInfo_FName,
                        LastName = c.ContactInfo_LName,
                        Phone = c.ContactInfo_Phone,
                        BirthDate =(DateTime)c.DateOfBirth
                }));
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
