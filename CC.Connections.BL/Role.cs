using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class Role
    {
        public int ID { get; set; }
        public string Description { get; set; }

        public Role()
        {}
        public Role(int? role_ID)
        {
            this.ID = (int)role_ID;
        }

        public int Insert()
        {
            try
            {
                if (Description == string.Empty)
                    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    ID = dc.Roles.Max(c => c.Role_ID) + 1;
                    PL.Role Genre = new PL.Role
                    {
                        Role_ID = ID,
                        Role_desc = this.Description
                    };

                    dc.Roles.Add(Genre);
                    return dc.SaveChanges();
                }
            }
            catch (Exception e){throw e;}
        }
        public int Delete()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    dc.Roles.Remove(dc.Roles.Where(c => c.Role_ID == ID).FirstOrDefault());
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public int Update()
        {
            try
            {
                if (Description == string.Empty)
                    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Role entry = dc.Roles.Where(c => c.Role_ID == this.ID).FirstOrDefault();
                    entry.Role_desc = Description;

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public void LoadId()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Role entry = dc.Roles.FirstOrDefault(c => c.Role_ID == this.ID);
                    if (entry == null)
                        throw new Exception("Genre does not exist");

                    Description = entry.Role_desc;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class RoleList
        : List<Role>
    {
        public void Load()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    dc.Roles.ToList().ForEach(c => this.Add(new Role
                    {
                        ID = c.Role_ID,
                        Description = c.Role_desc
                    }));
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}