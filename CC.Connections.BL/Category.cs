using System.Linq;
using CC.Connections.PL;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using CC.Abstract;
using System;

namespace CC.Connections.BL
{
    //TODO rename to just Category
    public class Category : ColumnEntry<PL.Category>//PRES inheritence
    {
        //Parameters


        //PRES id and other properties
        //id
        public new Guid ID
        {
            get { return (Guid)base.ID; }//ID is first column
            set { base.ID = value; }
        }

        //other parameters from PL.Categories
        [DisplayName("Description")]
        //must be the same name as the PL class
        public string Desc
        {
            get { return (string)getProperty(nameof(Desc)); }
            set { setProperty(nameof(Desc), value); }
        }
        public string Color
        {
            get { return (string)base.getProperty(nameof(Color)); }
            set { setProperty(nameof(Color), value); }
        }
        public string Image
        {
            get { return (string)base.getProperty(nameof(Image)); }
            set { setProperty(nameof(Image), value); }
        }

        //PRES constructor and CRUD
        public Category() :
            base() { }
        public Category(PL.Category entry) :
            base(entry) { }
        public Category(Guid id,bool preloaded =true) :
            base(new CCEntities().Categories, id,preloaded) {
            this.ID = id;
            if (preloaded)
                LoadId(true);
        }
        //turns a PL class into a BL equivelent example: 
        //Category pl = new Category();
        //...
        //AbsCategory bl = (AbsCategory)pl
        public static implicit operator Category(PL.Category entry)
        { return new Category(entry); }
        public void SetCategory(Category c)
        {
            this.ID = c.ID;
            this.Color = c.Color;
            this.Desc = c.Desc;
            this.Image = c.Image;
        }


        public void LoadId(bool preloaded =true){
            if (preloaded == false)//definitly needs to be taken from database
                using (CCEntities dc = new CCEntities()){
                    base.LoadId(dc.Categories);//should only need to be called from INSTANCE
                }
            else
            {
                Category loadC = CategoryList.INSTANCE.Where(c => c.ID == this.ID).FirstOrDefault();
                if (loadC != null)//retreive from existing
                    this.SetCategory(loadC);
                else//load from database
                    LoadId(false);
            }
        }
        public void LoadId(Guid id,bool preloaded =true)
        {
            ID = id;
            LoadId(preloaded);
        }
        public void Insert() {
            using (CCEntities dc = new CCEntities()){
                base.Insert(dc, dc.Categories);
                CategoryList.AddToInstance(this);
            }
        }
        public void Update(){
            using (CCEntities dc = new CCEntities()){
                base.Update(dc, dc.Categories);
                CategoryList.UpdateInstance(this);
            }
        }
        public void Delete(){
            using (CCEntities dc = new CCEntities()){
                base.Insert(dc, dc.Categories);
                CategoryList.RemoveInstance(this);
            }
        }
    }

    //STOP HERE
    public class CategoryList : AbsList<Category, PL.Category>
    {
        private static CategoryList INS = new CategoryList();
        public static CategoryList INSTANCE
        {
            get
            {
                if (INS == null || INS.Count == 0)
                    INS = LoadInstance();
                return INS;
            }
            private set => INS = value;
        }

        public static CategoryList LoadInstance()
        {
            try
            {
                INSTANCE = new CategoryList();
                using (CCEntities dc = new CCEntities())
                {
                    foreach (var c in dc.Categories.ToList())
                        INS.Add(new Category(c));
                }
                return INS;
            }
            catch (EntityException e) { throw e.InnerException; }
        }
        public static void AddToInstance(Category category)
        {
            INS.Add(category);
        }
        //Might be able to optimize better
        internal static void UpdateInstance(Category category)
        {
            RemoveInstance(category);
            AddToInstance(category);
        }

        internal static void RemoveInstance(Category category)
        {
            INSTANCE.Remove(category);
        }

        public void LoadAll()
        {
            this.Clear();
            this.AddRange(INS);
        }
        //hides categories that are unused
        public new void LoadUsed()
        {
            try{
                using (CCEntities dc = new CCEntities()){
                    //base.LoadAll(dc.Categories);
                    foreach (var c in INSTANCE)
                        if(CharityList.INSTANCE.Where(d=>d.Category.ID == c.ID).Any())
                            base.Add(c);
                }
            }
            catch (EntityException e)
            {
                throw e.InnerException;

            }
        }

    }
    public class CategoryPreferences : AbsListJoin<Category, PL.Category, Preferred_Category>
    {
        string volunteerEmail
        {
            get { return (string)joinGrouping_ID; }
            set { joinGrouping_ID = value; }
        }


        public CategoryPreferences(string member_id)
            : base("MemberCat_Category_ID",member_id, "MemberCat_Member_ID")
        {
            Preferred_Category c = new Preferred_Category { Volunteer_Email = member_id };
            base.joinGrouping_ID = c.Volunteer_Email;
        }

        public new void LoadPreferences()
        { LoadPreferences((string)base.joinGrouping_ID); }
        public new void LoadPreferences(string volunteer_Email){
            using (CCEntities dc = new CCEntities()){
                volunteerEmail = volunteer_Email;
                if (dc.Categories.ToList().Count != 0)
                    dc.Preferred_Category
                        .Where(c => c.Volunteer_Email == volunteerEmail).ToList()
                        .ForEach(b => base.Add(new Category(dc.Categories
                            .Where(d =>
                                d.ID == b.Category_ID
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