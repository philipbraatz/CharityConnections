using System;
using System.Linq;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class Preference : PL.Preference
    {
        public new double Distance { get; set; }

        public Preference(int ID)
        {
            this.Preference_ID = ID;
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
                        Preference_ID = dc.Preferences.Max(c => c.Preference_ID) + 1;//unique id
                    else
                        Preference_ID = 0;

                    dc.Preferences.Add(this);
                    dc.SaveChanges();
                    return Preference_ID;
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

                    dc.Preferences.Remove(dc.Preferences.Where(c => c.Preference_ID == Preference_ID).FirstOrDefault());
                    this.Distance = 0;
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

                    PL.Preference entry = dc.Preferences.Where(c => c.Preference_ID == this.Preference_ID).FirstOrDefault();
                    entry.Distance = (decimal)Distance;

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

                    PL.Preference entry = dc.Preferences.FirstOrDefault(c => c.Preference_ID == this.Preference_ID);
                    if (entry == null)
                        throw new Exception("Preference does not exist");

                    Distance = (double)entry.Distance;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}