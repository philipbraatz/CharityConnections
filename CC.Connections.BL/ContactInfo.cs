using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

// 30 min
namespace CC.Connections.BL
{
    public class AbsContactInfo : ColumnEntry<PL.Contact_Info>
    {
        private static DBconnections dc;

        //real contact_Info_ID
        public int contact_ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }
        //hide ID to rename it to contact_ID
        [Obsolete("This property has been replaced with contact_ID and should no longer be used.", true)]
        public new const object ID = null;//if you try to access this id it will alway be null
        //{
        //    get => throw new Exception("ID does not exist, use contact_ID instead");//if not const it could throw errors instead
        //    set => throw new Exception("ID does not exist, use contact_ID instead");
        //}


        [DisplayName("Email")]
        public new string ContactInfo_Email
        {
            get { return (string)base.getProperty("ContactInfo_Email"); }
            set { setProperty("ContactInfo_Email", value); }
        }
        [DisplayName("First Name")]
        public new string ContactInfo_FName
        {
            get { return (string)base.getProperty("ContactInfo_FName"); }
            set { setProperty("ContactInfo_FName", value); }
        }
        [DisplayName("Last Name")]
        public new string ContactInfo_LName
        {
            get { return (string)base.getProperty("ContactInfo_LName"); }
            set { setProperty("ContactInfo_LName", value); }
        }
        [DisplayName("Phone #")]
        public new string ContactInfo_Phone
        {
            get { return (string)base.getProperty("ContactInfo_Phone"); }
            set { setProperty("ContactInfo_Phone", value); }
        }
        [DisplayName("Birth Date")]
        public new DateTime DateOfBirth
        {
            get
            {
                object ret = base.getProperty("DateOfBirth");
                if (ret != null)
                    return (DateTime)base.getProperty("DateOfBirth");
                else
                    return new DateTime();
            }
            set { setProperty("DateOfBirth", value); }
        }

        //New values
        [DisplayName("Name")]
        public string FullName
        {
            get { return ContactInfo_FName + " " + ContactInfo_LName; }
        }

        public AbsContactInfo() :
            base(new PL.Contact_Info()){ Clear(); }
        public AbsContactInfo(PL.Contact_Info entry) :
        base(entry){ }
        public AbsContactInfo(string email) :
            base(new DBconnections().Contact_Info,email, "ContactInfo_Email")
        {
            ContactInfo_Email = email;
            LoadId();
        }

        private void emailEmptyCheck()
        {
            if (this.ContactInfo_Email == string.Empty)
                throw new Exception("Contact Info Email cannot be blank");
        }

        public int Insert()
        {
            try
            {
                emailEmptyCheck();
                using (fvtcEntities dc = new fvtcEntities()){
                    return base.Insert(dc,dc.Contact_Info);
                }
            }
            catch (Exception e) { throw e; }
        }
        public int Delete()
        {
            try
            {
                emailEmptyCheck();
                using (fvtcEntities dc = new fvtcEntities()){
                    return base.Delete(dc,dc.Contact_Info);
                }
            }
            catch (Exception e) { throw e; }
        }

        public int Update()
        {
            try
            {
                emailEmptyCheck();
                using (fvtcEntities dc = new fvtcEntities()){
                    return base.Update(dc, dc.Contact_Info);
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
                emailEmptyCheck();
                using (fvtcEntities dc = new fvtcEntities())
                {
                    PL.Contact_Info entry = dc.Contact_Info.FirstOrDefault(c =>
                        c.ContactInfo_Email == this.ContactInfo_Email);
                    if (entry == null)
                        throw new Exception("Contact_Info does not exist: Email " + this.ContactInfo_Email ) ;
                    base.LoadId(dc.Contact_Info,entry.Contact_Info_ID);
                }
            }
            catch (Exception e) { throw e; }
        }

        //TODO depreciate
        internal static AbsContactInfo fromNumID(int? memberContact_ID)
        {
            try{
                using (fvtcEntities dc = new fvtcEntities()){
                    AbsContactInfo entry = new AbsContactInfo();
                    entry.LoadId(dc.Contact_Info,memberContact_ID);
                    return entry;
                }
            }
            catch (Exception e) { throw e; }
        }
    }

    public class AbsContactList : AbsList<AbsContactInfo, Contact_Info>
    {
        public new void LoadAll(){
            using (fvtcEntities dc = new fvtcEntities()){
                base.LoadAll(dc.Contact_Info);
            }
        }
    }
}
