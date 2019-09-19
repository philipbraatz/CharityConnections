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
        public Role Role { get; set; }
        public Preference Pref { get; set; }

        //required for login controller
        public Member()
        {}
        public Member(int contactID)
        {
            //get ID and password from other tables
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    ID = (int)dc.Members.Where(c => c.Contact_ID== contactID).FirstOrDefault().Member_ID;
                    this.LoadId();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int Insert()
        {
            try
            {
                //if (ID == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    ID = dc.Members.Max(c => c.Member_ID) + 1;//unique id
                    PL.Member entry = new PL.Member
                    {
                        Member_ID = ID,
                        Contact_ID = Contact.ID,
                        Role_ID = Role.ID
                    };

                    Contact.Insert();
                    Password.Insert(dc,ID);
                    Pref.Insert();
                    foreach (var cat in Prefered_Categories)
                        cat.InsertMember(dc, ID);
                    foreach (var act in helping_Action_List)
                        act.InsertMember(dc, ID);
                    foreach (var char_ID in Prefered_Charity_ID_List)
                        Charity.InsertMember(dc, ID);

                    dc.Members.Add(entry);
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
                    dc.Roles.Remove(dc.Roles.Where(c => c.Role_ID == ID).FirstOrDefault());

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

                    PL.Member entry = dc.Members.Where(c => c.Role_ID == this.ID).FirstOrDefault();
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

                    PL.Member entry = dc.Members.Where(c => c.Role_ID == this.ID).FirstOrDefault();
                    this.ID = entry.Member_ID;
                    this.Contact = new ContactInfo(entry.Contact_ID);

                    PL.Log_in login = dc.Log_in.FirstOrDefault(c => c.MemeberID == this.ID);
                    this.Password = new Password(login.Log_in_ID, login.Password);

                    this.Pref = new Preference((int)entry.Preference_ID);

                    this.Prefered_Categories = Category.LoadMembersList(dc,entry.Member_ID);
                    helping_Action_List = Helping_Action.LoadMembersList(dc,entry.Member_ID);
                    this.Prefered_Charity_ID_List = Charity.LoadMembersIdList(dc,entry.Member_ID);
                    this.Member_Type = new Member_Type(this.ID);
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
                else if (String.IsNullOrEmpty(Password.Hash))
                    throw new Exception("password must be set");//no UserPass
                else
                {
                    DBconnections dc = new DBconnections();
                    PL.Log_in entry =dc.Log_in.FirstOrDefault(u => u.MemeberID == this.ID);

                    if (entry == null)
                        return false;
                    else
                        return entry.Password == Password.Hash;//success if match
                }
            }
            catch (Exception e)
            {throw e;}
        }
    }

    public class MemberList
    : List<Member>
    {
        public void Load()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {

                    dc.Members.ToList().ForEach(c => this.Add(new Member
                    {
                        ID = c.Member_ID,
                        Contact = new ContactInfo(c.Contact_ID),
                        Pref = new Preference((int)c.Preference_ID),
                        Password = new Password(
                            dc.Log_in.FirstOrDefault(d => d.MemeberID == c.Member_ID).Log_in_ID,
                            dc.Log_in.FirstOrDefault(d => d.MemeberID == c.Member_ID).Password),
                        Prefered_Categories = Category.LoadMembersList(dc,c.Member_ID),
                        helping_Action_List = Helping_Action.LoadMembersList(dc,c.Member_ID),
                        Prefered_Charity_ID_List = Charity.LoadMembersIdList(dc,c.Member_ID),
                        Member_Type = new Member_Type(c.Member_ID)
                    }));
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
