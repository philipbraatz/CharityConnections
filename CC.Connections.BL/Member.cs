using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Connections.BL
{
    public class Member
    {
        public int ID { get; set; }
        public ContactInfo Contact { get; set; }
        public Role Role { get; set; }
        public Member_Type Member_Type { get; set; }
        public string Password { get; set; }

        public bool Login()
        {
            return false;//TODO create login
        }
    }


}
