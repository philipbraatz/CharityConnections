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
        public string email { get; set; }
        private string hash;
        public string Pass
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
        //existing
        public Password(string email, bool load = true)
        {
            this.email = email;
            if(load)
                loadId();
        }

        private void loadId()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Log_in entry = dc.Log_in.FirstOrDefault(c => c.ContactInfoEmail == this.email);
                    if (entry == null)
                        throw new Exception("Log_in does not exist");

                    this.hash = entry.LogInPassword;
                    this.email = entry.ContactInfoEmail;
                }
            }
            catch (Exception e)
            { throw e; }
        }

        //new
        public Password(string email, string password, bool hashed = false)
        {
            this.email = email;
            if (hashed)
                this.hash = password;
            else
                this.Pass = password;
        }

        internal void Insert(DBconnections dc, int iD)
        {
            //if (dc.Log_in.ToList().Count > 0)
            //    email = (int)dc.Log_in.Max(c => c.LogInMember_ID) + 1;
            //else
            //    email = 0;

            PL.Log_in entry = new PL.Log_in
            {
                ContactInfoEmail = email,
                LogInMember_ID = iD,
                LogInPassword = hash
            };
            dc.Log_in.Add(entry);
            dc.SaveChanges();
        }

        internal void Delete(DBconnections dc)
        {
            dc.Log_in.Remove(dc.Log_in.Where(c => c.ContactInfoEmail == email).FirstOrDefault());
        }

        internal void Update(DBconnections dc)
        {
            PL.Log_in entry = dc.Log_in.Where(c => c.ContactInfoEmail == this.email).FirstOrDefault();
            entry.LogInPassword = hash;
        }
    }
}
