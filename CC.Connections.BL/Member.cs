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

        //required for login controller
        public Member()
        {
            Clear();
        }
        public Member(int memberID)
        {
            //get ID and password from other tables
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    PL.Member mID = dc.Members.Where(c => c.Member_ID == memberID).FirstOrDefault();
                    if(mID == null)
                        throw new Exception("Contact_ID is null and cannot be loaded");
                    ID = (int)mID.Member_ID;
                    this.LoadId();
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
            //Role = new Role();
            Pref = new Preference();
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


                    PL.Member entry = new PL.Member
                    {
                        Member_ID = ID,
                        MemberContact_ID = Contact.ID,
                        //Role_ID = Role.ID,
                        MemberType_ID =Member_Type.ID,
                        MemberPreference_ID = Pref.ID
                    };
                    dc.Members.Add(entry);//adding prior to everything else

                    Contact.Insert();
                    Password.Insert(dc,ID);
                    Pref.Insert();
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

                    Contact.Delete();
                    Password.Delete(dc, ID);
                    Pref.Delete();
                    foreach (var cat in Prefered_Categories)
                        cat.DeleteMember(dc, ID);
                    foreach (var act in helping_Action_List)
                        act.DeleteMember(dc, ID);
                    foreach (int char_ID in Prefered_Charity_ID_List)
                        Charity.InsertMember(dc, ID, char_ID);//Maybe put in Member class
                    //dc.Roles.Remove(dc.Roles.Where(c => c.Role_ID == ID).FirstOrDefault());

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
                    Password.Update(dc, ID);
                    Pref.Update();
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
        public void LoadId()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Member entry = dc.Members.Where(c => c.Member_ID == this.ID).FirstOrDefault();
                    this.ID = entry.Member_ID;
                    this.Contact = new ContactInfo(entry.MemberContact_ID);

                    PL.Log_in login = dc.Log_in.FirstOrDefault(c => c.LogInMember_ID == this.ID);
                    this.Password = new Password(login.ContactInfoEmail,login.LogInPassword,true);

                    if (entry.MemberPreference_ID == null)
                        throw new Exception("Preference ID is null and cannot be loaded");
                    this.Pref = new Preference((int)entry.MemberPreference_ID);
                    this.Member_Type = new Member_Type((int)entry.MemberType_ID);

                    this.Prefered_Categories = Category.LoadMembersList(dc,entry.Member_ID);
                    helping_Action_List = Helping_Action.LoadMembersList(dc,entry.Member_ID);
                    //this.Prefered_Charity_ID_List = Charity.LoadMembersIdList(dc,entry.Member_ID);//TODO impliment charities
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool Login()
        {
            try
            {
                if (String.IsNullOrEmpty(Contact.Email))
                    throw new Exception("email must be set");//no userId
                else if (String.IsNullOrEmpty(Password.Pass))
                    throw new Exception("password must be set");//no UserPass
                else
                {
                    using (DBconnections dc = new DBconnections())
                    {
                        PL.Log_in entry = dc.Log_in.FirstOrDefault(u => u.LogInMember_ID == this.ID);

                        if (entry == null)
                            return false;
                        else
                            return entry.LogInPassword == Password.Pass;//success if match
                    }
                }
            }
            catch (Exception e)
            {throw e;}
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
                    //if (entry.Preference_ID == null)
                    //    throw new Exception("Preference ID is null and cannot be loaded");
                    dc.Members.ToList().ForEach(c => this.Add(new Member
                    {
                        ID = c.Member_ID,
                        Contact = new ContactInfo(c.MemberContact_ID),
                        Pref = new Preference((int)c.MemberPreference_ID),
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
