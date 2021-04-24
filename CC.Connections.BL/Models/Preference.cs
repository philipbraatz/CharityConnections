using System;
using System.ComponentModel;
using System.Linq;
using Doorfail.DataConnection;
using Doorfail.Connections.PL;

namespace Doorfail.Connections.BL
{
    public class Preference : CrudModel_Json<PL.Preference>
    {
        //private static CCEntities dc;

        //id
        public new Guid ID
        {
            get { return (Guid)base.ID; }
            set { base.ID = value; }
        }

        [DisplayName("Max search Distance")]
        public decimal Distance {
            get {
                try
                {
                    return (decimal)(base.getProperty(nameof(Distance)) ?? 0);
                }
                catch
                {
                    return 0;//probably failed because value is null
                }
            }
            set { setProperty(nameof(Distance), value); }
        }

        public Preference() :
            base(new PL.Preference()) { }
        public Preference(PL.Preference entry) :
            base(entry){ }
        public Preference(Guid id) :
            base(JsonDatabase.GetTable<PL.Preference>(), id)
        {
            Clear();
            ID = id;
            LoadId();
        }
        public static implicit operator Preference(PL.Preference entry)
        { return new Preference(entry); }

        public void LoadId(){
            using (CCEntities dc = new CCEntities()){
                base.LoadId(JsonDatabase.GetTable<PL.Preference>());
            }
        }
        public int Insert() {
            using (CCEntities dc = new CCEntities()) {
                return base.Insert(dc, dc.Preferences);
            }
        }
        public int Update()
        {
            using (CCEntities dc = new CCEntities())
            {
                return base.Update(dc, dc.Preferences);
            }
        }
        public int Delete()
        {
            using (CCEntities dc = new CCEntities())
            {
                //dc.Preferences.Remove(this);
                //return dc.SaveChanges();
                return base.Delete(dc, dc.Preferences);
            }
        }
    }

    public class AbsPreferenceCollection : CrudModelList<Preference, PL.Preference>
    {
        public void LoadAll()
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.LoadAll(JsonDatabase.GetTable<PL.Preference>());
                }
            else
                base.LoadAll(JsonDatabase.GetTable<PL.Preference>());
        }
    }
}