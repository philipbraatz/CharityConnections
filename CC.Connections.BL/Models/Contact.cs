using CC.Abstract;
using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace CC.Connections.BL
{
    public class Contact : BaseModel<PL.ContactInfo>
    {
        //private static CCEntities dc;

        [DisplayName("Email")]
        public string MemberEmail
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
                    setProperty(nameof(DateOfBirth), value); 
                else
                    setProperty(nameof(DateOfBirth), DateTime.Parse("1800/1/1"));//default min value
            }
        }

        //New values
        [DisplayName("Name")]
        public string FullName
        {
            get { return FName + " " + LName; }
        }

        public Contact() :
            base(new PL.ContactInfo()){ Clear(); }
        public Contact(PL.ContactInfo entry) :
        base(entry){ }
        public Contact(string email,bool preloaded =true) :
            base(new CCEntities().ContactInfoes,email, preloaded,"ContactInfo_Email")
        {
            MemberEmail = email;
            LoadId();
        }
        public Contact(Password _password) :
            base(new PL.ContactInfo())
        {
            MemberEmail = _password.email;
            LoadId();
        }

        private void emailEmptyCheck()
        {
            if (string.IsNullOrEmpty(this.MemberEmail))
                throw new Exception("Contact Info Email cannot be blank");
        }

        public bool Insert()
        {
            try
            {
                emailEmptyCheck();
                using (CCEntities dc = new CCEntities()){
                    return base.Insert(dc,dc.ContactInfoes) >0;
                }
            }
            catch (Exception) { return false; }//probably already exists OR bad data
        }
        public int Delete()
        {
            try
            {
                emailEmptyCheck();
                using (CCEntities dc = new CCEntities()){
                    return base.Delete(dc,dc.ContactInfoes);
                }
            }
            catch (Exception ) { throw; }
        }

        public int Update()
        {
            try
            {
                emailEmptyCheck();
                using (CCEntities dc = new CCEntities()){
                    return base.Update(dc, dc.ContactInfoes);
                }
            }
            catch (Exception ) { throw; }
        }

        //sets email then loads
        public void LoadId(string email)
        {
            this.MemberEmail = email;
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
                    PL.ContactInfo entry = dc.ContactInfoes.FirstOrDefault(c =>
                        c.MemberEmail == this.MemberEmail);
                    if (entry == null)
                        throw new Exception("ContactInfo does not exist with Email \'" + this.MemberEmail+"\'" ) ;
                    base.LoadId(dc.ContactInfoes);
                }
            }
            catch (Exception ) { throw; }
        }

        //TODO depreciate
        internal static Contact fromNumID(int? memberContact_ID)
        {
            try{
                using (CCEntities dc = new CCEntities()){
                    Contact entry = new Contact();
                    entry.LoadId(dc.ContactInfoes,memberContact_ID);
                    return entry;
                }
            }
            catch (Exception ) { throw; }
        }

        public void setContactInfo(Contact contactInfo)
        {
            if (contactInfo == null)
                throw new ArgumentNullException(nameof(contactInfo));
            this.FName = contactInfo.FName ?? "";
            this.LName = contactInfo.LName ?? "";
            this.Phone = contactInfo.Phone ?? "";
            this.MemberEmail = contactInfo.MemberEmail;
            this.DateOfBirth = contactInfo.DateOfBirth;
        }

        public static implicit operator Contact(PL.ContactInfo c) => new Contact(c);
        //public static implicit operator Contact(ColumnEntry<PL.ContactInfo> c) {c. }
    }

    public class ContactCollection : BaseList<Contact, ContactInfo>
    {
        public void LoadAll(){
            using (CCEntities dc = new CCEntities()){
                base.LoadAll(dc.ContactInfoes);
            }
        }
    }
}
