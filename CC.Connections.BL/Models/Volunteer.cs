using Doorfail.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Doorfail.DataConnection;
using System.Data.Entity.Core;

namespace Doorfail.Connections.BL
{
    public class Volunteer : CrudModel_Json<PL.Volunteer>//TODO change to a ColumnEntry<PL.Volunteer>
    {

        [DisplayName("Member Email")]
        public string Email
        {
            get { return (string)base.ID; }
            set { base.ID = value; }
        }
        public Contact ContactInfo { get; set; }
        private CategoryPreferenceCollection preferedCategories;
        [DisplayName("Prefered Categories")]
        public CategoryPreferenceCollection PreferedCategories
        {
            get
            {
                if (preferedCategories == null)
                    preferedCategories = new CategoryPreferenceCollection(Email);
                return preferedCategories;
            }
            set
            {
                preferedCategories = value;
            }
        }
        private CharityCollection preferedCharities;
        [DisplayName("Prefered Charities")]
        public CharityCollection PreferedCharities
        {
            get
            {
                if (preferedCharities == null)
                {
                    preferedCharities = new CharityCollection();
                    preferedCharities.LoadWithPreferences(this);
                }
                return preferedCharities;
            }
            set
            {
                preferedCharities = value;
            }
        }
        private AbsMemberActionCollection preferedHelpingActions;
        [DisplayName("My Helping actions")]
        public AbsMemberActionCollection PreferedHelpingActions
        {
            get
            {
                if (preferedHelpingActions == null)
                    preferedHelpingActions = new AbsMemberActionCollection(Email);
                return preferedHelpingActions;
            }
            set
            {
                preferedHelpingActions = value;
            }
        }
        //depreciated
        //public Role Role { get; set; }
        private Preference pref;
        [DisplayName("My Preferences")]
        public Preference Pref
        {
            get
            {
                if (pref == null)
                    pref = new Preference((Guid)(base.getProperty(nameof(Preference) + "ID")));
                return pref;
            }
            set
            {
                pref = value;
                if (value != null)
                    base.setProperty(nameof(Preference) + "ID", value.ID);
            }
        }
        private Location loc;
        [DisplayName("My Location")]
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
                if (value != null)
                    base.setProperty(nameof(Location) + "ID", value.ID);
            }
        }

        internal static bool Exists(CCEntities dc, string id)
        {
            return dc.Volunteers.Where(c => c.VolunteerEmail == id).FirstOrDefault() != null;
        }

        //required for LogIns controller
        public Volunteer()
        {
            Clear();
        }
        public Volunteer(Password entry) : this(entry.email, true)
        {
            if (entry is null)
                throw new ArgumentNullException(nameof(entry));
        }
        public Volunteer(ContactInfo entry)
        {
            this.setContactInfo(entry);
            this.LoadId();
        }
        public Volunteer(PL.Volunteer entry)
        {
            if (entry == null)
                entry = new PL.Volunteer();//set to blank PL

            Email = entry.VolunteerEmail;
            Clear();
            setContactInfo(new Contact(entry.VolunteerEmail));

            Pref = new Preference((Guid)entry.PreferenceID);
            Location = new Location((Guid)entry.LocationID);

            PreferedCategories.LoadPreferences(Email);
            //Prefered_Charities.load();
            PreferedHelpingActions.LoadPreferences(Email);
        }

        private void setContactInfo(Contact contact)
        {
            this.Email = contact.MemberEmail;
            this.ContactInfo = contact;
        }

        //new member, member_type is hardcoded as guess
        //DOES NOT try to LOAD from database
        public Volunteer(string contactEmail, bool preloaded = true) :
            base(JsonDatabase.GetTable<PL.Volunteer>(), contactEmail, preloaded)
        {
            this.Email = contactEmail;
            if (preloaded)
                LoadId(true);
            else
            {
                PL.Volunteer charityPL = null;

                if (false)
                using (CCEntities dc = new CCEntities())
                {
                    charityPL = dc.Volunteers.Where(c => c.VolunteerEmail == contactEmail).FirstOrDefault();
                }
                else
                {
                    charityPL = JsonDatabase.GetTable<PL.Volunteer>().Where(c => c.VolunteerEmail == contactEmail).FirstOrDefault();
                }  
                PreferedHelpingActions = new AbsMemberActionCollection(contactEmail);
                PreferedCategories = new CategoryPreferenceCollection(contactEmail);
                PreferedHelpingActions.LoadPreferences(Email);
                PreferedCategories.LoadPreferences(Email);
            }
        }
        public void setVolunteer(Volunteer volunteer)
        {
            if (volunteer is null)
                throw new ArgumentNullException(nameof(volunteer));

            this.Email = volunteer.Email;
            this.ContactInfo = volunteer.ContactInfo;
            this.Location = volunteer.Location;
            this.Pref = volunteer.Pref;
            this.PreferedCategories = volunteer.PreferedCategories;
            this.PreferedCharities = volunteer.PreferedCharities;
            this.PreferedHelpingActions = volunteer.PreferedHelpingActions;
        }
        public void setVolunteerInfo(PL.Volunteer volunteer)
        {
            setVolunteer(new Volunteer(volunteer));
        }

        public Volunteer(string email, string password = "", bool hashed = false, bool debug = false)
        {
            try
            {
                Email = email;
                Clear();
                PL.Volunteer volunteerPL = null;

                if (false)
                using (CCEntities dc = new CCEntities())
                {
                    volunteerPL = dc.Volunteers.Where(c => c.VolunteerEmail == email).FirstOrDefault();
                }
                else
                    volunteerPL =JsonDatabase.GetTable<PL.Volunteer>().Where(c => c.VolunteerEmail == email).FirstOrDefault();

                //TODO refactor

                //check for existing account before inserting                    
                    if (volunteerPL != null)
                {
                    setVolunteerInfo(volunteerPL);
                    PreferedHelpingActions = new AbsMemberActionCollection(email);
                    PreferedCategories = new CategoryPreferenceCollection(email);
                    PreferedHelpingActions.LoadPreferences(Email);
                    PreferedCategories.LoadPreferences(Email);
                }
                else//its a new account
                {
                    Clear();//new Charity
                    Password newPassword = new Password(email, password, MemberType.VOLLUNTEER, hashed);
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

        public static implicit operator Volunteer(PL.Volunteer entry)
        { return new Volunteer(entry); }

        //only clears BLclass varibles
        private new void Clear()
        {
            ContactInfo = new Contact();
            PreferedCategories = new CategoryPreferenceCollection(Email);
            PreferedCharities = new CharityCollection();
            PreferedHelpingActions = new AbsMemberActionCollection(Email);
            Pref = new Preference();
            Location = new Location();
        }

        public void LoadId(bool preloaded = true)
        {
            if (preloaded)
            {
                Volunteer loadC = VolunteerCollection.INSTANCE.Where(c => c.Email == this.Email).FirstOrDefault();
                if (loadC != null)//retreive from existing
                    this.setVolunteer(loadC);
                else
                    LoadId(false);
            }
            else
                try
                {
                    //TODO IMPORTANT: I HAVE NOT CLUE WHAT THIS IS DOING figure it out
                    if (false)
                    using (CCEntities dc = new CCEntities())
                    {
                        base.LoadId(JsonDatabase.GetTable<PL.Volunteer>());
                        PL.Volunteer entry = dc.Volunteers.Where(c => c.VolunteerEmail == this.Email).FirstOrDefault();
                    }
                    else
                    {
                        base.LoadId(JsonDatabase.GetTable<PL.Volunteer>());
                    }

                    ContactInfo = new Contact(Email);
                    PreferedHelpingActions = new AbsMemberActionCollection(Email);
                    PreferedCategories = new CategoryPreferenceCollection(Email);
                    PreferedHelpingActions.LoadPreferences(Email);
                    PreferedCategories.LoadPreferences(Email);
                }
                catch (Exception)
                { throw; }
        }
        public void LoadId(string email, bool preloaded = true)
        {
            Email = email;
            LoadId(preloaded);
        }
        public void Insert(Password password)
        {
            try
            {
                if (password == null)
                    throw new ArgumentNullException(nameof(password));
                //double check ID before insert
                if (String.IsNullOrEmpty(Email))
                    throw new NullReferenceException("Cannot insert with an empty email");
                if (Pref == null)
                    Pref = new Preference();
                if (Location == null)
                    Location = new Location();

                ContactInfo.Insert();
                Pref.Insert();
                this.Location.Insert();
                this.setProperty("LocationID", this.Location.ID);

                if (false)
                using (CCEntities dc = new CCEntities())
                {
                   
                    base.Insert(dc, dc.Volunteers);
                    
                }
                else
                {
                    base.Insert(JsonDatabase.GetTable<PL.Volunteer>());
                }
                password.Insert();
                VolunteerCollection.AddToInstance(this);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int Delete(Password password)
        {
            if (password is null)
                throw new ArgumentNullException(nameof(password));

            try
            { 
                password.Delete();
                    Pref.Delete();
                    Location.Delete();

                if(false)
                using (CCEntities dc = new CCEntities())
                {

                    base.Delete(dc,dc.Volunteers);

                    dc.Volunteers.Remove(dc.Volunteers.Where(c => c.VolunteerEmail == Email).FirstOrDefault());
                    ContactInfo.Delete();
                    PreferedCategories.DeleteAllPreferences();
                    //Prefered_Charities.DeleteAllPreferences();
                    PreferedHelpingActions.DeleteAllPreferences();

                    Clear();
                    return dc.SaveChanges();
                }
                else
                {
                    base.Delete(JsonDatabase.GetTable<PL.Volunteer>());

                    JsonDatabase.GetTable<PL.Volunteer>().Remove(JsonDatabase.GetTable<PL.Volunteer>().Where(c => c.VolunteerEmail == Email).FirstOrDefault());
                    ContactInfo.Delete();
                    PreferedCategories.DeleteAllPreferences();
                    //Prefered_Charities.DeleteAllPreferences();
                    PreferedHelpingActions.DeleteAllPreferences();

                    Clear();
                    JsonDatabase.SaveChanges();
                    return 1;
                }
            }
            catch (Exception e) { throw; }
        }
        public int Update(Password password)
        {
            if (password is null)
                throw new ArgumentNullException(nameof(password));
            

            try
            {
                if(false)
                using (CCEntities dc = new CCEntities())
                {
                    PL.Volunteer entry = dc.Volunteers.Where(c => c.VolunteerEmail == this.Email).FirstOrDefault();
                    ContactInfo.Update();
                    base.Update(dc,dc.Volunteers);
                    Pref.Update();
                    //Member_Type.Update();
                    Location.Update();
                    password.Update();

                    //do not handle member preference lists here

                    return dc.SaveChanges();
                }
                else
                {
                    PL.Volunteer entry = JsonDatabase.GetTable<PL.Volunteer>().Where(c => c.VolunteerEmail == this.Email).FirstOrDefault();
                    ContactInfo.Update();
                    base.Update(JsonDatabase.GetTable<PL.Volunteer>());
                    Pref.Update();
                    //Member_Type.Update();
                    Location.Update();
                    password.Update();

                    //do not handle member preference lists here

                    JsonDatabase.SaveChanges();
                    return 1;
                }
            }
            catch (Exception e) { throw; }
        }
    }

    public class VolunteerCollection
    : CrudModelList<Volunteer, PL.Volunteer>
    {
        //START instance
        private static VolunteerCollection INS = new VolunteerCollection();
        public static VolunteerCollection INSTANCE
        {
            get
            {
                if (INS == null || INS.Count == 0)
                    INS = LoadInstance();
                return INS;
            }
            private set => INS = value;
        }

        public static VolunteerCollection LoadInstance()
        {
            try
            {
                INSTANCE = new VolunteerCollection();
                if(false)
                using (CCEntities dc = new CCEntities())
                {
                    foreach (var c in dc.Volunteers.ToList())
                        INS.Add(new Volunteer(c));
                }
                else foreach (var c in JsonDatabase.GetTable<PL.Volunteer>().ToList())
                        INS.Add(new Volunteer(c));

                return INS;
            }
            catch (EntityException e) { throw e.InnerException; }
        }
        public static void AddToInstance(Volunteer charity)
            => INS.Add(charity);

        //Might be able to optimize better
        internal static void UpdateInstance(Volunteer charity)
        {
            RemoveInstance(charity);
            AddToInstance(charity);
        }

        internal static void RemoveInstance(Volunteer charity)
            => INSTANCE.Remove(charity);
        //END instance

        public void LoadAll()
        {
            this.Clear();
            LoadInstance();//make sure Instance is filled
            this.AddRange(INS);
        }

        public static int getCount() => new CCEntities().Volunteers.Count();

    }
}