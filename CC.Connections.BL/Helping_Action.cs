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

        internal bool InsertMember(DBconnections dc, int id)
        {
            if (MemberExists(dc, id))//dont add existing
                return false;
            if (!Exists(dc))//add missing
                Insert();

            if (dc.Member_Action.ToList().Count >0)
                ID = (int)dc.Member_Action.Max(c => c.MemberActionMember_ID) + 1;//unique id
            else
                ID = 0;

            //TODO check if new category or new member first
            Member_Action entry = new Member_Action
            {
                MemberAction_ID =ID,
                MemberActionMember_ID = id,//member
                MemberActionAction_ID = ID//action
            };

            dc.Member_Action.Add(entry);
            dc.SaveChanges();
            return true;
        }

        private bool Exists(DBconnections dc)
        {
            return dc.Helping_Action.Where(c => c.Helping_Action_ID == ID).FirstOrDefault() != null;
        }

        private bool MemberExists(DBconnections dc, int id)
        {
            return dc.Member_Action.Where(c => c.MemberActionAction_ID == ID && c.MemberActionMember_ID == id).FirstOrDefault() != null;
        }

        //TODO test methods
        internal void DeleteMember(DBconnections dc, int id)
        {
            var entryList = dc.Member_Action.Where(c => c.MemberActionMember_ID == id);
            foreach (var entry in entryList)
                dc.Member_Action.Remove(entry);
        }

        //TODO think how this would happen
        internal void UpdateMember(DBconnections dc, int id)
        {
            dc.Member_Action.Where(c => c.MemberActionAction_ID == id)
                .ToList().ForEach(c => c.MemberActionMember_ID = ID);//might not work
        }

        internal static List<Helping_Action> LoadMembersList(DBconnections dc, int member_ID)
        {
            List<Helping_Action> retList = new List<Helping_Action>();
            dc.Member_Action.Where(c => c.MemberActionMember_ID == member_ID).ToList().ForEach(c => retList.Add(new Helping_Action((int)c.MemberActionAction_ID)));
            return retList;
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
    }
}
