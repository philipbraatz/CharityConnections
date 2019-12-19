using System;
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
    public class Charity : ColumnEntry<PL.Charities>
    {
        //id
        public new int ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }

        [DisplayName("Charity Email")]
        public string Charity_Email
        {
            get { return (string)base.getProperty("Charity_Email"); }
            set { setProperty("Charity_Email", value); }
        }

        [DisplayName("Charity Name")]
        public string Charity_Name
        {
            get { return (string)base.getProperty("Charity_Name"); }
            set { setProperty("Charity_Name", value); }
        }

        [DisplayName("Federal Tax ID# (EIN)")]
        public string Charity_EIN
        {
            get { return (string)base.getProperty("Charity_EIN"); }
            set { setProperty("Charity_EIN", value); }
        }

        [DisplayName("Is Deductible")]
        public bool Charity_Deductibility
        {
            get { return (bool)base.getProperty("Charity_Deductibility"); }//{ return false; }//
            set { setProperty("Charity_Deductibility", value); }
        }

        [DisplayName("Charity Web Site")]
        public string Charity_URL
        {
            get { return (string)base.getProperty("Charity_URL"); }
            set { setProperty("Charity_URL", value); }
        }

        [DisplayName("Charities Cause")]
        public string Charity_Cause
        {
            get { return (string)base.getProperty("Charity_Cause"); }
            set { setProperty("Charity_Cause", value); }
        }
        [DisplayName("Charities Requirements")]
        public string Charity_Requirements
        {
            get { return (string)base.getProperty("Charity_Requirements"); }
            set { setProperty("Charity_Requirements", value); }
        }

        private Category category;
        private Location loc;
        public Category Category 
        {   get {
                if (category == null)
                    category = new Category((int)base.getProperty("Charity_Category_ID"));
                return category;
            }
            set { 
                category =value;
                if (value != null)
                    base.setProperty("Charity_Category_ID",value.ID);
                } 
        }
        public Location Location
        {
            get
            {
                var prop = base.getProperty("Location_ID");
                if (loc == null && prop != null)
                    loc = new Location((int)prop);
                return loc;
            }
            set
            {
                loc = value;
                if(value != null)
                 base.setProperty("Location_ID", value.ID);
            }
        }

        internal static bool Exists(CCEntities dc, int id)
        {
            return dc.Charities.Where(c => c.Charity_ID == id).FirstOrDefault() != null;
        }

        public Charity() {
            this.Charity_Deductibility = false;
        }
        //does not verify password
        public Charity(Password p) : this(p.email)
        {
            this.Charity_Deductibility = false;
        }
        public Charity(PL.Charities entry) : base(entry)
        {
            this.Category = new Category((int)entry.Charity_Category_ID);
            this.Location = new Location((int)entry.Location_ID);

            this.Charity_Deductibility = false;
        }
        public Charity(int id,bool preloaded =true) : base(new CCEntities().Charities, id, preloaded)
        {
            if (preloaded)
                LoadId(true);
            else
            {
                this.Category = new Category(((int)base.getProperty("Charity_Category_ID")));
                this.Location = new Location(((int)base.getProperty("Location_ID")));
            }
        }

        public void setCharity(Charity charityInfo)
        {
            if (charityInfo == null)
                throw new ArgumentNullException(nameof(charityInfo));

            this.Charity_Name = charityInfo.Charity_Name ?? "";
            this.Charity_Email = charityInfo.Charity_Email ?? "";
            this.Charity_Cause = charityInfo.Charity_Cause ?? "";
            this.Charity_URL = charityInfo.Charity_URL ?? "";
            this.Charity_Deductibility = charityInfo.Charity_Deductibility;
            this.Charity_EIN = charityInfo.Charity_EIN;
            this.Charity_Requirements = charityInfo.Charity_Requirements;
            this.Location = charityInfo.Location;
        }
        public void setCharityInfo(PL.Charities charityInfo)
        {
            if (charityInfo == null)
                throw new ArgumentNullException(nameof(charityInfo));

            this.ID = charityInfo.Charity_ID;
            this.Charity_Name = charityInfo.Charity_Name ?? "";
            this.Charity_Email = charityInfo.Charity_Email ?? "";
            this.Charity_Cause = charityInfo.Charity_Cause ?? "";
            this.Charity_URL = charityInfo.Charity_URL ?? "";
            this.Charity_Deductibility = (bool)charityInfo.Charity_Deductibility;
            this.Charity_EIN = charityInfo.Charity_EIN;
            this.Charity_Requirements = charityInfo.Charity_Requirements;
        }

        public Charity(string charityEmail, string password = "", bool hashed = false, bool debug = false)
        {
            this.Charity_Deductibility = false;
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    Clear();

                    //set new id for preferred lists
                    if (dc.Charities.ToList().Count > 0)
                        ID = dc.Charities.Max(c => c.Charity_ID) + 1;
                    else
                        ID = 0;//first entry


                    //TODO refactor
                    //try to load existing ID
                    PL.Charities charityPL = dc.Charities.Where(c => c.Charity_Email == charityEmail).FirstOrDefault();
                    if (charityPL != null)
                    {
                        setCharityInfo(charityPL);
                        this.Category = new Category((int)charityPL.Charity_Category_ID);
                        this.Location = new Location((int)charityPL.Location_ID);
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
        public static implicit operator Charity(PL.Charities entry)
        { return new Charity(entry); }

        public void LoadId(bool preloaded = true)
        {
            if (preloaded)
            {
                Charity loadC = CharityList.INSTANCE.Where(c => c.ID == this.ID).FirstOrDefault();
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
                        
                        PL.Charities entry = dc.Charities.Where(c => c.Charity_ID == this.ID).FirstOrDefault();
                        this.Category = new Category(entry.Charity_Category_ID.Value);
                        this.Location = new Location(entry.Location_ID.Value);
                    }
                }
                catch (Exception e)
                { throw; }
        }
        public void LoadId(int id,bool preloaded =true)
        {
            ID = id;
            LoadId(preloaded);
        }
        public int Insert(Password password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            using (CCEntities dc = new CCEntities())
            {
                this.Location.Insert();
                this.setProperty("Location_ID", this.Location.ID);
                return base.Insert(dc, dc.Charities) +
                    (password.Insert() ? 1 : 0);
            }
        }
        public int Update(Password password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            using (CCEntities dc = new CCEntities())
            {
                return base.Update(dc, dc.Charities) +
                    this.Location.Update() +
                    password.Update();
            }
        }
        public int Delete(Password password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            using (CCEntities dc = new CCEntities())
            {
                //dc.Categories.Remove(this);
                //return dc.SaveChanges();
                return base.Delete(dc, dc.Charities) +
                    this.Location.Delete() +
                    password.Delete();
            }
        }
    }

    public class CharityList :
        List<Charity>
    {
        public static CharityList INSTANCE { get; } = LoadInstance();

        public static CharityList LoadInstance()
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    foreach (var c in dc.Charities.ToList())
                        INSTANCE.Add(new Charity(c));
                }
                return INSTANCE;
            }
            catch (EntityException e) { throw e.InnerException; }
        }
        public static void AddToInstance(Charity charity)
        {
            INSTANCE.Add(charity);
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
            this.AddRange(INSTANCE);
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

        public void LoadWithFilter(int id, SortBy sort)
        {
            //throw new NotImplementedException();
            this.LoadAll();
            Filter(id, sort);

        }
        public void Filter(int id, SortBy sort)
        {
            Filterer filter = new Filterer();
            filter.FillFilter(this);
            switch (sort)
            {
                case SortBy.CATEGORY:
                    filter.Whitelist_Remaining(new List<int> { id },new List<int>());
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