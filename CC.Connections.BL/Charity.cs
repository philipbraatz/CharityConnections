using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class Charity
    {
        public int ID { get; set; }

        internal static List<int> LoadMembersIdList(DBconnections dc, int member_ID)
        {
            throw new NotImplementedException();
        }

        internal static void InsertMember(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        internal static void InsertMember(DBconnections dc, int iD, int char_ID)
        {
            throw new NotImplementedException();
        }

        internal static void UpdateMember(DBconnections dc, int iD, int char_ID)
        {
            throw new NotImplementedException();
        }
    }

    public class CharityList :
        List<Charity>
    {
        public void load()
        {
            throw new NotImplementedException();
        }
    }
}
