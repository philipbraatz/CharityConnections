using System;
using System.Linq;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class AbsPreference : ColumnEntry<PL.Preference>
    {
        private static fvtcEntities1 dc;

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
            base(new fvtcEntities1().Preferences, id)
        {
            Clear();
            ID = id;
            LoadId();
        }
        public static implicit operator AbsPreference(PL.Preference entry)
        { return new AbsPreference(entry); }

        public void LoadId(){
            using (fvtcEntities1 dc = new fvtcEntities1()){
                base.LoadId(dc.Preferences);
            }
        }
        public int Insert() {
            using (fvtcEntities1 dc = new fvtcEntities1()) {
                return base.Insert(dc, dc.Preferences);
            }
        }
        public int Update()
        {
            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                return base.Update(dc, dc.Preferences);
            }
        }
        public int Delete()
        {
            using (fvtcEntities1 dc = new fvtcEntities1())
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
            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                base.LoadAll(dc.Preferences);
            }
        }
    }
}