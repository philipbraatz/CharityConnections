using System;
using System.Linq;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class Member_Type
    {

        public int ID { get; set; }
        public string Desc { get; set; }

        public Member_Type(int id)
        {
            this.ID = id;
        }
        public int Insert()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    ID = dc.Member_Type.Max(c => c.Member_Type_ID) + 1;
                    PL.Member_Type entry = new PL.Member_Type
                    {
                        Member_Type_ID = ID,
                        Member_Type_Desc =Desc
                    };

                    dc.Member_Type.Add(entry);
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

                    dc.Member_Type.Remove(dc.Member_Type.Where(c => c.Member_Type_ID == ID).FirstOrDefault());
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

                    PL.Member_Type entry = dc.Member_Type.Where(c => c.Member_Type_ID == this.ID).FirstOrDefault();
                    entry.Member_Type_Desc = Desc;

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

                    PL.Member_Type entry = dc.Member_Type.FirstOrDefault(c => c.Member_Type_ID == this.ID);
                    if (entry == null)
                        throw new Exception("Genre does not exist");

                    Desc = entry.Member_Type_Desc;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}