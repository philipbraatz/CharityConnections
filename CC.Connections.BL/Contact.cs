using CC.Abstract;
using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CC.Connections.BL
{
    public class Contact : ColumnEntry<PL.Contact_Info>
    {
        private static CCEntities dc;

        [DisplayName("Email")]
        public string Member_Email
        {
            get { return (string)base.ID; }
            set { base.ID = value; }
        }

        [DisplayName("First Name")]
        public string FName
        {
            get { return (string)base.getProperty(nameof(FName)); }
            set { setProperty(nameof(FName), value); }
        }
        [DisplayName("Last Name")]
        public string LName
        {
            get { return (string)base.getProperty(nameof(LName)); }
            set { setProperty(nameof(LName), value); }
        }
        [DisplayName("Phone #")]
        public string Phone
        {
            get { return (string)base.getProperty(nameof(Phone)); }
            set { setProperty(nameof(Phone), value); }
        }
        [DisplayName("Birth Date")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth
        {
            get
            {
                object ret = base.getProperty(nameof(DateOfBirth));
                if (ret != null)
                    return (DateTime)base.getProperty(nameof(DateOfBirth));
                else
                    return new DateTime();
            }
            set { 
                if(value > DateTime.Parse("1753/1/1"))//good values get set
                    setProperty("DateOfBirth", value); 
                else
                    setProperty("DateOfBirth", DateTime.Parse("1800/1/1"));//default min value
            }
        }

        //New values
        [DisplayName("Name")]
        public string FullName
        {
            get { return FName + " " + LName; }
        }

        public Contact() :
            base(new PL.Contact_Info()){ Clear(); }
        public Contact(PL.Contact_Info entry) :
        base(entry){ }
        public Contact(string email,bool preloaded =true) :
            base(new CCEntities().Contact_Info,email, preloaded,"ContactInfo_Email")
        {
            Member_Email = email;
            LoadId();
        }
        public Contact(Password _password) :
            base(new PL.Contact_Info())
        {
            Member_Email = _password.email;
            LoadId();
        }

        private void emailEmptyCheck()
        {
            if (string.IsNullOrEmpty(this.Member_Email))
                throw new Exception("Contact Info Email cannot be blank");
        }

        public bool Insert()
        {
            try
            {
                emailEmptyCheck();
                using (CCEntities dc = new CCEntities()){
                    return base.Insert(dc,dc.Contact_Info) >0;
                }
            }
            catch (Exception e) { return false; }//probably already exists OR bad data
        }
        public int Delete()
        {
            try
            {
                emailEmptyCheck();
                using (CCEntities dc = new CCEntities()){
                    return base.Delete(dc,dc.Contact_Info);
                }
            }
            catch (Exception e) { throw; }
        }

        public int Update()
        {
            try
            {
                emailEmptyCheck();
                using (CCEntities dc = new CCEntities()){
                    return base.Update(dc, dc.Contact_Info);
                }
            }
            catch (Exception e) { throw; }
        }

        //sets email then loads
        public void LoadId(string email)
        {
            this.Member_Email = email;
            LoadId();
        }
        //loads from pre-set email
        public void LoadId()
        {
            try
            {
                emailEmptyCheck();
                using (CCEntities dc = new CCEntities())
                {
                    PL.Contact_Info entry = dc.Contact_Info.FirstOrDefault(c =>
                        c.Member_Email == this.Member_Email);
                    if (entry == null)
                        throw new Exception("Contact_Info does not exist with Email \'" + this.Member_Email+"\'" ) ;
                    base.LoadId(dc.Contact_Info);
                }
            }
            catch (Exception e) { throw; }
        }

        //TODO depreciate
        internal static Contact fromNumID(int? memberContact_ID)
        {
            try{
                using (CCEntities dc = new CCEntities()){
                    Contact entry = new Contact();
                    entry.LoadId(dc.Contact_Info,memberContact_ID);
                    return entry;
                }
            }
            catch (Exception e) { throw; }
        }

        public void setContactInfo(Contact contactInfo)
        {
            if (contactInfo == null)
                throw new ArgumentNullException(nameof(contactInfo));
            this.FName = contactInfo.FName ?? "";
            this.LName = contactInfo.LName ?? "";
            this.Phone = contactInfo.Phone ?? "";
            this.Member_Email = contactInfo.Member_Email;
            this.DateOfBirth = contactInfo.DateOfBirth;
        }
    }

    public class ContactList : AbsList<Contact, Contact_Info>
    {
        public new void LoadAll(){
            using (CCEntities dc = new CCEntities()){
                base.LoadAll(dc.Contact_Info);
            }
        }
    }
}
