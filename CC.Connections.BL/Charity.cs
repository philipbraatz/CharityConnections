using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;
using System.ComponentModel;



// setting the charity class with a inherit from contactInfo

namespace CC.Connections.BL
{
    public class Charity : AbsContact
    {
        //AbsContact properties
        //ContactInfo_Email = Support Email
        //ContactInfo_FName = Name of Charity
        //ContactInfo_Phone = phone #
        //DateOfBirth = Start of Charity organization

        //ContactInfo_LName = Not used
        //ContactInfo_FullName = Not used

        public string ChName { get; set; }
        [DisplayName("CharityName")]

        public new int ID { get; set; }
        [DisplayName("Federal Tax ID#")]
        public string EIN { get; set; }
        [DisplayName("Is Deductible")]
        public bool Deductibility { get; set; }
        [DisplayName("Charity Web Site")]
        public string URL { get; set; }
        [DisplayName("Charities Cause")]
        public string Cause { get; set; }
        public AbsCategory Category { get; set; }//never delete
        public AbsLocation Location { get; set; }//delete when removed
        [DisplayName("Charities Requirements")]
        public string Requirements { get; set; }
        [DisplayName("Charity Email")]
        public string CharityEmail { get; set; }
        public Password Password { get; set; }//delete when removed

        public CharityEventList charityEvents { get; set; }

        internal static bool Exists(CCEntities dc, int id)
        {
            return dc.Charities.Where(c => c.Charity_ID == id).FirstOrDefault() != null;
        }
        public string CharityName { 
            get { return ContactInfo_LName; }
            set { ContactInfo_LName = value; }
        }

        public Charity()
        {

        }
        public Charity(Password p) :this(p.email)
        {
        }

        public Charity(Contact_Info entry) :
            base(entry)
        { }
        public Charity(int id)
        {
            Clear();
            LoadId(id);
        }
        public Charity(PL.Charity entry)
        {
            Clear();
            //this.setContactInfo((Charity)AbsContact.ContactInfo_Email(entry.Charity_Email.ToString));
            //this.setContactInfo((Charity)AbsContact.fromNumID(entry.Charity_Contact_ID.Value));
            this.ID                 = entry.Charity_ID;
            this.ChName             = entry.Charity_Name;
            this.Deductibility      = entry.Charity_Deductibility.Value;
            this.EIN                = entry.Charity_EIN;
            this.ContactInfo_Email  = entry.Charity_Email;
            this.URL                = entry.Charity_URL;
            this.Requirements       = entry.Charity_Requirements;
            this.Cause              = entry.Charity_Cause;
            this.Category = new AbsCategory(entry.Charity_Category_ID.Value);
            this.Location = new AbsLocation(entry.Location_ID.Value);
        }


