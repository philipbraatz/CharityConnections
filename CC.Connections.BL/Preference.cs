using System;
using System.Linq;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    //NOTE PB:
    //.5 hours
    public class Preference
    {
        public int ID;
        public double distance;

        public Preference(int preference_ID)
        {
            this.ID = preference_ID;
        }

        public Preference(){}

        public int Insert()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Preferences.ToList().Count > 0)
                        ID = dc.Preferences.Max(c => c.Preference_Id) + 1;//unique id
                    else
                        ID = 0;

                    PL.Preference entry = new PL.Preference
                    {
                        Preference_Id =ID,
                        Distance = this.distance
                    };

                    dc.Preferences.Add(entry);
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public int Delete()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    dc.Preferences.Remove(dc.Preferences.Where(c => c.Preference_Id == ID).FirstOrDefault());
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public int Update()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invaild");

                    PL.Preference entry = dc.Preferences.Where(c => c.Preference_Id == this.ID).FirstOrDefault();
                    entry.Distance = distance;

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

                    PL.Preference entry = dc.Preferences.FirstOrDefault(c => c.Preference_Id == this.ID);
                    if (entry == null)
                        throw new Exception("Genre does not exist");

                    distance = (double)entry.Distance;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}