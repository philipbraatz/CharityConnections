using System.Linq;
using Doorfail.Connections.PL;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.Entity.Core;
using Doorfail.DataConnection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Doorfail.Connections.BL
{
    //TODO rename to just Category
    public class Category : CrudModel_Json<PL.Category>//PRES inheritence
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
            get { return (string)base.getProperty(nameof(Image)) ?? "NAN"; }
            set { setProperty(nameof(Image), value); }
        }

        //PRES constructor and CRUD
        public Category() : base() { }
        public Category(PL.Category entry) : base(entry) { }
        public Category(Guid id,bool preloaded =true) :
            base(JsonDatabase.GetTable<PL.Category>(), id,preloaded) {
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
            if (c is null)
                throw new ArgumentNullException(nameof(c));

            setProperties(c);
        }


        public void LoadId(bool preloaded =true){
            if (preloaded == false)//definitly needs to be taken from database
                if(false)
                    using(CCEntities dc = new CCEntities()){
                        //base.LoadId(dc.Categories);//should only need to be called from INSTANCE
                    }
                else
                    base.LoadId(JsonDatabase.GetTable<PL.Category>());//should only need to be called from INSTANCE
            else
            {
                Category loadC = CategoryCollection.INSTANCE.Where(c => c.ID == this.ID).FirstOrDefault();
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
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.Insert(dc, dc.Categories);
                    CategoryCollection.AddToInstance(this);
                }
            else
                base.Insert(JsonDatabase.GetTable<PL.Category>());
        }
        public void Update(){
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.Update(dc, dc.Categories);
                    CategoryCollection.UpdateInstance(this);
                }
            else
                base.Update(JsonDatabase.GetTable<PL.Category>());
        }
        public void Delete(){
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.Delete(dc, dc.Categories);
                    CategoryCollection.RemoveInstance(this);
                }
            else
                base.Delete(JsonDatabase.GetTable<PL.Category>());
        }
    }

    public class CategoryCollection : CrudModelList<Category, PL.Category>
    {
        private static CategoryCollection ins = new CategoryCollection();

        public CategoryCollection() :base(){}
        public CategoryCollection(Category[] categories):base(categories){}
        public CategoryCollection(PL.Category[] categories) : base(categories){}

        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.BL.Test")]
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.API")]
        public static CategoryCollection INSTANCE
        {
            get
            {
                if (ins == null || ins.Count == 0)
                    ins = LoadInstance();
                return ins;
            }
            private set => ins = value;
        }

        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.BL.Test")]
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.API")]
        public static CategoryCollection LoadInstance()
        {
            try
            {
                ins = new CategoryCollection();
                if(false)
                    using (CCEntities dc = new CCEntities())
                    {
                        foreach (var c in dc.Categories.ToList())
                            ins.Add(new Category(c));
                    }
                else
                {
                    foreach (var c in JsonDatabase.GetTable<PL.Category>().ToList())
                        ins.Add(new Category(c));
                }

                return ins;
            }
            catch (EntityException e) { throw e.InnerException; }
        }

        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.BL.Test")]
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.API")]
        public static void AddToInstance(Category category)
        {
            ins.Add(category);
        }

        //Might be able to optimize better
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.BL.Test")]
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.API")]
        public static void UpdateInstance(Category category)
        {
            RemoveInstance(category);
            AddToInstance(category);
        }

        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.BL.Test")]
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.API")]
        public static void RemoveInstance(Category category)
        {
            INSTANCE.Remove(category);
        }


        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.BL.Test")]
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.API")]
        public Category[] LoadAll()
        {
            this.Clear();
            LoadInstance();//make sure Instance is filled
            this.AddRange(ins);
            return this.ToArray();
        }

        ////hides categories that are unused
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.BL.Test")]
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.API")]
        public void LoadUsed()
        {
            try{
                //base.LoadAll(dc.Categories);
                foreach (var c in INSTANCE)
                    if(CharityCollection.INSTANCE.Where(d=>d.Category.ID == c.ID).Any())
                        base.Add(c);

            }
            catch (EntityException e)
            {
                throw e.InnerException;

            }
        }

    }
    public class CategoryPreferenceCollection : AbsListJoin<Category, PL.Category, PreferredCategory>
    {
        string volunteerEmail
        {
            get { return (string)joinGroupingID; }
            set { joinGroupingID = value; }
        }


        public CategoryPreferenceCollection(string member_id)
            : base("MemberCat_CategoryID",member_id, "MemberCat_Member_ID")
        {
            PreferredCategory c = new PreferredCategory { VolunteerEmail = member_id };
            base.joinGroupingID = c.VolunteerEmail;
        }

        public void LoadPreferences()
        { LoadPreferences((string)base.joinGroupingID); }
        public void LoadPreferences(string VolunteerEmail){
            volunteerEmail = VolunteerEmail;

            if(false)
            using (CCEntities dc = new CCEntities()){
                if (dc.Categories.ToList().Count != 0)
                    dc.PreferredCategories
                        .Where(c => c.VolunteerEmail == volunteerEmail).ToList()
                        .ForEach(b => base.Add(new Category(dc.Categories
                            .Where(d =>
                                d.ID == b.CategoryID
                            ).FirstOrDefault()))
                        );
            }
            else if (JsonDatabase.GetTable<PL.Category>().ToList().Count != 0)
                JsonDatabase.GetTable<PL.PreferredCategory>()
                    .Where(c => c.VolunteerEmail == volunteerEmail).ToList()
                    .ForEach(b => base.Add(JsonDatabase.GetTable<PL.Category>()
                        .Where(d =>
                            d.ID == b.CategoryID
                        ).FirstOrDefault())
                    );

        }
        public void DeleteAllPreferences(){
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.DeleteAllPreferences(dc, dc.PreferredCategories);
                }
            else
                base.DeleteAllPreferences(JsonDatabase.GetTable<PL.PreferredCategory>());
        }
        public new void Add(Category category){
            if(false)
            using (CCEntities dc = new CCEntities()){
                base.Add(dc, dc.PreferredCategories,new PreferredCategory(), category);
            }
            else
                base.Add(JsonDatabase.GetTable<PL.PreferredCategory>(), new PreferredCategory(), category);
        }
        public new void Remove(Category category){
            if(false)
            using (CCEntities dc = new CCEntities()){
                base.Remove(dc, dc.PreferredCategories,category);
            }
            else
                base.Remove(category);
        }
    }
}