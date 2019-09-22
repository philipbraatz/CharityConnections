using System;
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
            this.memberCat_ID = memberCat_ID;
            LoadId();
        }

        public int ID { get; set; }
        public int memberCat_ID { get; set; }
        public string Desc { get; set; }

        //takes dc and save externally!
        //Member_Categories table
        //link a existing member to an existing category
        internal bool InsertMember(DBconnections dc, int id)
        {
            if (MemberExists(dc, id))//dont add existing
                return false;
            if (!Exists(dc))//add missing
                Insert();

            int mID = 0;
            if (dc.Preferred_Category.ToList().Count > 0)
                mID = (int)dc.Preferred_Category.Max(c => c.PreferredCategory_ID) + 1;//unique id

            Preferred_Category entry = new Preferred_Category
            {
                PreferredCategory_ID = mID,
                MemberCat_Member_ID = id,//member
                MemberCat_Category_ID = ID//category
            };



            dc.Preferred_Category.Add(entry);

            dc.SaveChanges();
            return true;


        }
        //TODO test methods
        internal bool DeleteMember(DBconnections dc, int id)
        {
            if (MemberExists(dc, id))//doesnt exist
                return false;

            var entryList = dc.Preferred_Category.Where(c => c.MemberCat_Member_ID == id);
            foreach (var entry in entryList)
                dc.Preferred_Category.Remove(entry);
            return true;
        }

        internal bool UpdateMember(DBconnections dc, int id)
        {
            if (!MemberExists(dc, id))//doesnt exist
                return false;
            if (!Exists(dc))//add missing
                Insert();
            dc.Preferred_Category.Where(c => c.MemberCat_Member_ID == id).ToList().ForEach(c => c.MemberCat_Category_ID = ID);//might not work
            return true;
        }

        internal static List<Category> LoadMembersList(DBconnections dc, int member_ID)
        {
            List<Category> retList = new List<Category>();
            dc.Preferred_Category.Where(c => c.MemberCat_Member_ID == member_ID).ToList().ForEach(c => retList.Add(new Category(c.PreferredCategory_ID)));
            return retList;
        }

        private bool MemberExists(DBconnections dc, int member_ID)
        {
            return dc.Preferred_Category.Where(c => c.MemberCat_Member_ID == member_ID &&
                                                c.MemberCat_Category_ID == ID
            ).FirstOrDefault() != null;
        }
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
                        throw new Exception("Category does not exist");

                    Desc = entry.Category_Desc;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}