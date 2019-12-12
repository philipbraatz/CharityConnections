﻿using System;
using System.Linq;
using CC.Abstract;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class Preference : ColumnEntry<PL.Preference>
    {
        private static CCEntities dc;

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

        public Preference() :
            base(new PL.Preference()) { }
        public Preference(PL.Preference entry) :
            base(entry){ }
        public Preference(int id) :
            base(new CCEntities().Preferences, id)
        {
            Clear();
            ID = id;
            LoadId();
        }
        public static implicit operator Preference(PL.Preference entry)
        { return new Preference(entry); }

        public void LoadId(){
            using (CCEntities dc = new CCEntities()){
                base.LoadId(dc.Preferences);
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

    public class AbsPreferenceList : AbsList<Preference, PL.Preference>
    {
        public new void LoadAll()
        {
            using (CCEntities dc = new CCEntities())
            {
                base.LoadAll(dc.Preferences);
            }
        }
    }
}