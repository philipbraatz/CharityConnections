﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using CC.Abstract;
using System.Data.Entity.Core;



// setting the charity class with a inherit from contactInfo

namespace CC.Connections.BL
{
    public class Charity : ColumnEntry<PL.Charity>
    {
        [DisplayName("Charity Email")]
        public string Email
        {
            get { return (string)base.ID; }
            set { base.ID = value; }
        }

        [DisplayName("Charity Name")]
        public string Name
        {
            get { return (string)base.getProperty(nameof(Name)); }
            set { setProperty(nameof(Name), value); }
        }

        [DisplayName("Federal Tax ID# (EIN)")]
        public string EIN
        {
            get { return (string)base.getProperty(nameof(EIN)); }
            set { setProperty(nameof(EIN), value); }
        }

        [DisplayName("Is Deductible")]
        public bool Deductibility
        {
            get { return (bool)base.getProperty(nameof(Deductibility)); }
            set { setProperty(nameof(Deductibility), value); }
        }

        [DisplayName("Charity Web Site")]
        public Uri URL//TODO check if valid Uri implentation
        {
            get { return new Uri ((string)base.getProperty(nameof(URL))); }
            set { setProperty(nameof(URL), value.AbsoluteUri); }
        }

        [DisplayName("Charities Cause")]
        public string Cause
        {
            get { return (string)base.getProperty(nameof(Cause)); }
            set { setProperty(nameof(Cause), value); }
        }

        private Category category;
        private Location loc;
        public Category Category 
        {   get {
                if (category == null)
                    category = new Category((Guid)(base.getProperty(nameof(Category)+"_ID"))) ;
                return category;
            }
            set { 
                category =value;
                if (value != null)
                    base.setProperty(nameof(Category) + "_ID", value.ID);
                } 
        }
        public Location Location
        {
            get
            {
                var prop = base.getProperty(nameof(Location) + "_ID");
                if (loc == null && prop != null)
                    loc = new Location((Guid)prop);
                return loc;
            }
            set
            {
                loc = value;
                if(value != null)
                 base.setProperty(nameof(Location) + "_ID", value.ID);
            }
        }

        internal static bool Exists(CCEntities dc, string charity)
        {
            return dc.Charities.Where(c => c.Charity_Email == charity).FirstOrDefault() != null;
        }

        public Charity() {
            this.Deductibility = false;
        }
        //does not verify password
        public Charity(Password p) : this(p.email,true)
        {
            this.Deductibility = false;
        }
        public Charity(PL.Charity entry) : base(entry)
        {
            this.Category = new Category((Guid)entry.Category_ID);
            this.Location = new Location((Guid)entry.Location_ID);

            //this.Charity_Deductibility = false;
        }
        public Charity(string charity,bool preloaded =true) : base(new CCEntities().Charities, charity, preloaded)
        {
            this.Email = charity;
            if (preloaded)
                LoadId(true);
            else
            {
                using (CCEntities dc = new CCEntities())
                {
                    //TODO refactor
                    PL.Charity charityPL = dc.Charities.Where(c => c.Charity_Email == charity).FirstOrDefault();
                    this.Category = new Category((Guid)charityPL.Category_ID);
                    this.Location = new Location((Guid)charityPL.Location_ID);
                }
            }
        }

        public void setCharity(Charity charityInfo)
        {
            if (charityInfo == null)
                throw new ArgumentNullException(nameof(charityInfo));

            this.Name = charityInfo.Name ?? "";
            this.Email = charityInfo.Email ?? "";
            this.Cause = charityInfo.Cause ?? "";
            this.URL = charityInfo.URL;//TODO handle invalid URLs
            this.Deductibility = charityInfo.Deductibility;
            this.EIN = charityInfo.EIN;
            this.Location = charityInfo.Location;
            this.Category = charityInfo.Category;
        }
        public void setCharityInfo(PL.Charity charityInfo)
        {
            if (charityInfo == null)
                throw new ArgumentNullException(nameof(charityInfo));

            this.Email = charityInfo.Charity_Email;
            this.Name = charityInfo.Name ?? "";
            this.Cause = charityInfo.Cause ?? "";
            this.URL = new Uri(charityInfo.URL);//TODO handle invalid URLs
            this.Deductibility = (bool)charityInfo.Deductibility;
            this.EIN = charityInfo.EIN;
        }

