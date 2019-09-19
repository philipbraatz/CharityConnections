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
        internal void InsertMember(DBconnections dc, int id)
        {
            //TODO check if new category or new member first
            Member_Categories entry = new Member_Categories
            {
                MemberCat_ID = (int)dc.Member_Categories.Max(c => c.MemberCat_Categories_ID) + 1,
                MemberCat_Member_ID = id,//member
                MemberCat_Categories_ID = ID//category
            };

            dc.Member_Categories.Add(entry);
        }
        //TODO test methods
        internal void DeleteMember(DBconnections dc, int id)
        {
            var entryList= dc.Member_Categories.Where(c => c.MemberCat_Member_ID == id);
            foreach(var entry in entryList)
                dc.Member_Categories.Remove(entry);
        }

        internal void UpdateMember(DBconnections dc, int id)
        {
            dc.Member_Categories.Where(c => c.MemberCat_Member_ID == id).ToList().ForEach(c => c.MemberCat_Categories_ID = ID);//might not work
        }

        internal static List<Category> LoadMembersList(DBconnections dc,int member_ID)
        {
            List<Category> retList =new List<Category>();
            dc.Member_Categories.Where(c => c.MemberCat_Member_ID == member_ID).ToList().ForEach(c => retList.Add(new Category( c.MemberCat_ID)));
            return retList;
        }

        public int Insert()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    ID = dc.Categories.Max(c => c.Categories_ID) + 1;
                    PL.Category entry = new PL.Category
                    {
                        Categories_ID = ID,
                        Categories_Desc = this.Desc
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

                    dc.Categories.Remove(dc.Categories.Where(c => c.Categories_ID == ID).FirstOrDefault());
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

                    PL.Category entry = dc.Categories.Where(c => c.Categories_ID == this.ID).FirstOrDefault();
                    entry.Categories_Desc = Desc;

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

                    PL.Category entry = dc.Categories.FirstOrDefault(c => c.Categories_ID == this.ID);
                    if (entry == null)
                        throw new Exception("Genre does not exist");

                    Desc = entry.Categories_Desc;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


    }
}