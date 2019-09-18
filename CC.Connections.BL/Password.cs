using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Connections.BL
{
    public class Password
    {
        public int ID { get; set; }
        private string hash;
        public string Hash
        {
            //returns hashed password
            get { return hash; }
            //set with normal password
            set
            {
                using (var sha1 = new System.Security.Cryptography.SHA1Managed())
                {
                    var hashbytes = System.Text.Encoding.UTF8.GetBytes(value);
                    hash = Convert.ToBase64String(sha1.ComputeHash(hashbytes));
                }
            }
        }
        //new password
        public Password(string password)
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    ID = dc.Log_in.Max(m => m.Log_in_ID);
                }
                Hash = password;//auto hashs password
            }
            catch (Exception e)
            {throw e;}
        }
        //existing
        public Password(int id, string hash)
        {
            ID = id;
            this.hash =hash
        }

        internal void Insert(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        internal void Delete(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        internal void Update(DBconnections dc, int iD)
        {
            throw new NotImplementedException();
        }

        internal class FromID : Password
        {
            private int member_ID;

            public FromID(int member_ID)
            {
                this.member_ID = member_ID;
            }
        }
    }
}
