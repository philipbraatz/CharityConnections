using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    interface Icrud<T>{
        void Clear(DbSet<T> table);
        bool Exists(DBconnections dc);
        void LoadId();
        int Insert();
        int Delete();
        int Update();
    }

    interface IcrudList
    {
        void LoadAll();
    }

    class Category2 : PL.Category, Icrud
    {
        public void Clear()
        {
            throw new NotImplementedException();
        }

        public int Delete()
        {
            throw new NotImplementedException();
        }

        public bool Exists(DBconnections dc)
        {
            throw new NotImplementedException();
        }

        public int Insert()
        {
            throw new NotImplementedException();
        }

        public void LoadId()
        {
            throw new NotImplementedException();
        }

        public int Update()
        {
            throw new NotImplementedException();
        }
    }
}
