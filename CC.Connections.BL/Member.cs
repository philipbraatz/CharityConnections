using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class BLMember : AbsContact
    {
        public new int ID { get; set; }
        public Password Password { get; set; }
        public AbsCategoryPreferences Prefered_Categories { get; set; }
        public CharityList Prefered_Charities { get; set; }
        public AbsMemberActionList Prefered_helping_Actions { get; set; }
        public MemberType Member_Type { get; set; }
        //depreciated
        //public Role Role { get; set; }
        public AbsPreference Pref { get; set; }
        public AbsLocation Location { get; set; }

        internal static bool Exists(fvtcEntities1 dc, int id)
        {
            return dc.Members.Where(c => c.Member_ID == id).FirstOrDefault() != null;
        }

        //required for login controller
        public BLMember()
        {
            Clear();
        }
        public BLMember(Contact_Info entry) :
            base(entry)
        { }
        public BLMember(PL.Member entry)
        {
            ID = entry.Member_ID;
            Clear();
            base.setContactInfo( AbsContact.fromNumID(entry.MemberContact_ID));

            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                Member_Type = (MemberType)dc.Log_in.Where(c => this.ContactInfo_Email == c.ContactInfoEmail).FirstOrDefault().MemberType;
            }
            Pref = new AbsPreference((int)entry.MemberPreference_ID);
            Location = new AbsLocation((int)entry.Location_ID);
            Password = new Password(ContactInfo_Email);

            Prefered_Categories.LoadPreferences(ID);
            //Prefered_Charities.load();
            Prefered_helping_Actions.LoadPreferences(ID);
        }
        //new member, member_type is hardcoded as guess
        //DOES NOT try to LOAD from database
        public BLMember(string contactEmail, string password="", int member_type=3, bool hashed = false, bool debug = false)
        {
            //get ID and password from other tables
            try
            {
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    Clear();

                    //set new id for prefered lists
                    if (dc.Members.ToList().Count > 0)
                        ID = dc.Members.Max(c => c.Member_ID) + 1;
                    else
                        ID = 0;//first entry

                    //try to load existing ID
                    PL.Contact_Info cID = dc.Contact_Info.Where(c => c.ContactInfo_Email == contactEmail).FirstOrDefault();
                    PL.Member mID;
                    if (cID != null)
                    {
                        mID = dc.Members.Where(c => c.MemberContact_ID == cID.Contact_Info_ID).FirstOrDefault();
                        if (mID != null)
                            ID = mID.Member_ID;
                    }

                    this.ContactInfo_Email = contactEmail;
                    if (password != string.Empty)
                        Password = new Password(contactEmail, password, hashed);//standard
                    else
                        Password = new Password(contactEmail, false);//new

                    this.Member_Type = (MemberType)dc.Log_in.Where(c => this.ContactInfo_Email == c.ContactInfoEmail).FirstOrDefault().MemberType;

                    //will LOAD nothing if NEW ID but will SET preference ID
                    //needed for adding and removing items
                    Prefered_helping_Actions.LoadPreferences(ID);
                    Prefered_Categories.LoadPreferences(ID);
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
        //load
        public BLMember(string email)
        {
            //get ID and password from other tables
            try
            {
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    PL.Contact_Info cID = dc.Contact_Info.Where(c => c.ContactInfo_Email == email).FirstOrDefault();
                    if (cID == null)
                        throw new Exception("Email " + email + " does not have a contact info");
                    //else
                    //{
                    //    PL.Member mID = dc.Members.Where(c => c.MemberContact_ID == cID.Contact_Info_ID).FirstOrDefault();
                    //    ID = (int)mID.Member_ID;
                    //}
                    this.LoadId(email);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public static implicit operator AbsMember(PL.Member entry)
        //{
        //    return new AbsMember(entry);
        //}

        //only clears BLclass varibles
        private new void Clear()
        {
            Password = new Password();
            Prefered_Categories = new AbsCategoryPreferences(ID);
            Prefered_Charities = new CharityList();
            Prefered_helping_Actions = new AbsMemberActionList(ID);
            Member_Type = new MemberType();
            Pref = new AbsPreference();
            Location = new AbsLocation();
        }

        public new int Insert()
        {
            base.Insert();

            try
            {
                //if (ID == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    //double check ID before insert
                    if(dc.Members.Where(c=>c.Member_ID ==ID) != null)
                            ID = dc.Members.Max(c => c.Member_ID) + 1;//unique id

                    Pref.Insert();
                    //Member_Type.Insert();
                    Location.Insert();
                    PL.Member entry = new PL.Member
                    {
                        Member_ID = ID,
                        MemberContact_ID = contact_ID,
                        //Role_ID = Role.ID,
                        MemberPreference_ID = Pref.ID,
                        Location_ID = Location.ID
                    };
                    dc.Members.Add(entry);//adding prior to everything else

                    Password.Insert(ID);

                    //do not handle member preference lists here

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public new int Delete()
        {
            try
            {
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    Password.Delete();
                    Pref.Delete();
                    Location.Delete();
                    base.Delete();
                    //dc.Roles.Remove(dc.Roles.Where(c => c.Role_ID == ID).FirstOrDefault());

                    dc.Members.Remove(dc.Members.Where(c => c.Member_ID == ID).FirstOrDefault());

                    Prefered_Categories.DeleteAllPreferences();
                    //Prefered_Charities.DeleteAllPreferences();
                    Prefered_helping_Actions.DeleteAllPreferences();

                    Clear();
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public new int Update()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Member entry = dc.Members.Where(c => c.Member_ID == this.ID).FirstOrDefault();
                    base.Update();
                    Password.Update();
                    Pref.Update();
                    //Member_Type.Update();
                    Location.Update();

                    //do not handle member preference lists here

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public new void LoadId(string email)
        {
            base.LoadId(email);

            try
            {
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Member entry = dc.Members.Where(c => c.MemberContact_ID == this.contact_ID).FirstOrDefault();
                    if (entry != null)
                        this.ID = entry.Member_ID;
                    else
                        throw new Exception("Contact info " + email + " does not have a Member associated with it");
                    ///all good Login in related values
                    Clear();//make sure member fields are created

                    PL.Log_in login = dc.Log_in.FirstOrDefault(c => c.ContactInfoEmail == email);
                    if (login == null)
                        throw new Exception("Log in does not exist for Member with email " + email);

                    this.Password = new Password(login.ContactInfoEmail, login.LogInPassword, true);

                    if (entry.MemberPreference_ID == null)
                        throw new Exception("Preference ID is null and cannot be loaded");
                    this.Pref = new AbsPreference((int)entry.MemberPreference_ID);
                    this.Member_Type = (MemberType)dc.Log_in.Where(c => this.ContactInfo_Email == c.ContactInfoEmail).FirstOrDefault().MemberType;
                    this.Location = new AbsLocation((int)entry.Location_ID);

                    this.Prefered_Categories.LoadPreferences(entry.Member_ID);
                    this.Prefered_helping_Actions.LoadPreferences(entry.Member_ID);
                    //this.Prefered_Charities.LoadPreferences( entry.Member_ID);


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //loads contact info only
        internal static BLMember loadContactInfo(int? memberContact_ID)
        {
            AbsContact info = AbsContact.fromNumID(memberContact_ID);
            return new BLMember
            {
                ContactInfo_Email = info.ContactInfo_Email,
                ContactInfo_FName = info.ContactInfo_FName,
                ContactInfo_LName = info.ContactInfo_LName,
                ContactInfo_Phone = info.ContactInfo_Phone,
                DateOfBirth = info.DateOfBirth
            };
        }

        //sets contact info only
        public void setContactInfo(AbsContact contact)
        {
            ContactInfo_Email = contact.ContactInfo_Email;
            ContactInfo_FName = contact.ContactInfo_FName;
            ContactInfo_LName = contact.ContactInfo_LName;
            ContactInfo_Phone = contact.ContactInfo_Phone;
            DateOfBirth = contact.DateOfBirth;
        }
    }

    public class MemberList
    : List<BLMember>
    {
        public void LoadList()
        {
            try
            {
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    if (dc.Members.ToList().Count != 0)
                        dc.Members.ToList().ForEach(c =>
                        {
                            BLMember newMem = BLMember.loadContactInfo(c.MemberContact_ID);
                            newMem.ID = c.Member_ID;
                            newMem.Pref = new AbsPreference((int)c.MemberPreference_ID);
                            newMem.Password = new Password(newMem.ContactInfo_Email);
                            newMem.Member_Type = (MemberType)dc.Log_in.Where(d => newMem.ContactInfo_Email == d.ContactInfoEmail).FirstOrDefault().MemberType;
                            newMem.Location = new AbsLocation((int)c.Location_ID);

                            newMem.Prefered_Categories.LoadPreferences(c.Member_ID);
                            newMem.Prefered_helping_Actions.LoadPreferences(c.Member_ID);

                            this.Add(newMem);
                        });
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}