        public Charity(string charityEmail, string password = "", bool hashed = false, bool debug = false)
        {
            //get ID and password from other tables
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    Clear();

                    //set new id for preferred lists
                    if (dc.Members.ToList().Count > 0)
                        ID = dc.Members.Max(c => c.Member_ID) + 1;
                    else
                        ID = 0;//first entry

                    //try to load existing ID
                    PL.Contact_Info cID = dc.Contact_Info.Where(c => c.ContactInfo_Email == charityEmail).FirstOrDefault();
                    PL.Member mID;
                    if (cID != null)
                    {
                        mID = dc.Members.Where(c => c.MemberContact_ID == cID.Contact_Info_ID).FirstOrDefault();
                        if (mID != null)
                            ID = mID.Member_ID;
                    }

                    this.ContactInfo_Email = charityEmail;
                    if (password != string.Empty)
                        Password = new Password(charityEmail, password, MemberType.CHARITY, hashed);//standard
                    else
                        Password = new Password(charityEmail, false);//new

                    //will LOAD nothing if NEW ID but will SET preference ID
                    //needed for adding and removing items
                    //Prefered_helping_Actions.LoadPreferences(ID);
                    //Prefered_Categories.LoadPreferences(ID);
                    //Prefered_Charities.LoadPreferences(ID);

                    //this.LoadId();
                }
            }
            catch (Exception e)
            {
                if (e.Message != "The underlying provider failed on Open.")
                    throw e;
                else
                    throw e.InnerException;//database error

            }
        }


        private new void Clear()
        {
            base.Clear();
            ChName = string.Empty;
            CharityEmail = string.Empty;
            EIN = string.Empty;
            Deductibility = false;
            URL = string.Empty;
            Cause = string.Empty;
            Category = new Category();
            Location = new Location();
            Requirements = string.Empty;
        }

        internal static List<int> LoadMembersIdList(CCEntities dc, int member_ID)
        {
            throw new NotImplementedException();
        }

        public void LoadId(int charity_id)
        {
            this.ID = charity_id;
            LoadId();
        }
        public new void LoadId()
        {
                try
            {
                using (CCEntities dc = new CCEntities())
                {
                    PL.Charity entry = dc.Charities.Where(c => c.Charity_ID == this.ID).FirstOrDefault();
                    if (entry != null)
                        this.ID = entry.Charity_ID;
                    else
                        throw new Exception("Charity ID " + this.ID + " does not have a Charity associated with it");

                    Clear();

                    //base.setContactInfo(AbsContact.fromNumID(entry.Charity_Contact_ID));
                    this.EIN            = entry.Charity_EIN;
                    this.CharityName    = entry.Charity_Name;
                    this.Deductibility  = entry.Charity_Deductibility.Value;
                    this.URL            = entry.Charity_URL;
                    this.Cause          = entry.Charity_Cause;
                    this.CharityEmail   = entry.Charity_Email;
                    this.Category   = new  AbsCategory(entry.Charity_Category_ID.Value);
                    this.Location   = new  AbsLocation(entry.Location_ID.Value);
                    this.Requirements   = entry.Charity_Requirements;

                    this.Category = new AbsCategory(entry.Charity_Category_ID.Value);
                    this.Location = new AbsLocation(entry.Location_ID.Value);
                    this.Password = new Password(   entry.Charity_Email);
                }
            }
            catch (Exception e)
            {throw e;}
        }

        public new int Insert()
        {
            {
                base.Insert();

                try
                {
                    //if (ID == string.Empty)
                    //    throw new Exception("Description cannot be empty");
                    using (CCEntities dc = new CCEntities())
                    {
                        //double check ID before insert
                        if (dc.Charities.Where(c => c.Charity_ID == ID) != null)
                            ID = dc.Charities.Max(c => c.Charity_ID) + 1;
                        
                        PL.Charity entry = new PL.Charity
                        {
                            Charity_ID = ID,
                            Charity_Name = this.ChName,
                            Charity_EIN = EIN,
                            Charity_Deductibility = Deductibility,
                            Charity_URL = URL,
                            Charity_Cause = Cause,
                            Charity_Email = CharityEmail,
                            Charity_Category_ID = this.Category.ID,
                            Location_ID = this.Location.ID,
                            Charity_Requirements = Requirements
                        };
                        dc.Charities.Add(entry);
                        Location.Insert();
                        Password.Insert(ID);//match Charity ID with given password
                        
                        return dc.SaveChanges();
                    }
                }
                catch (Exception e) { throw e; }
            }
        }

        public new int Update()
        {
            try
            {           
                using (CCEntities dc = new CCEntities())
                {
                    PL.Charity entry = dc.Charities.Where(c => c.Charity_ID == this.ID).FirstOrDefault();
                    entry.Charity_Category_ID = this.Category.ID;
                    entry.Charity_Cause = this.Cause;
                    entry.Charity_Name = this.ChName;
                    entry.Charity_Deductibility = this.Deductibility;
                    entry.Charity_EIN = this.EIN;
                    entry.Charity_Email = this.CharityEmail;
                    entry.Charity_ID = this.ID;
                    entry.Charity_Requirements = this.Requirements;
                    entry.Charity_URL = this.URL;
                    entry.Location_ID = this.Location.ID;
                    
                    base.Update();
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        public new int Delete()
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {                  
                    base.Delete();

                    dc.Charities.Remove(dc.Charities.Where(c => c.Charity_ID == ID).FirstOrDefault());
                    
                    Clear();
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        internal static Charity fromCharityID(int? charity_ID)
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    Charity retChar = new Charity();
                    PL.Charity entryCharity = dc.Charities.FirstOrDefault(c => c.Charity_ID == charity_ID);
                    if (entryCharity == null)
                        throw new Exception("Charity does not exist: Charity ID '" + retChar.ID + "'"); ;

                    //retChar = (Charity)AbsContact.fromNumID(entryCharity.Charity_Contact_ID.Value);
                    retChar.ID = entryCharity.Charity_ID;
                    retChar.CharityName = entryCharity.Charity_Name;
                    retChar.Category = new AbsCategory( entryCharity.Charity_Category_ID.Value);
                    retChar.Deductibility = entryCharity.Charity_Deductibility.Value;
                    retChar.EIN = entryCharity.Charity_EIN;
                    retChar.ContactInfo_Email = entryCharity.Charity_Email;
                    retChar.Location = new AbsLocation( entryCharity.Location_ID.Value);
                    retChar.URL = entryCharity.Charity_URL;
                    retChar.Requirements = entryCharity.Charity_Requirements;
                    retChar.Cause = entryCharity.Charity_Cause;
                    return retChar;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }    

    public class CharityList :
        List<Charity>
    {
        public string locationCity;
        public string locationState;

        public void LoadList()
        {
            try{
                using (CCEntities dc = new CCEntities()){
                    if(dc.Charities.ToList().Count != 0)
                        dc.Charities.ToList().ForEach(c =>{ 
                            this.Add(new Charity(c.Charity_ID));});
                }
            }
            catch (Exception e) { throw e; }
        }
        public void load()
        {
            throw new NotImplementedException();
        }     
    }
}
