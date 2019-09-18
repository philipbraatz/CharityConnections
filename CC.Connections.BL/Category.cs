using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class Category
    {
        int ID { get; set; }
        string Desc { get; set; }

        internal void InsertMember(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        internal void DeleteMember(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        internal void UpdateMember(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        internal static void LoadMembersList(int member_ID)
        {
            throw new NotImplementedException();
        }

        //TODO CRUD
    }
}
