using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CC.Connections.BL
{
    public class Volunteer : Contact
    {
        public CategoryPreferences Prefered_Categories { get; set; }
        public CharityList Prefered_Charities { get; set; }
        public AbsMemberActionList Prefered_helping_Actions { get; set; }
        //depreciated
        //public Role Role { get; set; }
        public Preference Pref { get; set; }
        public Location Location { get; set; }

        internal static bool Exists(CCEntities dc, string id)
        {
            return dc.Volunteers.Where(c => c.Volunteer_Email == id).FirstOrDefault() != null;
        }

        //required for login controller
        public Volunteer()
        {
            Clear();
        }
        public Volunteer(Contact_Info entry) :
            base(entry)
        { }
        public Volunteer(PL.Volunteer entry)
        {
            if(entry == null)
                entry = new PL.Volunteer();//set to blank PL

            Member_Email = entry.Volunteer_Email;
            Clear();
            base.setContactInfo( new Contact(entry.Volunteer_Email));

            Pref = new Preference((Guid)entry.Preference_ID);
            Location = new Location((Guid)entry.Location_ID);

            Prefered_Categories.LoadPreferences(Member_Email);
            //Prefered_Charities.load();
            Prefered_helping_Actions.LoadPreferences(Member_Email);
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
                    Member_Email = contactEmail;//first entry


                    //try to load existing ID
                    PL.Contact_Info cID = dc.Contact_Info.Where(c => c.Member_Email == contactEmail).FirstOrDefault();
                    if (cID != null)
                    {
                        base.setContactInfo(new Contact(cID));

                        //will LOAD nothing if NEW ID but will SET preference ID
                        //needed for adding and removing items
                        Prefered_helping_Actions = new AbsMemberActionList(contactEmail);
                        Prefered_Categories = new CategoryPreferences(contactEmail);
                        Prefered_helping_Actions.LoadPreferences(Member_Email);
                        Prefered_Categories.LoadPreferences(Member_Email);
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
                    PL.Contact_Info cID = dc.Contact_Info.Where(c => c.Member_Email == email).FirstOrDefault();
                    if (cID == null)
                        throw new Exception("Email " + email + " does not have a contact info");
                    //else
                    //{
                    //    PL.Member mID = dc.Members.Where(c => c.MemberContact_ID == cID.Contact_Info_ID).FirstOrDefault();
                    //    ID = (int)mID.Member_ID;
                    //}
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
            Prefered_Categories = new CategoryPreferences(Member_Email);
            Prefered_Charities = new CharityList();
            Prefered_helping_Actions = new AbsMemberActionList(Member_Email);
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
                        if (String.IsNullOrEmpty(Member_Email))
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
                            Volunteer_Email = Member_Email,
                            Preference_ID = Pref.ID,
                            Location_ID = Location.ID
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
        public new int Delete(Password password)
        {
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

                    dc.Volunteers.Remove(dc.Volunteers.Where(c => c.Volunteer_Email == Member_Email).FirstOrDefault());

                    Prefered_Categories.DeleteAllPreferences();
                    //Prefered_Charities.DeleteAllPreferences();
                    Prefered_helping_Actions.DeleteAllPreferences();
                    
                    Clear();
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public new int Update(Password password)
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (CCEntities dc = new CCEntities())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");

                    PL.Volunteer entry = dc.Volunteers.Where(c => c.Volunteer_Email == this.Member_Email).FirstOrDefault();
                    base.Update();
                    Pref.Update();
                    //Member_Type.Update();
                    Location.Update();
                    password.Update();

                    //do not handle member preference lists here

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        //TODO remove in favor of email
        public new void LoadId(int id)
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");
                    int contactId;

                    PL.Volunteer entry = dc.Volunteers.Where(c => c.Volunteer_Email == Member_Email).FirstOrDefault();
                    if (entry != null)
                    {
                        this.Member_Email = entry.Volunteer_Email;
                        Clear();//make sure member fields are created
                        base.setContactInfo(new Contact(entry.Volunteer_Email));
                    }
                    else
                        throw new Exception("ID = " + id + ", does not have a Member associated with it");
                    ///all good Login in related values
                    
                    PL.Log_in login = dc.Log_in.FirstOrDefault(c => c.Member_Email == this.Member_Email);
                    if (login == null)
                        throw new Exception("Log in does not exist for Member with email " + this.Member_Email);

                    if (entry.Preference_ID == null)
                        throw new Exception("Preference ID is null and cannot be loaded");
                    this.Pref = new Preference((Guid)entry.Preference_ID);
                    this.Location = new Location((Guid)entry.Location_ID);

                    this.Prefered_Categories.LoadPreferences(entry.Volunteer_Email);
                    this.Prefered_helping_Actions.LoadPreferences(entry.Volunteer_Email);
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

                    PL.Volunteer entry = dc.Volunteers.Where(c => c.Volunteer_Email == email).FirstOrDefault();
                    if (entry != null)
                        this.Member_Email = email;
                    else
                        throw new Exception("Contact info " + email + " does not have a Member associated with it");
                    ///all good Login in related values
                    Clear();//make sure member fields are created

                    PL.Log_in login = dc.Log_in.FirstOrDefault(c => c.Member_Email == this.Member_Email);
                    if (login == null)
                        throw new Exception("Log in does not exist for Member with email " + this.Member_Email);

                    if (entry.Preference_ID == null)
                        throw new Exception("Preference ID is null and cannot be loaded");
                    this.Pref = new Preference((Guid)entry.Preference_ID);
                    this.Location = new Location((Guid)entry.Location_ID);

                    this.Prefered_Categories.LoadPreferences(entry.Volunteer_Email);
                    this.Prefered_helping_Actions.LoadPreferences(entry.Volunteer_Email);
                    //this.Prefered_Charities.LoadPreferences( entry.Member_ID);


                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        //loads contact info only
        internal static Volunteer loadContactInfo(string memberContact_ID)
        {
            Contact info = new Contact(memberContact_ID);
            return new Volunteer
            {
                Member_Email = info.Member_Email,
                FName = info.FName,
                LName = info.LName,
                Phone = info.Phone,
                DateOfBirth = info.DateOfBirth
            };
        }

        //sets contact info only
        public void setContactInfo(Contact contact)
        {
            Member_Email = contact.Member_Email;
            FName = contact.FName;
            LName = contact.LName;
            Phone = contact.Phone;
            DateOfBirth = contact.DateOfBirth;
        }
    }

    public class MemberList
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
                            Volunteer newMem = Volunteer.loadContactInfo(c.Volunteer_Email);
                            newMem.Member_Email = c.Volunteer_Email;
                            newMem.Pref = new Preference((Guid)c.Preference_ID);
                            newMem.Location = new Location((Guid)c.Location_ID);

                            newMem.Prefered_Categories.LoadPreferences(c.Volunteer_Email);
                            newMem.Prefered_helping_Actions.LoadPreferences(c.Volunteer_Email);

                            this.Add(newMem);
                        });
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}