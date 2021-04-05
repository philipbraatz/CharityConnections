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
    public class Charity : BaseModel<PL.Charity>
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

        private Uri uri;
        [DisplayName("Charity Web Site")]
        public Uri URL//TODO check if valid Uri implentation
        {
            get {
                String stest = (string)base.getProperty(nameof(URL));
                Uri.TryCreate(stest,UriKind.RelativeOrAbsolute,out uri); 
                return uri; 
            }
            set { 
                setProperty(nameof(URL), value);
                uri = value;
            }
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
                    category = new Category((Guid)(base.getProperty(nameof(Category)+"ID"))) ;
                return category;
            }
            set { 
                category =value;
                if (value != null)
                    base.setProperty(nameof(Category) + "ID", value.ID);
                } 
        }
        public Location Location
        {
            get
            {
                var prop = base.getProperty(nameof(Location) + "ID");
                if (loc == null && prop != null)
                    loc = new Location((Guid)prop);
                return loc;
            }
            set
            {
                loc = value;
                if(value != null)
                 base.setProperty(nameof(Location) + "ID", value.ID);
            }
        }

        internal static bool Exists(CCEntities dc, string charity)
            => Exists(dc.Charities,new Charity { ID = charity});


        public Charity() => this.Deductibility = false;
        
        //does not verify password
        public Charity(Password p) : this(p.email,true)
        {
            if (p is null)
                throw new ArgumentNullException(nameof(p));

            this.Deductibility = false;
        }
        public Charity(PL.Charity entry) : base(entry)
        {
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            this.Category = new Category((Guid)entry.CategoryID);
            this.Location = new Location((Guid)entry.LocationID);
        }
        public Charity(string charity,bool preloaded =true) : 
            base(new CCEntities().Charities, charity, preloaded)
        {
            this.Email = charity;
            if (preloaded)
                LoadId(true);
            else
            {
                using (CCEntities dc = new CCEntities())
                {
                    //TODO refactor
                    PL.Charity charityPL = dc.Charities.Where(c => c.CharityEmail == charity).FirstOrDefault();
                    this.Category = new Category((Guid)charityPL.CategoryID);
                    this.Location = new Location((Guid)charityPL.LocationID);
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

            this.Email = charityInfo.CharityEmail;
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
                    PL.Charity charityPL = dc.Charities.Where(c => c.CharityEmail == charityEmail).FirstOrDefault();
                    if (charityPL != null)
                    {
                        setCharityInfo(charityPL);
                        this.Category = new Category((Guid)charityPL.CategoryID);
                        this.Location = new Location((Guid)charityPL.LocationID);
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
                    throw;
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
                Charity loadC = CharityCollection.INSTANCE.Where(c => c.Email == this.Email).FirstOrDefault();
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
                        
                        PL.Charity entry = dc.Charities.Where(c => c.CharityEmail == this.Email).FirstOrDefault();
                        this.Category = new Category(entry.CategoryID.Value);
                        this.Location = new Location(entry.LocationID.Value);
                    }
                }
                catch (Exception)
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
                this.setProperty("LocationID", this.Location.ID);
                base.Insert(dc, dc.Charities);
                password.Insert();
                CharityCollection.AddToInstance(this);
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
                CharityCollection.AddToInstance(this);
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
                CharityCollection.RemoveInstance(this);
            }
        }
    }

    public class CharityCollection : BaseList<Charity,PL.Charity>
    {
        //START instance
        private static CharityCollection INS = new CharityCollection();
        public static CharityCollection INSTANCE
        {
            get
            {
                if (INS == null || INS.Count == 0)
                    INS = LoadInstance();
                return INS;
            }
            private set => INS = value;
        }

        public static CharityCollection LoadInstance()
        {
            try
            {
                INSTANCE = new CharityCollection();
                using (CCEntities dc = new CCEntities())
                {
                    foreach (var c in dc.Charities.ToList())
                        INS.Add(new Charity(c));
                }
                return INS;
            }
            catch (System.Data.SqlClient.SqlException)
            {
                //if(ex.)
                throw;
            }
            catch (EntityException e) { throw e.InnerException; }
        }
        public static void AddToInstance(Charity charity)
            => INS.Add(charity);

        //Might be able to optimize better
        internal static void UpdateInstance(Charity charity)
        {
            RemoveInstance(charity);
            AddToInstance(charity);
        }

        internal static void RemoveInstance(Charity charity)
            => INSTANCE.Remove(charity);
        //END instance

        private const string Event_LOAD_ERROR = "Events not loaded, please loadEvents with a Charity ID";
        private const string AddPreference_ERROR = "Currently being used as a preference list. Please use AddPreference instead";

        private Volunteer userPref { get; set; }
        private int? SortID { get; set; }

        public void setPreferences(Volunteer userPref)
        {
            this.userPref = userPref ?? throw new ArgumentNullException(nameof(userPref));
        }

        public void LoadAll()
        {
            this.Clear();
            LoadInstance();//make sure Instance is filled
            this.AddRange(INS);
        }
        public void LoadWithFilter(object id, SortBy sort)
        {
            //throw new NotImplementedException();
            LoadAll();
            Filter(id, sort);

        }
        public void Filter(object id, SortBy sort)
        {
            Filterer filter = new Filterer();
            filter.FillFilter(this);
            switch (sort)
            {
                case SortBy.CATEGORY:
                    filter.Whitelist_Remaining(new List<Guid> { (Guid)id }, new List<Guid>());
                    this.Clear();

                    foreach (var item in filter.GetRemainingCharities())
                        this.Add(item);
                    break;
                case SortBy.HelpingAction:
                    throw new NotImplementedException();
                case SortBy.CHARITY:
                    throw new NotImplementedException();
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
                this.Add(item);
        }

        public new void Clear()
        {
            base.Clear();
            SortID = null;
        }

        private void Add(Charity item)
        {
            base.Add(item);
        }
        private void Remove(Charity item)
        {
            base.Remove(item);
        }
        public new void Add(Charity item)
        {
            if (SortID != null)
                throw new Exception(AddPreference_ERROR);
            base.Add(item);
        }
        public new void Remove(Charity item)
        {
            if (SortID != null)
                throw new Exception("Currently being used as a preference list. Please use DeletePreference instead");
            base.Remove(item);
        }

        public static int getCount() => new CCEntities().Charities.Count();

    }
}