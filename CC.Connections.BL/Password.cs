using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Connections.BL
{
    //NOTE PB: 
    //  .1 hour
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

        public Password() { }
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
            this.hash = hash;
        }

        internal void Insert(DBconnections dc, int iD)
        {
            ID = dc.Helping_Action.Max(c => c.Helping_Action_ID) + 1;
            PL.Log_in entry = new PL.Log_in
            {
                Log_in_ID = ID,
                MemeberID = iD,
                Password = hash
            };
            dc.Log_in.Add(entry);
        }

        internal void Delete(DBconnections dc, int iD)
        {
            dc.Log_in.Remove(dc.Log_in.Where(c => c.MemeberID== ID).FirstOrDefault());
        }

        internal void Update(DBconnections dc, int iD)
        {
            PL.Log_in entry = dc.Log_in.Where(c => c.MemeberID== this.ID).FirstOrDefault();
            entry.Password = hash;
        }
    }
}