        public Charity(string charityEmail, string password = "", bool hashed = false, bool debug = false)
        {
            this.Deductibility = false;
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    Clear();

                    Email = charityEmail;


                    //TODO refactor
                    //try to load existing ID
                    PL.Charity charityPL = dc.Charities.Where(c => c.Charity_Email == charityEmail).FirstOrDefault();
                    if (charityPL != null)
                    {
                        setCharityInfo(charityPL);
                        this.Category = new Category((Guid)charityPL.Category_ID);
                        this.Location = new Location((Guid)charityPL.Location_ID);
                    }
                    else
                    {
                        Clear();//new Charity
                        Password newPassword = new Password(charityEmail, password, MemberType.CHARITY, hashed);
                        newPassword.Insert();
                        this.Location.Insert();
                    }

                    
                }
            }
            catch (Exception e)
            {
                if (e.Message != "The underlying provider failed on Open.")
                    throw e;
                else
                    throw e.InnerException;//database error

            }
        }


        //turns a PL class into a BL equivelent example: 
        //Category pl = new Category();
        //...
        //AbsCategory bl = (AbsCategory)pl
        public static implicit operator Charity(PL.Charity entry)
        { return new Charity(entry); }

        public void LoadId(bool preloaded = true)
        {
            if (preloaded)
            {
                Charity loadC = CharityList.INSTANCE.Where(c => c.Email == this.Email).FirstOrDefault();
                if (loadC != null)//retreive from existing
                    this.setCharity(loadC);
                else
                    LoadId(false);
            }
            else
                try
                {
                    using (CCEntities dc = new CCEntities())
                    {
                        base.LoadId(dc.Charities);
                        
                        PL.Charity entry = dc.Charities.Where(c => c.Charity_Email == this.Email).FirstOrDefault();
                        this.Category = new Category(entry.Category_ID.Value);
                        this.Location = new Location(entry.Location_ID.Value);
                    }
                }
                catch (Exception e)
                { throw; }
        }
        public void LoadId(string email,bool preloaded =true)
        {
            Email = email;
            LoadId(preloaded);
        }
        public void Insert(Password password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            using (CCEntities dc = new CCEntities())
            {
                this.Location.Insert();
                this.setProperty("Location_ID", this.Location.ID);
                base.Insert(dc, dc.Charities);
                password.Insert();
                CharityList.AddToInstance(this);
            }
        }
        public void Update(Password password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            using (CCEntities dc = new CCEntities())
            {
                base.Update(dc, dc.Charities);
                this.Location.Update();
                password.Update();
                CharityList.AddToInstance(this);
            }
        }
        public void Delete(Password password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            using (CCEntities dc = new CCEntities())
            {
                //dc.Categories.Remove(this);
                //return dc.SaveChanges();
                base.Delete(dc, dc.Charities);
                this.Location.Delete();
                password.Delete();
                CharityList.RemoveInstance(this);
            }
        }
    }

    public class CharityList :
        List<Charity>
    {
        private static CharityList INS = new CharityList();
        public static CharityList INSTANCE 
        { get {
                if(INS == null || INS.Count ==0)
                    INS = LoadInstance();
                return INS;
            }
            private set => INS = value;
         }

        public static CharityList LoadInstance()
        {
            try
            {
                INSTANCE = new CharityList();
                using (CCEntities dc = new CCEntities())
                {
                    foreach (var c in dc.Charities.ToList())
                        INS.Add(new Charity(c));
                }
                return INS;
            }
            catch (EntityException e) { throw e.InnerException; }
        }
        public static void AddToInstance(Charity charity)
        {
            INS.Add(charity);
        }
        //Might be able to optimize better
        internal static void UpdateInstance(Charity charity)
        {
            RemoveInstance(charity);
            AddToInstance(charity);
        }

        internal static void RemoveInstance(Charity charity)
        {
            INSTANCE.Remove(charity);
        }

        //only used for Event lists
        private int? Sort_ID { get; set; }
        private SortBy sorter { get; set; }
        private Volunteer userPref { get; set; }

        private const string Event_LOAD_ERROR = "Events not loaded, please loadEvents with a Charity ID";

        public CharityList()
        {
            sorter = SortBy.NONE;
        }
        public CharityList(int id, SortBy sort, Volunteer user_pref = default)
        {
            Sort_ID = id;
            sorter = sort;
            userPref = user_pref;
            LoadAll();
        }

        public void setPreferences(Volunteer user_pref)
        {
            userPref = user_pref;
        }

        public void LoadAll()
        {
            sorter = SortBy.NONE;
            this.Clear();
            this.AddRange(INS);
            //try
            //{
            //    using (CCEntities dc = new CCEntities())
            //    {
            //        var test = dc.Charities.ToList();
            //        dc.Charities.ToList().ForEach(c => { if (c != null) this.Add(c, true); });
            //    }
            //}
            //catch (Exception e) { throw e; }
        }

        public void LoadWithFilter(object id, SortBy sort)
        {
            //throw new NotImplementedException();
            this.LoadAll();
            Filter(id, sort);

        }
        public void Filter(object id, SortBy sort)
        {
            Filterer filter = new Filterer();
            filter.FillFilter(this);
            switch (sort)
            {
                case SortBy.CATEGORY:
                    filter.Whitelist_Remaining(new List<Guid> { (Guid)id },new List<Guid>());
                    this.Clear();

                    foreach (var item in filter.GetRemainingCharities())
                        this.Add(item);
                    break;
                case SortBy.HELPING_ACTION:
                    throw new NotImplementedException();
                    break;
                case SortBy.CHARITY:
                    throw new NotImplementedException();
                    break;
                default:
                    throw new Exception("Cannot use id Filterer to Sort By " + sort.GetType().GetEnumName(sort));
            }
        }

        //Loads list using preferences filter
        public void LoadWithPreferences(Volunteer userpreferences)
        {
            userPref = userpreferences;
            Clear();
            Filterer filter = new Filterer();
            filter.CutCharitiesByPreferences(userPref);
            foreach (var item in filter.GetRemainingCharities())
                this.Add(item, true);
        }

        public new void Clear()
        {
            base.Clear();
            Sort_ID = null;
        }

        private void Add(Charity item, bool overrideMethod = true)
        {
                base.Add(item);
        }
        private void Remove(Charity item, bool overrideMethod = true)
        {
            base.Remove(item);
        }
        public new void Add(Charity item)
        {
            if (Sort_ID != null)
                throw new Exception("Currently being used as a preference list. Please use AddPreference instead");
            base.Add(item);
        }
        public new void Remove(Charity item)
        {
            if (Sort_ID != null)
                throw new Exception("Currently being used as a preference list. Please use DeletePreference instead");
            base.Remove(item);
        }

        //TODO depreciate
        public static int getCount()
        {
            using (CCEntities dc = new CCEntities())
            {
                return dc.Charities.Count();
            }
        }

        public static implicit operator List<object>(CharityList v)
        {
            throw new NotImplementedException();
        }
    }
}