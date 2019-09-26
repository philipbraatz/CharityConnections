using System;
using System.Linq;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    //NOTE PB:
    // .25 hours
    public class Member_Type
    {

        public int ID { get; set; }
        public string Desc { get; set; }

        public Member_Type(int id)
        {
            this.ID = id;
            LoadId();
        }

        public Member_Type() { }

        public int Insert()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Member_Type.ToList().Count > 0)
                        ID = dc.Member_Type.Max(c => c.MemberType_ID) + 1;//unique id
                    else
                        ID = 0;
                    PL.Member_Type entry = new PL.Member_Type
                    {
                        MemberType_ID = ID,
                        MemberTypeDescription = Desc
                    };

                    dc.Member_Type.Add(entry);
                    dc.SaveChanges();
                    return ID;
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

                    dc.Member_Type.Remove(dc.Member_Type.Where(c => c.MemberType_ID == ID).FirstOrDefault());

                    this.Desc = string.Empty;
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

                    PL.Member_Type entry = dc.Member_Type.Where(c => c.MemberType_ID == this.ID).FirstOrDefault();
                    entry.MemberTypeDescription = Desc;

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

                    PL.Member_Type entry = dc.Member_Type.FirstOrDefault(c => c.MemberType_ID == this.ID);
                    if (entry == null)
                        throw new Exception("Member_Type does not exist");

                    Desc = entry.MemberTypeDescription;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}