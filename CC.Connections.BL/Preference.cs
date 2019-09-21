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

        public Preference(int Preference_ID)
        {
            this.ID = Preference_ID;
            LoadId();
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
                        ID = dc.Preferences.Max(c => c.Preference_ID) + 1;//unique id
                    else
                        ID = 0;

                    PL.Preference entry = new PL.Preference
                    {
                        Preference_ID =ID,
                        Distance = (decimal)distance
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

                    dc.Preferences.Remove(dc.Preferences.Where(c => c.Preference_ID == ID).FirstOrDefault());
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

                    PL.Preference entry = dc.Preferences.Where(c => c.Preference_ID == this.ID).FirstOrDefault();
                    entry.Distance = (decimal)distance;

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

                    PL.Preference entry = dc.Preferences.FirstOrDefault(c => c.Preference_ID == this.ID);
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