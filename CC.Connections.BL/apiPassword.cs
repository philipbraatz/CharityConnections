using Doorfail.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorfail.Connections.BL
{
    public class apiPassword : Password
    {
        //TODO
        private static List<apiPassword> allPasswords = new List<apiPassword>();

        //other parameters from PL.Categories
        [DisplayName("API Key")]
        //must be the same name as the PL class
        public string Key { get; set; }

        public apiPassword(Password p)
        {
            if (p is null)
                throw new ArgumentNullException(nameof(p));
            SetPassword(p);
            getKey();
        }

        public apiPassword(string email, string password, MemberType memberType, bool hashed = false)
        {
            SetPassword(new Password(email, password, memberType, hashed));
            getKey();
        }

        private void getKey()
        {
            Login();
            if (MemberType != MemberType.GUEST)
            {
                using (CCEntities dc = new CCEntities())
                {
                    Key = dc.LogIns.Where(c => c.MemberEmail == email).FirstOrDefault().Key;
                }

                if (Key is null)
                    throw new Exception("API Key not set. To sign up please go to your profile under 'API Key'");
            }
        }

        public void SetPassword(Password p)
        {
            if (p is null)
                throw new ArgumentNullException(nameof(p));

            this.email = p.email;
            this.Pass = p.Pass;
            this.MemberType = p.MemberType;
        }
    }

}
