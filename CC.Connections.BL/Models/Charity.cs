using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doorfail.Connections.PL;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Doorfail.DataConnection;
using System.Data.Entity.Core;
using System.Reflection;



// setting the charity class with a inherit from contactInfo

namespace Doorfail.Connections.BL
{
    public class Charity : CrudModel_Json<PL.Charity>
    {
        [DisplayName("Charity Email")]
        [MaxLength(64), MinLength(6)]
        [DataConnection.Base]
        public string Email
        {
            get { return (string)base.ID; }
            set { base.ID = value; }
        }

        [DisplayName("Charity Name")]
        [MaxLength(64), MinLength(3)]
        ////[Abstract.Base]
        public string Name
        {
            get { //DEBUG
                MethodBase mb = MethodBase.GetCurrentMethod();
                //string ret = mb.NamedArguments.FirstOrDefault().ToString();
                return (string)base.getProperty(nameof(Name)); }
            set { setProperty(nameof(Name), value); }
        }

        [DisplayName("Federal Tax ID# (EIN)")]
        [MaxLength(32), MinLength(8)]
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
        [MaxLength(64), MinLength(8)]
        public string Cause
        {
            get { return (string)base.getProperty(nameof(Cause)); }
            set { setProperty(nameof(Cause), value); }
        }

        private Category category;
        private Location loc;
        public Category Category 
        {   get {
                return category = category ?? new Category(base.getProperty(nameof(Category)+"ID") != null ?
                        (Guid)(base.getProperty(nameof(Category) + "ID")) : 
                        Guid.Empty);
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
                if (loc == null)
                    loc = new Location(base.getProperty(nameof(Location)+"ID") != null ?
                        (Guid)base.getProperty(nameof(Location)+"ID") :
                        Guid.Empty);
                return loc;
            }
            set
            {
                loc = value;
                if(value != null)
                 base.setProperty(nameof(Location) + "ID", value.ID);
            }
        }


        public Charity() => this.Deductibility = false;
        
        //does not verify password
        internal Charity(Password p) : this(p.email,true)
        {
            if (p is null)
                throw new ArgumentNullException(nameof(p));

            this.Deductibility = false;
        }
        public Charity(PL.Charity entry) : base(entry)
        {
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));

