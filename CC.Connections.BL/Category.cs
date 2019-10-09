using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;
using System.Diagnostics;
using System.ComponentModel;

namespace CC.Connections.BL
{
    public class BLCategory : PL.Category
    {
        //You have to redeclare the varible to give it a display name
        [DisplayName("Description")]
        public new string Category_Desc { get; set; }

        public BLCategory() { }
        public BLCategory(int memberCat_ID)
        {
            this.Category_ID = memberCat_ID;
            LoadId();
        }
        public BLCategory(int memberCat_ID,bool debug)
        {
            this.Category_ID = memberCat_ID;
            LoadId(debug);
        }

        private bool Exists(DBconnections dc)
        {
            return dc.Categories.Where(c => c.Category_ID == Category_ID).FirstOrDefault() != null;
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
                        Category_ID = dc.Categories.Max(c => c.Category_ID) + 1;//unique id
                    else
                        Category_ID = 0;

                    dc.Categories.Add(this.ToPL());
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        private PL.Category ToPL()
        {
            return new PL.Category {
                Category_Desc =Category_Desc,
                Category_ID =Category_ID
            };
        }

        public int Delete()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    dc.Categories.Remove(this.ToPL());
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

                    PL.Category entry = dc.Categories.Where(c => c.Category_ID == this.Category_ID).FirstOrDefault();
                    entry.Category_Desc = Category_Desc;

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public void LoadId(bool debug =false)
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Category entry = dc.Categories.FirstOrDefault(c => c.Category_ID == this.Category_ID);
                    if (entry == null)
                        if (!debug)
                            throw new Exception("Category does not exist ID: " + Category_ID);
                        else
                        {
                            Debug.WriteLine("Category does not exist ID: " + Category_ID);
                            entry = new PL.Category { Category_ID = Category_ID, Category_Desc = "DEBUG: not found"};
                        }

                    Category_Desc = entry.Category_Desc;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class CategoryList
        : List<BLCategory>
    {
        private int? member_ID { get; set; }//only used for preferences
        private const string PREFERENCE_LOAD_ERROR = "Preferences not loaded, please loadPrefences with a Member ID";
        public void LoadList()
        {
            try
            {
                this.Clear();
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Categories.ToList().Count != 0)
                        dc.Categories.ToList().ForEach(c => this.Add((BLCategory)c));
                }
            }
            catch (Exception e) { throw e; }
        }

        public CategoryList LoadPreferences(int memberID,bool debug =false)
        {
            try
            {
                this.Clear();
                member_ID = (int?)memberID;
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Categories.ToList().Count != 0)
                        dc.Preferred_Category.Where(d => d.MemberCat_Member_ID == memberID).ToList().ForEach(c =>
                             this.Add(new BLCategory((int)c.MemberCat_Category_ID, debug),true));
                }
                return this;
            }
            catch (Exception e) { throw e; }
        }
        public void AddPreference(int categoryID,bool debug =false)
        {
            if (member_ID == null)
                throw new Exception(PREFERENCE_LOAD_ERROR);

            using (DBconnections dc = new DBconnections())
            {
                int newID = 0;
                if (dc.Preferred_Category.ToList().Count != 0)
                    newID = dc.Preferred_Category.Max(c => c.PreferredCategory_ID)+1;

                dc.Preferred_Category.Add(new Preferred_Category {
                PreferredCategory_ID = newID,
                MemberCat_Member_ID = member_ID,
                MemberCat_Category_ID = categoryID
                });
                this.Add(new BLCategory(categoryID),true);
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
                else//TODO FIX
                    categoryID= categoryID;//throw new Exception("Prefferred_Category category "+categoryID+" does not exist for Member "+member_ID);
                this.Remove(this.Where(c => c.Category_ID == categoryID).FirstOrDefault(),true);//might need to be casted to PL first
                dc.SaveChanges();
            }
        }

        internal void DeleteAllPreferences()
        {
            if (member_ID == null)
                throw new Exception(PREFERENCE_LOAD_ERROR);

            using (DBconnections dc = new DBconnections())
            {
                dc.Preferred_Category.RemoveRange(dc.Preferred_Category.Where(c => 
                c.MemberCat_Member_ID == member_ID).ToList());
                this.Clear();
                dc.SaveChanges();
            }
        }

        public void UpdateCategory(BLCategory cat, string description)
        {
            UpdateCategory(cat.Category_ID, description);

        }
        public void UpdateCategory(int catID,string description)
        {
            BLCategory cthis = this.Where(c => c.Category_ID == catID).FirstOrDefault();
            cthis.Category_Desc = description;
            cthis.Update();
        }

        private bool MemberExists(DBconnections dc, int member_ID)
        {
            return dc.Preferred_Category.Where(c => 
                c.MemberCat_Member_ID == member_ID
            ).FirstOrDefault() != null;
        }

        public new void Clear()
        {
            base.Clear();
            member_ID = null;
        }
        
        //Calls base method for interal use
        //could be replaced with base.Add()
        private void Add(BLCategory item,bool overrideMethod =true)
        {
            base.Add(item);
        }
        private void Remove(BLCategory item, bool overrideMethod = true)
        {
            base.Remove(item);
        }

        //public Add and Remove list
        //could get confused with preference list because this is shared
        public new void Add(BLCategory item)
        {
            if (member_ID != null)
                throw new Exception("Currently being used as a preference list. Please use AddPrefrence instead");
            base.Add(item);
        }
        public new void Remove(BLCategory item)
        {
            if (member_ID != null)
                throw new Exception("Currently being used as a preference list. Please use DeletePrefrence instead");
            base.Remove(item);
        }
    }
}