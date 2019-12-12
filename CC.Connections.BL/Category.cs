using System.Linq;
using CC.Connections.PL;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using CC.Abstract;

namespace CC.Connections.BL
{
    public class Category : ColumnEntry<PL.Categories>
    {
        //Parameters

        //id
        public new int ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }

        //other parameters from PL.Categories
        [DisplayName("Description")]
        //must be the same name as the PL class
        public string Category_Desc
        {
            get { return (string)base.getProperty("Category_Desc"); }
            set { setProperty("Category_Desc", value); }
        }
        public string Category_Color
        {
            get { return (string)base.getProperty("Category_Color"); }
            set { setProperty("Category_Color", value); }
        }
        public string Category_Image
        {
            get { return (string)base.getProperty("Category_Image"); }
            set { setProperty("Category_Image", value); }
        }

        public Category() : 
            base(){ }
        public Category(PL.Categories entry) : 
            base(entry) { }
        public Category(int id) : 
            base(new CCEntities().Categories, id){}
        //turns a PL class into a BL equivelent example: 
        //Category pl = new Category();
        //...
        //AbsCategory bl = (AbsCategory)pl
        public static implicit operator Category(PL.Categories entry)
        {return new Category(entry);}

        public void LoadId(){
            using (CCEntities dc = new CCEntities()){
                base.LoadId(dc.Categories);
            }
        }
        public void LoadId(int id)
        {
            ID = id;
            LoadId();
        }
        public int Insert() {
            using (CCEntities dc = new CCEntities()){
                return base.Insert(dc, dc.Categories);
            }
        }
        public int Update()
        {
            using (CCEntities dc = new CCEntities())
            {
                return base.Update(dc, dc.Categories);
            }
        }
        public int Delete()
        {
            using (CCEntities dc = new CCEntities())
            {
                //dc.Categories.Remove(this);
                //return dc.SaveChanges();
                return base.Delete(dc, dc.Categories);
            }
        }
    }

    public class CategoryList : AbsList<Category, Categories>
    {
        public new void LoadAll()
        {
            try
            {
            using (CCEntities dc = new CCEntities())
            {
                //base.LoadAll(dc.Categories);
                foreach (var c in dc.Categories.ToList())
                    base.Add(new Category(c));
            }
            }
            catch (EntityException e)
            {
                throw e.InnerException;

            }
        }
        //hides categories that are unused
        public new void LoadUsed()
        {
            try{
                using (CCEntities dc = new CCEntities()){
                    //base.LoadAll(dc.Categories);
                    foreach (var c in dc.Categories.ToList())
                        if(dc.Charities.Where(d=>d.Charity_Category_ID == c.Category_ID).Count() != 0)
                            base.Add(new Category(c));
                }
            }
            catch (EntityException e)
            {
                throw e.InnerException;

            }
        }
    }
    public class AbsCategoryPreferences : AbsListJoin<Category, Categories, Preferred_Category>
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
            using (CCEntities dc = new CCEntities()){
                memberID = member_id;
                if (dc.Categories.ToList().Count != 0)
                    dc.Preferred_Category
                        .Where(c => c.MemberCat_Member_ID == memberID).ToList()
                        .ForEach(b => base.Add(new Category(dc.Categories
                            .Where(d =>
                                d.Category_ID == b.MemberCat_Category_ID
                            ).FirstOrDefault()))
                        );
            }
        }
        public new void DeleteAllPreferences(){
            using (CCEntities dc = new CCEntities()){
                base.DeleteAllPreferences(dc, dc.Preferred_Category);
            }
        }
        public new void Add(Category category){
            using (CCEntities dc = new CCEntities()){
                base.Add(dc, dc.Preferred_Category,new Preferred_Category(), category);
            }
        }
        public new void Remove(Category category){
            using (CCEntities dc = new CCEntities()){
                base.Remove(dc, dc.Preferred_Category,category);
            }
        }
    }
}