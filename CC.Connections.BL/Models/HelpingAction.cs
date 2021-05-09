using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doorfail.DataConnection;
using Doorfail.Connections.PL;

namespace Doorfail.Connections.BL
{
    public class HelpingAction
        : CrudModel_Json<PL.HelpingAction>
    {
        //private static CCEntities dc;

        //id
        public new Guid ID
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
            base(JsonDatabase.GetTable<PL.HelpingAction>(), id)
        {
            Clear();
            ID = id;
            LoadId();
        }

        public void LoadId()
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.LoadId(JsonDatabase.GetTable<PL.HelpingAction>());
                }
            else
                base.LoadId(JsonDatabase.GetTable<PL.HelpingAction>());
        }
        public int Insert()
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    return base.Insert(dc, dc.HelpingActions);
                }
            else
                base.Insert(JsonDatabase.GetTable<PL.HelpingAction>());
            return 1;
        }
        public int Update()
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    return base.Update(dc, dc.HelpingActions);
                }
            else
                base.Update(JsonDatabase.GetTable<PL.HelpingAction>());
            return 1;
        }
        public int Delete()
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    return base.Delete(dc, dc.HelpingActions);
                }
            else
                base.Delete(JsonDatabase.GetTable<PL.HelpingAction>());
            return 1;
        }
    }

    public class HelpingActionCollection
        : CrudModelList<HelpingAction, PL.HelpingAction>
    {
        public void LoadAll()
        {
            if(false)
            using (CCEntities dc = new CCEntities())
            {
                //base.LoadAll(dc.HelpingActions);
                foreach (var c in dc.HelpingActions.ToList())
                    base.Add(new HelpingAction(c));
            }
            else foreach (var c in JsonDatabase.GetTable<PL.HelpingAction>().ToList())
                    base.Add(new HelpingAction(c));
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
            //base.LoadWithJoin(dc.HelpingActions, dc.MemberActions,
                //                  new MemberAction { MemberActionMember_ID = member_id }.MemberActionMember_ID);
            memberID = member_id;
            List<MemberAction> MemberActions = null;

            if (false)
            using (CCEntities dc = new CCEntities())
            {
                if (dc.MemberActions.ToList().Count != 0)
                {
                     MemberActions = dc.MemberActions
                        .Where(c => c.MemberEmail == memberID).ToList();
                }
            }
            else
            {
                if (JsonDatabase.GetTable<PL.MemberAction>().ToList().Count != 0)
                {
                    MemberActions = JsonDatabase.GetTable<PL.MemberAction>()
                        .Where(c => c.MemberEmail == memberID).ToList();
                }
            }

            if (MemberActions != null && MemberActions.Count != 0)
            {
                MemberActions.ForEach(b => {
                    base.Add(new HelpingAction(JsonDatabase.GetTable<PL.HelpingAction>()
                            .Where(d => d.ID == b.ActionID)
                            .FirstOrDefault())
                    );});
            }
        }
        public void DeleteAllPreferences()
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.DeleteAllPreferences(dc, dc.MemberActions);
                }
            else
                base.DeleteAllPreferences(JsonDatabase.GetTable<PL.MemberAction>());
        }
        public new void Add(HelpingAction HelpingAction)
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.Add(dc, dc.MemberActions, new MemberAction(), HelpingAction);
                }
            throw new NotImplementedException();
        }
        public new void Remove(HelpingAction HelpingAction)
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.Remove(dc, dc.MemberActions, HelpingAction);
                }
            else
                base.Remove(HelpingAction);
        }
    }
}
