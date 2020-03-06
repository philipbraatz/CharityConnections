using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace CC.Connections.BL
{
    public class Volunteer : Contact
    {
        [DisplayName("Prefered Categories")]
        public CategoryPreferenceCollection PreferedCategories { get; set; }
        [DisplayName("Prefered Charities")]
        public CharityCollection PreferedCharities { get; set; }
        [DisplayName("My Helping actions")]
        public AbsMemberActionCollection PreferedHelpingActions { get; set; }
        //depreciated
        //public Role Role { get; set; }
        [DisplayName("My Preferences")]
        public Preference Pref { get; set; }
        [DisplayName("My Location")]
        public Location Location { get; set; }

        internal static bool Exists(CCEntities dc, string id)
        {
            return dc.Volunteers.Where(c => c.VolunteerEmail == id).FirstOrDefault() != null;
        }

        //required for LogIns controller
        public Volunteer()
        {
            Clear();
        }
        public Volunteer(ContactInfo entry) :
            base(entry)
        { }
        public Volunteer(PL.Volunteer entry)
        {
            if(entry == null)
                entry = new PL.Volunteer();//set to blank PL

            MemberEmail = entry.VolunteerEmail;
            Clear();
            base.setContactInfo( new Contact(entry.VolunteerEmail));

            Pref = new Preference((Guid)entry.PreferenceID);
            Location = new Location((Guid)entry.LocationID);

            PreferedCategories.LoadPreferences(MemberEmail);
            //Prefered_Charities.load();
            PreferedHelpingActions.LoadPreferences(MemberEmail);
        }
        //new member, member_type is hardcoded as guess
        //DOES NOT try to LOAD from database
        public Volunteer(string contactEmail, string password="", bool hashed = false, bool debug = false)
        {
            //get ID and password from other tables
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    MemberEmail = contactEmail;//first entry


                    //try to load existing ID
                    PL.ContactInfo cID = dc.ContactInfoes.Where(c => c.MemberEmail == contactEmail).FirstOrDefault();
                    if (cID != null)
                    {
                        base.setContactInfo(new Contact(cID));

                        //will LOAD nothing if NEW ID but will SET preference ID
                        //needed for adding and removing items
                        PreferedHelpingActions = new AbsMemberActionCollection(contactEmail);
                        PreferedCategories = new CategoryPreferenceCollection(contactEmail);
                        PreferedHelpingActions.LoadPreferences(MemberEmail);
                        PreferedCategories.LoadPreferences(MemberEmail);
                        //Prefered_Charities.LoadPreferences(ID);

                        //this.LoadId();
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
        //load
        public Volunteer(string email)
        {
            //get ID and password from other tables
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    PL.ContactInfo cID = dc.ContactInfoes.Where(c => c.MemberEmail == email).FirstOrDefault();
                    if (cID == null)
                        throw new Exception("Email " + email + " does not have a contact info");
                    this.LoadId(email);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        //public static implicit operator AbsMember(PL.Member entry)
        //{
        //    return new AbsMember(entry);
        //}

        //only clears BLclass varibles
        private new void Clear()
        {
            PreferedCategories = new CategoryPreferenceCollection(MemberEmail);
            PreferedCharities = new CharityCollection();
            PreferedHelpingActions = new AbsMemberActionCollection(MemberEmail);
            Pref = new Preference();
            Location = new Location();
        }
        public bool Insert(Password password)
        {
            if (base.Insert())//checks for existing
                try
                {
                    //if (ID == string.Empty)
                    //    throw new Exception("Description cannot be empty");
                    using (CCEntities dc = new CCEntities())
                    {
                        //double check ID before insert
                        if (String.IsNullOrEmpty(MemberEmail))
                            throw new NullReferenceException("Cannot insert an empty email");


                        if (Pref == null)
                            Pref = new Preference();
                        if (Location == null)
                            Location = new Location();
                        Pref.Insert();
                        //Member_Type.Insert();
                        Location.Insert();
                        PL.Volunteer entry = new PL.Volunteer
                        {
                            VolunteerEmail = MemberEmail,
                            PreferenceID = Pref.ID,
                            LocationID = Location.ID
                        };
                        dc.Volunteers.Add(entry);//adding prior to everything else
                        password.Insert();

                        //do not handle member preference lists here

                        return dc.SaveChanges() > 0;
                    }
                }
                catch (Exception e) { throw e; }
            else
                return false;
        }
        public int Delete(Password password)
        {
            if (password is null)
                throw new ArgumentNullException(nameof(password));

            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");

                    password.Delete();
                    Pref.Delete();
                    Location.Delete();
                    base.Delete();
                    //dc.Roles.Remove(dc.Roles.Where(c => c.Role_ID == ID).FirstOrDefault());

                    dc.Volunteers.Remove(dc.Volunteers.Where(c => c.VolunteerEmail == MemberEmail).FirstOrDefault());

                    PreferedCategories.DeleteAllPreferences();
                    //Prefered_Charities.DeleteAllPreferences();
                    PreferedHelpingActions.DeleteAllPreferences();
                    
                    Clear();
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public int Update(Password password)
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (CCEntities dc = new CCEntities())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");

                    PL.Volunteer entry = dc.Volunteers.Where(c => c.VolunteerEmail == this.MemberEmail).FirstOrDefault();
                    base.Update();
                    Pref.Update();
                    //Member_Type.Update();
                    Location.Update();
                    password.Update();

                    //do not handle member preference lists here

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw; }
        }

        //TODO remove in favor of email
        public void LoadId(int id)
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {

                    PL.Volunteer entry = dc.Volunteers.Where(c => c.VolunteerEmail == MemberEmail).FirstOrDefault();
                    if (entry != null)
                    {
                        this.MemberEmail = entry.VolunteerEmail;
                        Clear();//make sure member fields are created
                        base.setContactInfo(new Contact(entry.VolunteerEmail));
                    }
                    else
                        throw new Exception("ID = " + id + ", does not have a Member associated with it");
                    ///all good LogIns in related values
                    
                    PL.LogIn LogIns = dc.LogIns.FirstOrDefault(c => c.MemberEmail == this.MemberEmail);
                    if (LogIns == null)
                        throw new Exception("Log in does not exist for Member with email " + this.MemberEmail);

                    if (entry.PreferenceID == null)
                        throw new Exception("Preference ID is null and cannot be loaded");
                    this.Pref = new Preference((Guid)entry.PreferenceID);
                    this.Location = new Location((Guid)entry.LocationID);

                    this.PreferedCategories.LoadPreferences(entry.VolunteerEmail);
                    this.PreferedHelpingActions.LoadPreferences(entry.VolunteerEmail);
                    //this.Prefered_Charities.LoadPreferences( entry.Member_ID);


                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public new void LoadId(string email)
        {
            base.LoadId(email);

            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");

                    PL.Volunteer entry = dc.Volunteers.Where(c => c.VolunteerEmail == email).FirstOrDefault();
                    if (entry != null)
                        this.MemberEmail = email;
                    else
                        throw new Exception("Contact info " + email + " does not have a Member associated with it");
                    ///all good LogIns in related values
                    Clear();//make sure member fields are created

                    PL.LogIn LogIns = dc.LogIns.FirstOrDefault(c => c.MemberEmail == this.MemberEmail);
                    if (LogIns == null)
                        throw new Exception("Log in does not exist for Member with email " + this.MemberEmail);

                    if (entry.PreferenceID == null)
                        throw new Exception("Preference ID is null and cannot be loaded");
                    this.Pref = new Preference((Guid)entry.PreferenceID);
                    this.Location = new Location((Guid)entry.LocationID);

                    this.PreferedCategories.LoadPreferences(entry.VolunteerEmail);
                    this.PreferedHelpingActions.LoadPreferences(entry.VolunteerEmail);
                    //this.Prefered_Charities.LoadPreferences( entry.Member_ID);


                }
            }
            catch (Exception e)
            {
                //probably could not find the data it was looking for
                throw;
            }
        }

        //loads contact info only
        internal static Volunteer loadContactInfo(string memberContact_ID)
        {
            Contact info = new Contact(memberContact_ID);
            return new Volunteer
            {
                MemberEmail = info.MemberEmail,
                FName = info.FName,
                LName = info.LName,
                Phone = info.Phone,
                DateOfBirth = info.DateOfBirth
            };
        }

        //base.setContactInfo to set only contact info
    }

    public class MemberCollection
    : List<Volunteer>
    {
        public void LoadList()
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    if (dc.Volunteers.ToList().Count != 0)
                        dc.Volunteers.ToList().ForEach(c =>
                        {
                            Volunteer newMem = Volunteer.loadContactInfo(c.VolunteerEmail);
                            newMem.MemberEmail = c.VolunteerEmail;
                            newMem.Pref = new Preference((Guid)c.PreferenceID);
                            newMem.Location = new Location((Guid)c.LocationID);

                            newMem.PreferedCategories.LoadPreferences(c.VolunteerEmail);
                            newMem.PreferedHelpingActions.LoadPreferences(c.VolunteerEmail);

                            this.Add(newMem);
                        });
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}