            if (entry.CategoryID != null)
                this.Category = new Category((Guid)entry.CategoryID);
            //else //Don't be bad data
                //throw new NullReferenceException("Category cannot be null");
            if(entry.LocationID != null)
                this.Location = new Location((Guid)entry.LocationID);
        }
        public Charity(string email,bool preloaded =true) : 
            base(JsonDatabase.GetTable<PL.Charity>(), email, preloaded)
        {
            this.Email = email;
            if (preloaded)
                LoadId(true);
            else
            {
                PL.Charity charityPL = null;
                if (false)
                using (CCEntities dc = new CCEntities())
                {
                    charityPL = dc.Charities.Where(c => c.CharityEmail == email).FirstOrDefault();
                }
                else
                    charityPL =JsonDatabase.GetTable<PL.Charity>().Where(c => c.CharityEmail == email).FirstOrDefault();

                if (Category.ID != Guid.Empty)
                        this.Category = new Category((Guid)charityPL.CategoryID);
                    this.Location = new Location((Guid)charityPL.LocationID);
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
            Clear();

            Email = charityEmail;

            try
            {
                PL.Charity charityPL = null;

                if (false)
                using (CCEntities dc = new CCEntities())
                {
                    //try to load existing ID
                    charityPL = dc.Charities.Where(c => c.CharityEmail == charityEmail).FirstOrDefault();
                }
                else
                    charityPL = JsonDatabase.GetTable<PL.Charity>().Where(c => c.CharityEmail == charityEmail).FirstOrDefault();

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
                Charity[] ctest = CharityCollection.INSTANCE.ToArray();
                Charity loadC = CharityCollection.INSTANCE.Where(c => c.Email == this.Email).FirstOrDefault();
                if (loadC != null)//retreive from existing
                    this.setCharity(loadC);
                else
                    LoadId(false);
            }
            else
                try
                {
                    PL.Charity entry = null;
                    if (false)
                        using (CCEntities dc = new CCEntities())
                        {
                            //base.LoadId();

                            //entry = JsonDatabase.GetTable<PL.Charity>().Where(c => c.CharityEmail == this.Email).FirstOrDefault();

                        }
                    else
                    {
                        base.LoadId(JsonDatabase.GetTable<PL.Charity>());
                        entry = JsonDatabase.GetTable<PL.Charity>().Where(c => c.CharityEmail == this.Email).FirstOrDefault();
                    }
                    this.Category = new Category(entry.CategoryID.Value);
                    this.Location = new Location(entry.LocationID.Value);
                }
                catch (Exception)
                { throw; }
        }
        internal void LoadId(string email,bool preloaded =true)
        {
            Email = email;
            LoadId(preloaded);
        }
        internal void Insert(Password password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password)); 

            this.Location.Insert();
            this.setProperty("LocationID", this.Location.ID);

            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.Insert(dc, dc.Charities);
                }
            else
                base.Insert(JsonDatabase.GetTable<PL.Charity>());

            password.Insert();
                    
            CharityCollection.AddToInstance(this);
        }
        internal void Update(Password password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.Update(dc, dc.Charities);
                }
            else
                base.Update(JsonDatabase.GetTable<PL.Charity>());

            this.Location.Update();
            password.Update();
            CharityCollection.AddToInstance(this);
        }
        internal void Delete(Password password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    base.Delete(dc, dc.Charities);
                }
            else
                base.Delete(JsonDatabase.GetTable<PL.Charity>());
            this.Location.Delete();
            password.Delete();
            CharityCollection.RemoveInstance(this);
        }
    }

    public class CharityCollection : CrudModelList<Charity,PL.Charity>
    {
        public static explicit operator CharityCollection(Charity[] carray)
        {
            CharityCollection ret = new CharityCollection();
            ret.AddRange(carray);
            return ret;
        }

        //START instance
        private static CharityCollection INS = new CharityCollection();
        internal static CharityCollection INSTANCE
        {
            get
            {
                if (INS == null || INS.Count == 0)
                    INS = LoadInstance();
                return INS;
            }
            private set => INS = value;
        }

        internal static CharityCollection LoadInstance()
        {
            try{


                if (!JsonDatabase.inintalized)
                    JsonDatabase.LoadDatabase();
                
                INSTANCE = new CharityCollection();
                if(false)
                using (CCEntities dc = new CCEntities())
                {
                    foreach (var c in dc.Charities.ToList())
                        INS.Add(new Charity(c));
                }
                else
                    foreach (var c in JsonDatabase.GetTable<PL.Charity>().ToList())
                        INS.Add(new Charity(c));
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

        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.BL.Test")]
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.API")]
        public Charity[] LoadAll()
        {
            this.Clear();
            LoadInstance();//make sure Instance is filled
            this.AddRange(INS);
            return INS.ToArray();
        }
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.BL.Test")]
        [assmbly: System.Runtime.CompilerServices.publicsVisibleTo("Doorfail.Connections.API")]
        internal void LoadWithFilter(object id, SortBy sort)
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
        internal void LoadWithPreferences(Volunteer userpreferences)
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

       // private new void Add(Charity item)
        //{
        //    base.Add(item);
        //}
        //private new void Remove(Charity item)
        //{
        //    base.Remove(item);
        //}
        public new void Add(Charity item)
        {
            if (SortID != null)
                throw new Exception(AddPreference_ERROR);
            base.Add(item);
        }
        //public new void Remove(Charity item)
        //{
        //   if (SortID != null)
        //       throw new Exception("Currently being used as a preference list. Please use DeletePreference instead");
        //    base.Remove(item);
        //}

        internal static int getCount() => new CCEntities().Charities.Count();

    }
}