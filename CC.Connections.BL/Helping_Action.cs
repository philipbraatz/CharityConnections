using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    //NOTE PB:
    // 20 min CRUD and Member Crud
    public class Helping_Action
    {
        public int ID { get; set; }
        public Category category { get; set; }
        //TODO ask team to rename Helping Action Desc to Action
        public string Action { get; set; }

        public Helping_Action(){ Clear(); }
        private void Clear()
        {
            Action = string.Empty;
            category = new Category();
        }
        public Helping_Action(int member_Action_Action_ID)
        {
            this.ID = member_Action_Action_ID;
            LoadId();
        }

        private bool Exists(DBconnections dc)
        {
            return dc.Helping_Action.Where(c => c.Helping_Action_ID == ID).FirstOrDefault() != null;
        }


        public int Insert()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Helping_Action.ToList().Count > 0)
                        ID = dc.Helping_Action.Max(c => c.Helping_Action_ID) + 1;//unique id
                    else
                        ID = 0;
                    PL.Helping_Action entry = new PL.Helping_Action
                    {
                        Helping_Action_ID =ID,
                        HelpingActionDescription = this.Action,
                        HelpingActionCategory_ID = this.category.ID
                    };

                    dc.Helping_Action.Add(entry);
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

                    dc.Helping_Action.Remove(dc.Helping_Action.Where(c => c.Helping_Action_ID == ID).FirstOrDefault());
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

                    PL.Helping_Action entry = dc.Helping_Action.Where(c => c.Helping_Action_ID == this.ID).FirstOrDefault();
                    entry.HelpingActionDescription = Action;
                    entry.HelpingActionCategory_ID = category.ID;

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

                    PL.Helping_Action entry = dc.Helping_Action.FirstOrDefault(c => c.Helping_Action_ID == this.ID);
                    if (entry == null)
                        throw new Exception("Helping_Action does not exist");

                    Action = entry.HelpingActionDescription;
                    category = new Category((int)entry.HelpingActionCategory_ID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        internal static bool Exists(DBconnections dc,int actionID)
        {
            return dc.Helping_Action.Where(c => c.Helping_Action_ID == actionID).FirstOrDefault() != null;
        }
    }

    public class Helping_ActionList
        : List<Helping_Action>
    {
        //only used for preference lists
        int? member_ID { get; set; }

        private const string PREFERENCE_LOAD_ERROR = "Preferences not loaded, please loadPrefences with a Member ID";
        public void LoadList()
        {
            try
            {
                this.Clear();
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Helping_Action.ToList().Count != 0)
                        dc.Helping_Action.ToList().ForEach(c => this.Add(new Helping_Action
                        {
                            ID = c.Helping_Action_ID,
                            Action = c.HelpingActionDescription,
                            category = new Category((int)c.HelpingActionCategory_ID)
                        }));
                }
            }
            catch (Exception e) { throw e; }
        }

        public Helping_ActionList LoadPreferences(int memberID)
        {
            try
            {
                this.Clear();
                member_ID = (int?)memberID;
                using (DBconnections dc = new DBconnections())
                {
                    if (!MemberExists(dc, memberID))
                        throw new Exception("Member ID: "+ memberID + "does not exist");

                    if (dc.Member_Action.ToList().Count != 0)
                        dc.Member_Action.Where(d => d.MemberActionMember_ID == memberID).ToList().ForEach(c =>
                             this.Add(new Helping_Action((int)c.MemberActionAction_ID)));
                }
                return this;
            }
            catch (Exception e) { throw e; }
        }
        public void AddPreference(int actionID)
        {
            if (member_ID == null)
                throw new Exception(PREFERENCE_LOAD_ERROR);

            using (DBconnections dc = new DBconnections())
            {
                if (!Helping_Action.Exists(dc, actionID))
                    throw new Exception("Helping Action ID: "+actionID+" does not exist");

                dc.Member_Action.Add(new Member_Action
                {
                    MemberAction_ID = dc.Member_Action.Max(c => c.MemberAction_ID),
                    MemberActionAction_ID = actionID,
                    MemberActionMember_ID = member_ID
                });
                this.Add(new Helping_Action(actionID));
            }
        }

        public void DeletePreference(int actionID)
        {
            if (member_ID == null)
                throw new Exception(PREFERENCE_LOAD_ERROR);

            using (DBconnections dc = new DBconnections())
            {
                if (!Helping_Action.Exists(dc, actionID))
                    throw new Exception("Helping Action ID: " + actionID + " does not exist");

                dc.Member_Action.Remove(dc.Member_Action.Where(
                    c => c.MemberActionMember_ID == member_ID &&
                    c.MemberActionAction_ID == actionID).FirstOrDefault());
                this.Remove(new Helping_Action(actionID));
            }
        }
        //public void UpdateCategory(Category cat, string description)
        //{
        //    Category cthis = this.Where(c => c.ID == cat.ID).FirstOrDefault();
        //    cthis.Desc = description;
        //    cthis.Update();
        //
        //}
        //public void UpdateCategory(int catID, string description)
        //{
        //    Category cthis = this.Where(c => c.ID == catID).FirstOrDefault();
        //    cthis.Desc = description;
        //    cthis.Update();
        //}

        private bool MemberExists(DBconnections dc, int? member_ID)
        {
            return dc.Member_Action.Where(c => c.MemberActionMember_ID == member_ID
            ).FirstOrDefault() != null;
        }

        public new void Clear()
        {
            base.Clear();
            member_ID = null;
        }
    }
}
