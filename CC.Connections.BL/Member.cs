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
    public class Member
    {
        public int ID { get; set; }
        public ContactInfo Contact { get; set; }
        public Password Password { get; set; }
        public List<Category> Prefered_Categories { get; set; }
        public List<int> Prefered_Charity_ID_List { get; set; }
        public List<Helping_Action> helping_Action_List { get; set; }
        public Member_Type Member_Type { get; set; }
        //depreciated
        //public Role Role { get; set; }
        public Preference Pref { get; set; }
        public Location Location { get; set; }

        internal static bool Exists(DBconnections dc, int id)
        {
            return dc.Members.Where(c => c.Member_ID== id).FirstOrDefault() != null;
        }

        //required for login controller
        public Member()
        {
            Clear();
        }
        //new
        public Member(string contactEmail,string password ="",bool hashed =false)
        {
            //get ID and password from other tables
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    Clear();
                    PL.Contact_Info cID = dc.Contact_Info.Where(c => c.ContactInfo_Email == contactEmail).FirstOrDefault();
                    PL.Member mID;
                    if (cID != null)
                    {
                        mID = dc.Members.Where(c => c.MemberContact_ID == cID.Contact_Info_ID).FirstOrDefault();
                        if (mID != null)
                            ID = mID.Member_ID;
                    }
                    Contact.Email = contactEmail;
                    if (password != string.Empty)
                        Password = new Password(contactEmail, password, hashed);//standard
                    else
                        Password = new Password(contactEmail,false);//new
                    //else
                    //    throw new Exception("Contact_ID is null and cannot be loaded");

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
                    Clear();
                    PL.Contact_Info cID = dc.Contact_Info.Where(c => c.ContactInfo_Email == email).FirstOrDefault();
                    if (cID == null)
                        throw new Exception("email "+email+" does not have a contact info");
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

        private void Clear()
        {
            Contact = new ContactInfo();
            Password = new Password();
            Prefered_Categories = new List<Category>();
            Prefered_Charity_ID_List = new List<int>();
            helping_Action_List = new List<Helping_Action>();
            Member_Type = new Member_Type();
            Pref = new Preference();
            Location = new Location();
            
        }

        public int Insert() 
        {
            try
            {
                //if (ID == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    if(dc.Members.ToList().Count > 0 )
                        ID = dc.Members.Max(c => c.Member_ID) + 1;//unique id
                    else
                        ID = 0;

                    Contact.ID = Contact.Insert();
                    Pref.ID = Pref.Insert();
                    Member_Type.ID = Member_Type.Insert();
                    Location.ID = Location.Insert();
                    PL.Member entry = new PL.Member
                    {
                        Member_ID = ID,
                        MemberContact_ID = Contact.ID,
                        //Role_ID = Role.ID,
                        MemberType_ID =Member_Type.ID,
                        MemberPreference_ID = Pref.ID,
                        Location_ID =Location.ID
                    };
                    dc.Members.Add(entry);//adding prior to everything else

                    Password.Insert(dc,ID);
                    foreach (var cat in Prefered_Categories)
                        cat.InsertMember(dc, ID);
                    foreach (var act in helping_Action_List)
                        act.InsertMember(dc, ID);
                    foreach (var char_ID in Prefered_Charity_ID_List)
                        Charity.InsertMember(dc, ID);

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
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    Password.Delete(dc);
                    Pref.Delete();
                    Contact.Delete();
                    //Location.Delete();//TODO impliment
                    foreach (var cat in Prefered_Categories)
                        cat.DeleteMember(dc, ID);
                    foreach (var act in helping_Action_List)
                        act.DeleteMember(dc, ID);
                    foreach (int char_ID in Prefered_Charity_ID_List)
                        Charity.InsertMember(dc, ID, char_ID);//Maybe put in Member class
                    //dc.Roles.Remove(dc.Roles.Where(c => c.Role_ID == ID).FirstOrDefault());

                    dc.Members.Remove(dc.Members.Where(c => c.Member_ID == ID).FirstOrDefault());

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public int Update()
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
                    Contact.Update();
                    Password.Update(dc);
                    Pref.Update();
                    Member_Type.Update();
                    Location.Update();
                    foreach (var cat in Prefered_Categories)
                        cat.UpdateMember(dc, ID);
                    foreach (var act in helping_Action_List)
                        act.UpdateMember(dc, ID);
                    foreach (int char_ID in Prefered_Charity_ID_List)
                        Charity.UpdateMember(dc, ID, char_ID);

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public void LoadId(string email)
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");
                    PL.Contact_Info cID = dc.Contact_Info.Where(c => c.ContactInfo_Email == email).FirstOrDefault();
                    if (cID == null)
                        throw new Exception("Contact info with email " + email + " does not exist");
                    
                    PL.Member entry = dc.Members.Where(c => c.MemberContact_ID == cID.Contact_Info_ID).FirstOrDefault();
                    if (entry != null)
                        this.ID = entry.Member_ID;
                    else
                        throw new Exception("Contact info " + email + " does not have a Member associated with it");

                    PL.Log_in login = dc.Log_in.FirstOrDefault(c => c.LogInMember_ID == this.ID);
                    if (login == null)
                        throw new Exception("Log in does not exist for Member with email "+email);


                    ///all good Login in related values
                    
                    this.Contact = new ContactInfo(email);

                    this.Password = new Password(login.ContactInfoEmail, login.LogInPassword, true);

                    if (entry.MemberPreference_ID == null)
                        throw new Exception("Preference ID is null and cannot be loaded");
                    this.Pref = new Preference((int)entry.MemberPreference_ID);
                    this.Member_Type = new Member_Type((int)entry.MemberType_ID);
                    this.Location = new Location((int)entry.Location_ID);

                    this.Prefered_Categories = Category.LoadMembersList(dc, entry.Member_ID);
                    helping_Action_List = Helping_Action.LoadMembersList(dc, entry.Member_ID);
                    //this.Prefered_Charity_ID_List = Charity.LoadMembersIdList(dc,entry.Member_ID);//TODO impliment charities

                        
                }
            }
            catch (Exception e)
            {
                throw e;
            }
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
                    if(dc.Members.ToList().Count !=0)
                        dc.Members.ToList().ForEach(c => this.Add(new Member
                        {
                            ID = c.Member_ID,
                            Contact = ContactInfo.fromNumID(c.MemberContact_ID),//TODO convert to email instantiation
                            Pref = new Preference((int)c.MemberPreference_ID),
                            //Location = new Location(c.Location_ID),//TODO impliment
                            Password = new Password(
                                dc.Log_in.FirstOrDefault(d => d.LogInMember_ID == c.Member_ID).ContactInfoEmail,
                                dc.Log_in.FirstOrDefault(d => d.LogInMember_ID == c.Member_ID).LogInPassword,
                                true),
                            Prefered_Categories = Category.LoadMembersList(dc,c.Member_ID),
                            helping_Action_List = Helping_Action.LoadMembersList(dc,c.Member_ID),
                            //Prefered_Charity_ID_List = Charity.LoadMembersIdList(dc,c.Member_ID),
                            Member_Type = new Member_Type(c.Member_ID)
                        }));
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
