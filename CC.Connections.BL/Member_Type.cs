using System;
using System.Linq;
using CC.Connections.PL;
using System.ComponentModel;

namespace CC.Connections.BL
{
    //only developers should be inserting, updating and deleting type
    //The 2 main types are volunteer and charity
    public class Member_Type : PL.Member_Type
    {
        [DisplayName("Description")]
        public new string MemberTypeDescription { get; set; }

        public Member_Type(int id)
        {
            this.MemberType_ID = id;
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
                        MemberType_ID = dc.Member_Type.Max(c => c.MemberType_ID) + 1;//unique id
                    else
                        MemberType_ID = 0;
                    PL.Member_Type entry = new PL.Member_Type
                    {
                        MemberType_ID = MemberType_ID,
                        MemberTypeDescription = MemberTypeDescription
                    };

                    dc.Member_Type.Add(entry);
                    dc.SaveChanges();
                    return MemberType_ID;
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

                    dc.Member_Type.Remove(dc.Member_Type.Where(c => c.MemberType_ID == MemberType_ID).FirstOrDefault());

                    this.MemberTypeDescription = string.Empty;
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

                    PL.Member_Type entry = dc.Member_Type.Where(c => c.MemberType_ID == this.MemberType_ID).FirstOrDefault();
                    entry.MemberTypeDescription = MemberTypeDescription;

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

                    PL.Member_Type entry = dc.Member_Type.FirstOrDefault(c => c.MemberType_ID == this.MemberType_ID);
                    if (entry == null)
                        throw new Exception("Member Type does not exist: ID = "+this.MemberType_ID);

                    MemberTypeDescription = entry.MemberTypeDescription;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}