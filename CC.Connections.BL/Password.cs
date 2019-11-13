using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Connections.BL
{
    public enum MemberType
    {
        GUEST = 0,
        VOLLUNTEER,
        CHARITY
    }

    //Only to be used inside classes that need passwords
    //you cannot straight up create a new password without sending it to a classes password
    public class Password
    {
        [DisplayName("Email")]
        public string email { get; set; }
        private string hash;
        [DisplayName("Password")]
        public string Pass
        {
            //returns hashed password
            get { return hash; }
            //set with normal password
            set
            {
                using (var sha1 = new System.Security.Cryptography.SHA1Managed())
                {
                    if(value == null)
                    { 
                        var hashbytes = System.Text.Encoding.UTF8.GetBytes(value);
                        hash = Convert.ToBase64String(sha1.ComputeHash(hashbytes));
                    }
                }
            }
        }

        [DisplayName("Account Type")]
        public new MemberType MemberType { get; set; }//TODO inherit AbsColumn
        //{
            //get { return (MemberType)base.getProperty("MemberType"); }
            //set { setProperty("MemberType", value); }
        //}

        public Password() { }
        //existing Password
        //
        //if Password is not found with matching (email)
        //a new one will be create with (Pass) = "default"
        public Password(string email, bool load = true)
        {
            this.email = email;
            if (load)
                if (!loadId())//insert new Password with email
                {
                    GenerateDefault(email);//generate
                                           //insert
                    using (fvtcEntities1 dc = new fvtcEntities1())
                        this.Insert(dc.Log_in.Max(c =>
                            c.LogInMember_ID).GetValueOrDefault());
                }
        }

        private void GenerateDefault(string email, string password = "default")
        {
            this.email = email;
            this.Pass = password;
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

        private bool loadId()
        {
            try
            {
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Log_in entry = dc.Log_in.FirstOrDefault(c => c.ContactInfoEmail == this.email);
                    if (entry == null)
                        return false;//throw new Exception("Log_in "+ this.email + " does not exist");

                    this.hash = entry.LogInPassword;
                    return true;
                }
            }
            catch (Exception e)
            { throw e; }
        }


        internal bool Insert(int iD)
        {
            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                if (dc.Log_in.ToList().Count > 0)
                    iD = (int)dc.Log_in.Max(c => c.LogInMember_ID) + 1;
                else
                    iD = 0;

                if (dc.Log_in.Where(c => c.ContactInfoEmail == email).FirstOrDefault() != null)
                    return false;//already exists

                PL.Log_in entry = new PL.Log_in
                {
                    ContactInfoEmail = email,
                    LogInMember_ID = iD,
                    LogInPassword = hash
                };
                dc.Log_in.Add(entry);
                dc.SaveChanges();
                return true;//added
            }
        }

        internal void Delete()
        {
            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                dc.Log_in.Remove(dc.Log_in.Where(c => c.ContactInfoEmail == email).FirstOrDefault());
                this.email = string.Empty;
                this.hash = string.Empty;
                dc.SaveChanges();
            }
        }

        internal void Update()
        {
            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                PL.Log_in entry = dc.Log_in.Where(c => c.ContactInfoEmail == this.email).FirstOrDefault();
                entry.LogInPassword = hash;
                dc.SaveChanges();
            }
        }

        public bool Login()
        {
            try
            {
                if (String.IsNullOrEmpty(email))
                    throw new Exception("Email must be set");//no userId
                else if (String.IsNullOrEmpty(hash))
                    throw new Exception("Password must be set");//no UserPass
                else
                {
                    using (fvtcEntities1 dc = new fvtcEntities1())
                    {
                        PL.Log_in entry = dc.Log_in.FirstOrDefault(u => u.ContactInfoEmail == this.email);
                        if (entry == null)
                            return false;
                        else
                            return entry.LogInPassword == hash;//success if match
                    }
                }
            }
            catch (Exception e) { throw e; }
        }
    }


    public class PasswordList
        : List<Password>
    {
        public void LoadList()
        {
            try
            {
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    if (dc.Log_in.ToList().Count != 0)
                        dc.Log_in.ToList().ForEach(c =>
                            this.Add(new Password(c.ContactInfoEmail, c.LogInPassword, true)));
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
