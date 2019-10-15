using System;
using System.Linq;
using CC.Connections.PL;
using System.ComponentModel;

namespace CC.Connections.BL
{
    //only developers should be inserting, updating and deleting type
    //The 2 main types are volunteer and charity
    public class AbsMember_Type : ColumnEntry<PL.Member_Type>
    {
        //id
        public new int ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }
        [DisplayName("Description")]
        public new string MemberTypeDescription {
            get { return (string)base.getProperty("MemberTypeDescription"); }
            set { setProperty("MemberTypeDescription", value); }
        }

        public AbsMember_Type(int id) :
            base(new DBconnections().Member_Type, id)
        {
            Clear();
            ID = id;
            LoadId();
        }
        public AbsMember_Type(PL.Member_Type entry) :
            base(entry){ }
        public AbsMember_Type():
            base(new Member_Type()){ }

        public static implicit operator AbsMember_Type(PL.Member_Type entry)
        { return new AbsMember_Type(entry); }

        public void LoadId()
        {
            using (DBconnections dc = new DBconnections())
            {
                base.LoadId(dc.Member_Type);
            }
        }
        public int Insert()
        {
            using (DBconnections dc = new DBconnections())
            {
                return base.Insert(dc, dc.Member_Type);
            }
        }
        public int Update()
        {
            using (DBconnections dc = new DBconnections())
            {
                return base.Update(dc, dc.Member_Type);
            }
        }
        public int Delete()
        {
            using (DBconnections dc = new DBconnections())
            {
                //dc.Member_Type.Remove(this);
                //return dc.SaveChanges();
                return base.Delete(dc, dc.Member_Type);
            }
        }
    }

    public class AbsMember_TypeList : AbsList<AbsMember_Type, Member_Type>
    {
        public new void LoadAll()
        {
            using (DBconnections dc = new DBconnections())
            {
                base.LoadAll(dc.Member_Type);
            }
        }
    }
}