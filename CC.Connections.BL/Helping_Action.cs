﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class Helping_Action
    {
        public int ID { get; set; }
        public Category category { get; set; }
        //TODO ask team to rename Helping Action Desc to Action
        public string Action { get; set; }

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
