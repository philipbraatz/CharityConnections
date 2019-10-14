using System;
using System.Linq;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class AbsPreference : ColumnEntry<PL.Preference>
    {
        //id
        public new int ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }
        public new decimal Distance {
            get { return (decimal)base.getProperty("Distance"); }
            set { setProperty("Distance", value); }
        }

        public AbsPreference() :
            base(new PL.Preference()) { }
        public AbsPreference(PL.Preference entry) :
            base(entry){ }
        public AbsPreference(int id) :
            base(new DBconnections().Preferences, id)
        {
            Clear();
            ID = id;
            LoadId();
        }
        public static implicit operator AbsPreference(PL.Preference entry)
        { return new AbsPreference(entry); }

        public void LoadId(){
            using (DBconnections dc = new DBconnections()){
                base.LoadId(dc.Preferences);
            }
        }
        public int Insert() {
            using (DBconnections dc = new DBconnections()) {
                return base.Insert(dc, dc.Preferences);
            }
        }
        public int Update()
        {
            using (DBconnections dc = new DBconnections())
            {
                return base.Update(dc, dc.Preferences);
            }
        }
        public int Delete()
        {
            using (DBconnections dc = new DBconnections())
            {
                //dc.Preferences.Remove(this);
                //return dc.SaveChanges();
                return base.Delete(dc, dc.Preferences);
            }
        }
    }

    public class AbsPreferenceList : AbsList<AbsPreference, Preference>
    {
        public new void LoadAll()
        {
            using (DBconnections dc = new DBconnections())
            {
                base.LoadAll(dc.Preferences);
            }
        }
    }
}