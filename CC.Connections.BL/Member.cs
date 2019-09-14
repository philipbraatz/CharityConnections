using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class Member
    {
        //public int ID { get; set; }
        public ContactInfo Contact { get; set; }
        public Role Role { get; set; }
        public Member_Type Member_Type { get; set; }
        public string Password { get; set; }

        // Used for User Login
        public Member(string email, string userpass)
        {
            Contact = new ContactInfo(email);
            Password = userpass;
        }

        private string GetHash()
        {
            using (var hash = new System.Security.Cryptography.SHA1Managed())
            {
                var hashbytes = System.Text.Encoding.UTF8.GetBytes(Password);
                return Convert.ToBase64String(hash.ComputeHash(hashbytes));
            }
        }

        public bool Login()
        {
            try
            {
                if (String.IsNullOrEmpty(Contact.Email))
                    throw new Exception("email must be set");//no userId
                else if (String.IsNullOrEmpty(Password))
                    throw new Exception("password must be set");//no UserPass
                else
                {
                    DBconnections dc = new DBconnections();
                    PL.Member entry = dc.Members.FirstOrDefault(u => u.ContactEmail == this.Contact.Email);

                    string temp = GetHash();

                    if (entry == null)
                        return false;
                    else if (entry.Password == temp)//success
                    {
                        //Contact = new ContactInfo( entry.ContactID);//Loaded on email check, unsecure
                        Role = new Role(entry.Role_ID);
                        Member_Type = new Member_Type(entry.MemeberTypeID);
                        //Id = entry.Member_ID;
                        return true;
                    }
                    else
                        return false;//wrong password
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}
