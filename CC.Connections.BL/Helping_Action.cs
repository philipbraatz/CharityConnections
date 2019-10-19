using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class AbsHelping_Action
        : ColumnEntry<Helping_Action>
    {
        private static fvtcEntities1 dc;

        //id
        public new int ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }
        public AbsCategory category { get; set; }
        public string Action {
            get { return (string)base.getProperty("HelpingActionDescription"); }
            set { setProperty("HelpingActionDescription", value); }
        }

        private void Clear()
        {
            Action = string.Empty;
            category = new AbsCategory();
        }

        public AbsHelping_Action() :
            base(new PL.Helping_Action())
        { Clear(); }
        public AbsHelping_Action(PL.Helping_Action entry) :
            base(entry)
        { Clear(); }
        public AbsHelping_Action(int id) :
            base(new fvtcEntities1().Helping_Action, id)
        {
            Clear();
            ID = id;
            LoadId();
        }

        public void LoadId(){
            using (fvtcEntities1 dc = new fvtcEntities1()){
                base.LoadId(dc.Helping_Action);
        }}
        public int Insert(){
            using (fvtcEntities1 dc = new fvtcEntities1()){
                return base.Insert(dc, dc.Helping_Action);
        }}
        public int Update(){
            using (fvtcEntities1 dc = new fvtcEntities1()){
                return base.Update(dc, dc.Helping_Action);
        }}
        public int Delete(){
            using (fvtcEntities1 dc = new fvtcEntities1()){
                return base.Delete(dc, dc.Helping_Action);
        }}
    }

    public class AbsHelping_ActionList
        : AbsList<AbsHelping_Action, Helping_Action>
    {
        public new void LoadAll(){
            using (fvtcEntities1 dc = new fvtcEntities1()){
                //base.LoadAll(dc.Helping_Action);
                foreach (var c in dc.Helping_Action.ToList())
                    base.Add(new AbsHelping_Action(c));
            }
        }
    }
    public class AbsMemberActionList : AbsListJoin<AbsHelping_Action, Helping_Action, PL.Member_Action>
    {
        int memberID {
            get { return (int)joinGrouping_ID; }
            set { joinGrouping_ID = value; }
        }

        public AbsMemberActionList(int member_id)
            : base("MemberActionMember_ID",//Helping_Action ID
                   member_id,
                   "MemberActionAction_ID")
        {
            Member_Action c = new Member_Action { MemberActionMember_ID = member_id };
            base.joinGrouping_ID = c.MemberActionMember_ID;
        }//  Member ID

        
        public new void LoadPreferences(int member_id){
            using (fvtcEntities1 dc = new fvtcEntities1()){
                //base.LoadWithJoin(dc.Helping_Action, dc.Member_Action,
                //                  new Member_Action { MemberActionMember_ID = member_id }.MemberActionMember_ID);
                memberID = member_id;
                if (dc.Member_Action.ToList().Count != 0)
                {
                    List<Member_Action> member_Actions = dc.Member_Action
                        .Where(c => c.MemberActionMember_ID == memberID).ToList();
                    if (member_Actions.Count != 0)
                    {
                        member_Actions.ForEach(b => base.Add(new AbsHelping_Action(dc.Helping_Action
                                .Where(d => d.Helping_Action_ID == b.MemberActionAction_ID)
                                .FirstOrDefault())
                        ));
                    }
                }
            }
        }
        public new void DeleteAllPreferences(){
            using (fvtcEntities1 dc = new fvtcEntities1()){
                base.DeleteAllPreferences(dc, dc.Member_Action);
            }
        }
        public new void Add(AbsHelping_Action Helping_Action){
            using (fvtcEntities1 dc = new fvtcEntities1()){
                base.Add(dc, dc.Member_Action, new Member_Action(), Helping_Action);
            }
        }
        public new void Remove(AbsHelping_Action Helping_Action){
            using (fvtcEntities1 dc = new fvtcEntities1()){
                base.Remove(dc, dc.Member_Action, Helping_Action);
            }
        }
    }
}
