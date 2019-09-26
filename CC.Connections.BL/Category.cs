﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    //NOTE PB:
    //  .5 hours CRUD and Member CRUD
    public class Category
    {

        public Category() { }
        public Category(int memberCat_ID)
        {
            this.ID = memberCat_ID;
            LoadId();
        }

        public int ID { get; set; }
        public string Desc { get; set; }
        public int Count { get; set; }

        private bool Exists(DBconnections dc)
        {
            return dc.Categories.Where(c => c.Category_ID == ID).FirstOrDefault() != null;
        }

        public int Insert()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Categories.ToList().Count > 0)
                        ID = dc.Categories.Max(c => c.Category_ID) + 1;//unique id
                    else
                        ID = 0;

                    PL.Category entry = new PL.Category
                    {
                        Category_ID = ID,
                        Category_Desc = this.Desc
                    };

                    dc.Categories.Add(entry);
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

                    dc.Categories.Remove(dc.Categories.Where(c => c.Category_ID == ID).FirstOrDefault());
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

                    PL.Category entry = dc.Categories.Where(c => c.Category_ID == this.ID).FirstOrDefault();
                    entry.Category_Desc = Desc;

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

                    PL.Category entry = dc.Categories.FirstOrDefault(c => c.Category_ID == this.ID);
                    if (entry == null)
                        throw new Exception("Category does not exist ID: "+ID);

                    Desc = entry.Category_Desc;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class CategoryList
        : List<Category>
    {
        int? member_ID { get; set; }//only used for preferences
        private const string PREFERENCE_LOAD_ERROR = "Preferences not loaded, please loadPrefences with a Member ID";
        public void LoadList()
        {
            try
            {
                this.Clear();
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Categories.ToList().Count != 0)
                        dc.Categories.ToList().ForEach(c => this.Add(new Category
                        {
                            ID = c.Category_ID,
                            Desc = c.Category_Desc
                        }));
                }
            }
            catch (Exception e) { throw e; }
        }

        public CategoryList LoadPreferences(int memberID)
        {
            try
            {
                this.Clear();
                member_ID = memberID;
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Categories.ToList().Count != 0)
                        dc.Preferred_Category.Where(d => d.MemberCat_Member_ID == memberID).ToList().ForEach(c =>
                             this.Add(new Category((int)c.MemberCat_Category_ID)));
                }
                return this;
            }
            catch (Exception e) { throw e; }
        }
        public void AddPreference(int categoryID)
        {
            if (member_ID == null)
                throw new Exception(PREFERENCE_LOAD_ERROR);

            using (DBconnections dc = new DBconnections())
            {
                this.Clear();
                int newID = 0;
                if (dc.Preferred_Category.ToList().Count != 0)
                    newID = dc.Preferred_Category.Max(c => c.PreferredCategory_ID)+1;
                dc.Preferred_Category.Add(new Preferred_Category {
                PreferredCategory_ID = newID,
                MemberCat_Member_ID = member_ID,
                MemberCat_Category_ID = categoryID
                });
                this.Add(new Category(categoryID));
                dc.SaveChanges();
            }
        }

        public void DeletePreference( int categoryID)
        {
            if (member_ID == null)
                throw new Exception(PREFERENCE_LOAD_ERROR);

            using (DBconnections dc = new DBconnections())
            {
                PL.Preferred_Category prefCat = dc.Preferred_Category.Where(
                    c => c.MemberCat_Member_ID == member_ID &&
                    c.MemberCat_Category_ID == categoryID).FirstOrDefault();
                if (prefCat != null)
                    dc.Preferred_Category.Remove(prefCat);
                else
                    throw new Exception("Prefferred_Category category "+categoryID+" does not exist for Member "+member_ID);
                this.Remove(this.Where(c => c.ID == categoryID).FirstOrDefault()) ;
                dc.SaveChanges();
            }
        }
        public void UpdateCategory(Category cat, string description)
        {
            Category cthis =this.Where(c => c.ID == cat.ID).FirstOrDefault();
            cthis.Desc = description;
            cthis.Update();

        }
        public void UpdateCategory(int catID,string description)
        {
            Category cthis = this.Where(c => c.ID == catID).FirstOrDefault();
            cthis.Desc = description;
            cthis.Update();
        }

        private bool MemberExists(DBconnections dc, int member_ID)
        {
            return dc.Preferred_Category.Where(c => c.MemberCat_Member_ID == member_ID
            ).FirstOrDefault() != null;
        }

        public new void Clear()
        {
            base.Clear();
            member_ID = null;
        }
    }
}