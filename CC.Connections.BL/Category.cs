using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;
using System.ComponentModel;

namespace CC.Connections.BL
{
    //2 hours
    public class AbsCategory : ColumnEntry<PL.Category>
    {
        //Parameters

        //id
        public new int ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }

        //other parameters from PL.Category
        [DisplayName("Description")]
        //must be the same name as the PL class
        public string Category_Desc
        {
            get { return (string)base.getProperty("Category_Desc"); }
            set { setProperty("Category_Desc", value); }
        }

        public AbsCategory() :
            base(new PL.Category()){ }
        public AbsCategory(PL.Category entry) :
            base(entry) { }
        public AbsCategory(int id) :
            base(new DBconnections().Categories, id)
        {
            Clear();
            ID = id;
            LoadId();
        }
        //turns a PL class into a BL equivelent example: 
        //Category pl = new Category();
        //...
        //AbsCategory bl = (AbsCategory)pl
        public static implicit operator AbsCategory(PL.Category entry)
        {return new AbsCategory(entry);}

        public void LoadId(){
            using (DBconnections dc = new DBconnections()){
                base.LoadId(dc.Categories);
            }
        }
        public int Insert() {
            using (DBconnections dc = new DBconnections()){
                return base.Insert(dc, dc.Categories);
            }
        }
        public int Update()
        {
            using (DBconnections dc = new DBconnections())
            {
                return base.Update(dc, dc.Categories);
            }
        }
        public int Delete()
        {
            using (DBconnections dc = new DBconnections())
            {
                //dc.Categories.Remove(this);
                //return dc.SaveChanges();
                return base.Delete(dc, dc.Categories);
            }
        }
    }

    public class AbsCategoryList : AbsList<AbsCategory, Category>
    {
        public new void LoadAll()
        {
            using (DBconnections dc = new DBconnections())
            {
                //base.LoadAll(dc.Categories);
                foreach (var c in dc.Categories.ToList())
                    base.Add(new AbsCategory(c));
            }
        }
    }
    public class AbsCategoryPreferences : AbsListJoin<AbsCategory, Category, Preferred_Category>
    {
        int memberID
        {
            get { return (int)joinGrouping_ID; }
            set { joinGrouping_ID = value; }
        }


        public AbsCategoryPreferences(int member_id)
            : base("MemberCat_Category_ID",member_id, "MemberCat_Member_ID")
        {
            Preferred_Category c = new Preferred_Category { MemberCat_Member_ID = member_id };
            base.joinGrouping_ID = c.MemberCat_Member_ID;
        }

        public new void LoadPreferences()
        { LoadPreferences((int)base.joinGrouping_ID); }
        public new void LoadPreferences(int member_id){
            using (DBconnections dc = new DBconnections()){
                memberID = member_id;
                if (dc.Categories.ToList().Count != 0)
                    dc.Preferred_Category
                        .Where(c => c.MemberCat_Member_ID == memberID).ToList()
                        .ForEach(b => base.Add(new AbsCategory(dc.Categories
                            .Where(d =>
                                d.Category_ID == b.MemberCat_Category_ID
                            ).FirstOrDefault()))
                        );
            }
        }
        public new void DeleteAllPreferences(){
            using (DBconnections dc = new DBconnections()){
                base.DeleteAllPreferences(dc, dc.Preferred_Category);
            }
        }
        public new void Add(AbsCategory category){
            using (DBconnections dc = new DBconnections()){
                base.Add(dc, dc.Preferred_Category,new Preferred_Category(), category);
            }
        }
        public new void Remove(AbsCategory category){
            using (DBconnections dc = new DBconnections()){
                base.Remove(dc, dc.Preferred_Category,category);
            }
        }
    }
}