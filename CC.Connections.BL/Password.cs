using Doorfail.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorfail.Connections.BL
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
        [MaxLength(32), MinLength(8)]
        public string Pass
        {
            //returns hashed password
            get { return hash; }
            //set with normal password
            set
            {
                using (var sha1 = new System.Security.Cryptography.SHA1Managed())
                {
                    if(value != null)
                    { 
                        var hashbytes = System.Text.Encoding.UTF8.GetBytes(value);
                        hash = Convert.ToBase64String(sha1.ComputeHash(hashbytes));
                    }
                }
            }
        }

        [DisplayName("Account Type")]
        public MemberType MemberType { get; set; }

        public Password() { }
        //existing Password
        //
        //if Password is not found with matching (email)
        //a new one will be create with (Pass) = "default" and MemberType =Guest
        //TODO Depereciate, unsafe
        public Password(string email, bool load = true)
        {
            this.email = email;
            if (load)
                if (!loadId())//insert new Password with email
                {
                    ///TODO Might break signup, verify this is fine
                    //try
                    //{
                    //    GenerateDefault(email);//generate
                    //                           //insert
                    //    using (CCEntities dc = new CCEntities())
                    //        this.Insert();
                    //}
                    //catch (Exception)
                    {
                        throw new Exception("Login credentials with email \""+email+"\" do not exist"); 
                    }
                }
        }

        private void GenerateDefault(string email, string password = "default")
        {
            this.email = email;
            this.Pass = password;
            this.MemberType = MemberType.GUEST;
        }

        //new
        public Password(string email, string password,MemberType memberType, bool hashed = false)
        {
            this.email = email;
            if (hashed)
                this.hash = password;
            else
                this.Pass = password;
            this.MemberType = memberType;
            Insert();
        }

        private bool loadId()
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");

                    PL.LogIn entry = dc.LogIns.FirstOrDefault(c => c.MemberEmail == this.email);
                    if (entry == null)
                        return false;//throw new Exception("LogIns "+ this.email + " does not exist");

                    this.hash = entry.Password;

                    //TODO remove null option in the future
                    if (entry.MemberType != null)
                        this.MemberType = (MemberType)entry.MemberType;
                    else
                        this.MemberType = MemberType.VOLLUNTEER;
                    return true;
                }
            }
            catch (Exception e)
            { throw e; }
        }


        internal bool Insert()
        {
            string iD;
            using (CCEntities dc = new CCEntities())
            {
                iD = this.email;

                if (dc.LogIns.Where(c => c.MemberEmail == email).FirstOrDefault() != null)
                    return false;//already exists

                PL.LogIn entry = new PL.LogIn
                {
                    MemberEmail = email,
                    Password = hash,
                    MemberType =(int)this.MemberType
                };
                dc.LogIns.Add(entry);
                dc.SaveChanges();
                return true;//added
            }
        }

        internal int Delete()
        {
            using (CCEntities dc = new CCEntities())
            {
                dc.LogIns.Remove(dc.LogIns.Where(c => c.MemberEmail == email).FirstOrDefault());
                this.email = string.Empty;
                this.hash = string.Empty;
                return dc.SaveChanges();
            }
        }

        internal int Update()
        {
            using (CCEntities dc = new CCEntities())
            {
                PL.LogIn entry = dc.LogIns.Where(c => c.MemberEmail == this.email).FirstOrDefault();
                entry.Password = hash;
                entry.MemberType = (int)this.MemberType;
                return dc.SaveChanges();
            }
        }

        //guest if password didnt match
        //otherwise sets volunteer or charity
        public void Login()
        {
            try
            {
                if (String.IsNullOrEmpty(email))
                    throw new Exception("Email must be set");//no userId
                else if (String.IsNullOrEmpty(hash))
                    throw new Exception("Password must be set");//no UserPass
                else
                {
                    using (CCEntities dc = new CCEntities())
                    {
                        PL.LogIn entry = dc.LogIns.FirstOrDefault(u => u.MemberEmail == this.email);
                        if (entry == null)
                            this.MemberType = MemberType.GUEST;//doesnt exist
                        else if (entry.Password == hash)//success if match
                            this.MemberType = (MemberType)entry.MemberType;
                        else
                            this.MemberType = MemberType.GUEST;//failed
                    }
                }
            }
            catch (Exception) { throw; }
        }
    }


    public class PasswordCollection
        : List<Password>
    {
        public void LoadList()
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    if (dc.LogIns.ToList().Count != 0)
                        dc.LogIns.ToList().ForEach(c =>
                            this.Add(new Password(c.MemberEmail, c.Password,(MemberType)c.MemberType, true)));
                }
            }
            catch (Exception) { throw; }
        }
    }
}
