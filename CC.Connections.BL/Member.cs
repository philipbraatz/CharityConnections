using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    //NOTE: PB 1.5 hours CRUD
    //      2 creating links to classes
    //      15 min load all
    public class Member : ContactInfo
    {
        public int ID { get; set; }
        public Password Password { get; set; }
        public CategoryList Prefered_Categories { get; set; }
        public CharityList Prefered_Charities { get; set; }
        public Helping_ActionList Prefered_helping_Actions { get; set; }
        public Member_Type Member_Type { get; set; }
        //depreciated
        //public Role Role { get; set; }
        public Preference Pref { get; set; }
        public Location Location { get; set; }

        internal static bool Exists(DBconnections dc, int id)
        {
            return dc.Members.Where(c => c.Member_ID == id).FirstOrDefault() != null;
        }

        //required for login controller
        public Member()
        {
            Clear();
        }
        //new member, member_type is hardcoded as guess
        //DOES NOT try to LOAD from database
        public Member(string contactEmail, string password="", int member_type=3, bool hashed = false)
        {
            //get ID and password from other tables
            try
            {
                using (DBconnections dc = new DBconnections())
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

                    this.Email = contactEmail;
                    if (password != string.Empty)
                        Password = new Password(contactEmail, password, hashed);//standard
                    else
                        Password = new Password(contactEmail, false);//new

                    this.Member_Type = new Member_Type(member_type);

                    //will LOAD nothing if NEW ID but will SET preference ID
                    //needed for adding and removing items
                    Prefered_helping_Actions.LoadPreferences(ID);
                    Prefered_Categories.LoadPreferences(ID);
                    //Prefered_Charities.LoadPreferences(ID);

                ///STOPPED HERE -----------------------------
                ///prefered lists cannot be added to without first creating their id!

                    //this.LoadId();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        //load
        public Member(string email)
        {
            //get ID and password from other tables
            try
            {
                using (DBconnections dc = new DBconnections())
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
        //only clears MEMBER values
        private new void Clear()
        {
            Password = new Password();
            Prefered_Categories = new CategoryList();
            Prefered_Charities = new CharityList();
            Prefered_helping_Actions = new Helping_ActionList();
            Member_Type = new Member_Type();
            Pref = new Preference();
            Location = new Location();
        }

        public new int Insert()
        {
            base.Insert();

            try
            {
                //if (ID == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    //double check ID before insert
                    if(dc.Members.Where(c=>c.Member_ID ==ID) != null)
                            ID = dc.Members.Max(c => c.Member_ID) + 1;//unique id

                    Pref.ID = Pref.Insert();
                    //Member_Type.ID = Member_Type.Insert();
                    Location.ID = Location.Insert();
                    PL.Member entry = new PL.Member
                    {
                        Member_ID = ID,
                        MemberContact_ID = contact_ID,
                        //Role_ID = Role.ID,
                        MemberType_ID = Member_Type.ID,
                        MemberPreference_ID = Pref.ID,
                        Location_ID = Location.ID
                    };
                    dc.Members.Add(entry);//adding prior to everything else

                    Password.Insert(dc, ID);

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
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    Password.Delete(dc);
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
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Member entry = dc.Members.Where(c => c.Member_ID == this.ID).FirstOrDefault();
                    base.Update();
                    Password.Update(dc);
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
                using (DBconnections dc = new DBconnections())
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
                    this.Pref = new Preference((int)entry.MemberPreference_ID);
                    this.Member_Type = new Member_Type((int)entry.MemberType_ID);
                    this.Location = new Location((int)entry.Location_ID);

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
        internal new static Member fromNumID(int? memberContact_ID)
        {
            ContactInfo info = ContactInfo.fromNumID(memberContact_ID);
            return new Member
            {
                Email = info.Email,
                FirstName = info.FirstName,
                LastName = info.LastName,
                Phone = info.Phone,
                BirthDate = info.BirthDate
            };
        }
    }

    public class MemberList
    : List<Member>
    {
        public void LoadList()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Members.ToList().Count != 0)
                        dc.Members.ToList().ForEach(c =>
                        {
                            Member newMem = Member.fromNumID(c.MemberContact_ID);
                            newMem.ID = c.Member_ID;
                            newMem.Pref = new Preference((int)c.MemberPreference_ID);
                            newMem.Password = new Password(
                                  dc.Log_in.FirstOrDefault(d => d.LogInMember_ID == c.Member_ID).ContactInfoEmail,
                                  dc.Log_in.FirstOrDefault(d => d.LogInMember_ID == c.Member_ID).LogInPassword,
                                  true);
                            newMem.Prefered_Categories = new CategoryList().LoadPreferences(c.Member_ID);
                            newMem.Prefered_helping_Actions = new Helping_ActionList().LoadPreferences(c.Member_ID);
                            newMem.Member_Type = new Member_Type((int)c.MemberType_ID);
                            newMem.Location = new Location((int)c.Location_ID);
                            this.Add(newMem);
                        });
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
