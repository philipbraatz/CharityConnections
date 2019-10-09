using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CC.Connections.BL
{
    public class BLContactInfo : PL.Contact_Info
    {
        //[DisplayName("Email")]
        //public new string ContactInfo_Email { get; set; }
        //[DisplayName("First Name")]
        //public new string ContactInfo_FName { get; set; }
        //[DisplayName("Last Name")]
        //public new string ContactInfo_LName { get; set; }
        //[DisplayName("Phone #")]
        //public new string ContactInfo_Phone { get; set; }
        //[DisplayName("Birth Date")]
        //public new DateTime DateOfBirth { get; set; }

        //New values

        [DisplayName("Name")]
        public string FullName
        {
            get { return ContactInfo_FName + " " + ContactInfo_LName; }
        }

        public BLContactInfo() { Clear(); }
        public BLContactInfo(string email)
        {
            ContactInfo_Email = email;
            LoadId();
        }
        private PL.Contact_Info toPL()
        {
            return new Contact_Info
            {
                ContactInfo_Email   = ContactInfo_Email,
                ContactInfo_FName   = ContactInfo_FName,
                ContactInfo_LName   = ContactInfo_LName,
                ContactInfo_Phone   = ContactInfo_Phone,
                Contact_Info_ID     = Contact_Info_ID,
                DateOfBirth         = DateOfBirth,
                Location_ID         = Location_ID,
            };
            
        }

        protected void Clear()
        {
            ContactInfo_Email =string.Empty;
            ContactInfo_FName = string.Empty;
            ContactInfo_LName = string.Empty;
            ContactInfo_Phone = string.Empty;
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
                if (ContactInfo_Email == string.Empty)
                    throw new Exception("Email cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    //Member already exists
                    if (dc.Contact_Info.Where(c => c.ContactInfo_Email == ContactInfo_Email).Count() != 0)
                        throw new Exception("Email "+ContactInfo_Email + " is already in use, please use a different one");
                    if (dc.Contact_Info.ToList().Count > 0)
                        Contact_Info_ID = dc.Contact_Info.Max(c => c.Contact_Info_ID) + 1;//unique id
                    else
                        Contact_Info_ID = 0;
                    dc.Contact_Info.Add(this.toPL());
                    dc.SaveChanges();
                    return Contact_Info_ID;
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
                    if (this.ContactInfo_Email == string.Empty)
                        throw new Exception("Email is invaild");

                    dc.Contact_Info.Remove(this.toPL());
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        public int Update()
        {
            try
            {
                if (ContactInfo_Email == string.Empty)
                    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Contact_Info entry = dc.Contact_Info.Where(c => c.ContactInfo_Email == this.ContactInfo_Email).FirstOrDefault();
                    entry.ContactInfo_Email = ContactInfo_Email;
                    entry.ContactInfo_FName = this.ContactInfo_FName;
                    entry.ContactInfo_LName = this.ContactInfo_LName;
                    entry.ContactInfo_Phone = this.ContactInfo_Phone;
                    entry.DateOfBirth       = this.DateOfBirth;

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        //sets email then loads
        public void LoadId(string email)
        {
            this.ContactInfo_Email = email;
            LoadId();
        }
        //loads from pre-set email
        public void LoadId()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    if (this.ContactInfo_Email == string.Empty)
                        throw new Exception("Email is not set");
                    PL.Contact_Info entry = dc.Contact_Info.FirstOrDefault(c => c.ContactInfo_Email == this.ContactInfo_Email);
                    if (entry == null)
                        throw new Exception("Contact_Info does not exist: Email " + this.ContactInfo_Email ) ;
                    this.Contact_Info_ID    = entry.Contact_Info_ID;
                    this.ContactInfo_Email  = entry.ContactInfo_Email;
                    this.ContactInfo_FName  = entry.ContactInfo_FName;
                    this.ContactInfo_LName  = entry.ContactInfo_LName;
                    this.ContactInfo_Phone  = entry.ContactInfo_Phone;
                    this.DateOfBirth        = (DateTime)entry.DateOfBirth;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //TODO depreciate
        internal static BLContactInfo fromNumID(int? memberContact_ID)
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");
                    BLContactInfo ret = new BLContactInfo();
                    PL.Contact_Info entry = dc.Contact_Info.FirstOrDefault(c => c.Contact_Info_ID == memberContact_ID);
                    if (entry == null)
                        throw new Exception("Contact Info does not exist: Email '" + entry.ContactInfo_Email + "'");
                    return (BLContactInfo)entry;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class ContactList
        : List<BLContactInfo>
    {
        public void Load()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    dc.Contact_Info.ToList().ForEach(c => this.Add(new BLContactInfo
                    {
                        ContactInfo_Email = c.ContactInfo_Email,
                        ContactInfo_FName = c.ContactInfo_FName,
                        ContactInfo_LName = c.ContactInfo_LName,
                        ContactInfo_Phone = c.ContactInfo_Phone,
                        DateOfBirth       = (DateTime)c.DateOfBirth
                }));
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
