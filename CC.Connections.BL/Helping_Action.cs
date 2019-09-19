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

        public Helping_Action(){}
        public Helping_Action(int member_Action_Action_ID)
        {
            this.ID = member_Action_Action_ID;
        }

        internal void InsertMember(DBconnections dc, int id)
        {
            //TODO check if new category or new member first
            Member_Action entry = new Member_Action
            {
                Member_Action_ID = (int)dc.Member_Action.Max(c => c.Member_Action_Member_ID) + 1,
                Member_Action_Member_ID = id,//member
                Member_Action_Action_ID = ID//action
            };

            dc.Member_Action.Add(entry);
        }
        //TODO test methods
        internal void DeleteMember(DBconnections dc, int id)
        {
            var entryList = dc.Member_Action.Where(c => c.Member_Action_Member_ID == id);
            foreach (var entry in entryList)
                dc.Member_Action.Remove(entry);
        }

        internal void UpdateMember(DBconnections dc, int id)
        {
            dc.Member_Action.Where(c => c.Member_Action_Member_ID == id)
                .ToList().ForEach(c => c.Member_Action_Member_ID = ID);//might not work
        }

        internal static List<Helping_Action> LoadMembersList(DBconnections dc, int member_ID)
        {
            List<Helping_Action> retList = new List<Helping_Action>();
            dc.Member_Action.Where(c => c.Member_Action_Member_ID == member_ID).ToList().ForEach(c => retList.Add(new Helping_Action((int)c.Member_Action_Action_ID)));
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
                    ID = dc.Helping_Action.Max(c => c.Helping_Action_ID) + 1;
                    PL.Helping_Action entry = new PL.Helping_Action
                    {
                        Helping_Action_ID =ID,
                        Helping_Action_Desc = this.Action,
                        Helping_Action_CatID = this.category.ID
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
                    entry.Helping_Action_Desc = Action;
                    entry.Helping_Action_CatID = category.ID;

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
                        throw new Exception("Genre does not exist");

                    Action = entry.Helping_Action_Desc;
                    category = new Category((int)entry.Helping_Action_CatID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
