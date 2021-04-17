using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.DataConnection;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class HelpingAction
        : CrudModel_Json<PL.HelpingAction>
    {
        //private static CCEntities dc;

        //id
        public Guid ID
        {
            get { return (Guid)base.ID; }
            set { base.ID = value; }
        }
        private Category CategoryID;
        public Category category {
            get {
                if (CategoryID == null)
                {
                    var catId = base.getProperty(nameof(Category) + "ID");
                    if (catId != null)
                        CategoryID = new Category((Guid)catId);
                    else return new Category();//todo force error here
                }
                return CategoryID;
            }
            set { setProperty(nameof(CategoryID), CategoryID); }
        }
        public string Description
        {
            get { return (string)base.getProperty(nameof(Description)); }
            set { setProperty(nameof(Description), value); }
        }

        private new void Clear()
        {
            Description = string.Empty;
            category = new Category();
        }

        public HelpingAction() :
            base(new PL.HelpingAction())
        { Clear(); }
        public HelpingAction(PL.HelpingAction entry) :
            base(entry)
        { Clear(); }
        public HelpingAction(Guid id) :
            base(JsonDatabase.HelpingActions, id)
        {
            Clear();
            ID = id;
            LoadId();
        }

        public void LoadId()
        {
            using (CCEntities dc = new CCEntities())
            {
                base.LoadId(JsonDatabase.HelpingActions);
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

    public class HelpingActionCollection
        : CrudModelList<HelpingAction, PL.HelpingAction>
    {
        public void LoadAll()
        {
            using (CCEntities dc = new CCEntities())
            {
                //base.LoadAll(dc.HelpingActions);
                foreach (var c in dc.HelpingActions.ToList())
                    base.Add(new HelpingAction(c));
            }
        }
    }
    public class AbsMemberActionCollection : AbsListJoin<HelpingAction, PL.HelpingAction, PL.MemberAction>
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
                            base.Add(new HelpingAction(dc.HelpingActions
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
        public new void Add(HelpingAction HelpingAction)
        {
            using (CCEntities dc = new CCEntities())
            {
                base.Add(dc, dc.MemberActions, new MemberAction(), HelpingAction);
            }
        }
        public new void Remove(HelpingAction HelpingAction)
        {
            using (CCEntities dc = new CCEntities())
            {
                base.Remove(dc, dc.MemberActions, HelpingAction);
            }
        }
    }
}
