using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Abstract;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class AbsHelpingAction
        : ColumnEntry<HelpingAction>
    {
        //private static CCEntities dc;

        //id
        public new int ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }
        public Category category { get; set; }
        public string Action
        {
            get { return (string)base.getProperty("HelpingActionDescription"); }
            set { setProperty("HelpingActionDescription", value); }
        }

        private new void Clear()
        {
            Action = string.Empty;
            category = new Category();
        }

        public AbsHelpingAction() :
            base(new PL.HelpingAction())
        { Clear(); }
        public AbsHelpingAction(PL.HelpingAction entry) :
            base(entry)
        { Clear(); }
        public AbsHelpingAction(int id) :
            base(new CCEntities().HelpingActions, id)
        {
            Clear();
            ID = id;
            LoadId();
        }

        public void LoadId()
        {
            using (CCEntities dc = new CCEntities())
            {
                base.LoadId(dc.HelpingActions);
            }
        }
        public int Insert()
        {
            using (CCEntities dc = new CCEntities())
            {
                return base.Insert(dc, dc.HelpingActions);
            }
        }
        public int Update()
        {
            using (CCEntities dc = new CCEntities())
            {
                return base.Update(dc, dc.HelpingActions);
            }
        }
        public int Delete()
        {
            using (CCEntities dc = new CCEntities())
            {
                return base.Delete(dc, dc.HelpingActions);
            }
        }
    }

    public class AbsHelpingActionCollection
        : AbsList<AbsHelpingAction, HelpingAction>
    {
        public void LoadAll()
        {
            using (CCEntities dc = new CCEntities())
            {
                //base.LoadAll(dc.HelpingActions);
                foreach (var c in dc.HelpingActions.ToList())
                    base.Add(new AbsHelpingAction(c));
            }
        }
    }
    public class AbsMemberActionCollection : AbsListJoin<AbsHelpingAction, HelpingAction, PL.MemberAction>
    {
        string memberID
        {
            get { return (string)joinGroupingID; }
            set { joinGroupingID = value; }
        }

        public AbsMemberActionCollection(string member_id)
            : base("MemberActionMember_ID",//HelpingAction ID
                   member_id,
                   "MemberActionActionID")
        {
            MemberAction c = new MemberAction { MemberEmail = member_id };
            base.joinGroupingID = c.MemberEmail;
        }//  Member ID


        public void LoadPreferences(string member_id)
        {
            using (CCEntities dc = new CCEntities())
            {
                //base.LoadWithJoin(dc.HelpingActions, dc.MemberActions,
                //                  new MemberAction { MemberActionMember_ID = member_id }.MemberActionMember_ID);
                memberID = member_id;
                if (dc.MemberActions.ToList().Count != 0)
                {
                    List<MemberAction> MemberActions = dc.MemberActions
                        .Where(c => c.MemberEmail == memberID).ToList();
                    if (MemberActions.Count != 0)
                    {
                        MemberActions.ForEach(b =>

                        {
                            base.Add(new AbsHelpingAction(dc.HelpingActions
                                  .Where(d => d.ID == b.ActionID)
                                  .FirstOrDefault())
                          );
                            });
                    }
                }
            }
        }
        public void DeleteAllPreferences()
        {
            using (CCEntities dc = new CCEntities())
            {
                base.DeleteAllPreferences(dc, dc.MemberActions);
            }
        }
        public new void Add(AbsHelpingAction HelpingAction)
        {
            using (CCEntities dc = new CCEntities())
            {
                base.Add(dc, dc.MemberActions, new MemberAction(), HelpingAction);
            }
        }
        public new void Remove(AbsHelpingAction HelpingAction)
        {
            using (CCEntities dc = new CCEntities())
            {
                base.Remove(dc, dc.MemberActions, HelpingAction);
            }
        }
    }
}
