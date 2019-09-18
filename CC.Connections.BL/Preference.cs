using System;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class Preference
    {
        private object preference_ID;

        public Preference(object preference_ID)
        {
            this.preference_ID = preference_ID;
        }

        internal void Update(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        internal void Insert(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        internal void Delete(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }
    }
}