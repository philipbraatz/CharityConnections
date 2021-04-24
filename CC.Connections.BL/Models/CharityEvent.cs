using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doorfail.Connections.PL;
using Doorfail.Connections.BL;
using System.Data.Entity.Core;
using Doorfail.DataConnection;

namespace Doorfail.Connections.BL
{
    public class CharityEvent : CrudModel_Json<PL.CharityEvent>
    {
        //private static CCEntities dc;

        public new Guid ID
        {
            get { return (Guid)base.ID; }
            set { base.ID = value; }
        }
        public String CharityEmail
        {
            get { return (string)base.getProperty(nameof(CharityEmail)); }
            set { setProperty(nameof(CharityEmail), value); }
        }
        [DisplayName("Charity Event")]
        public string Name
        {
            get { return (string)base.getProperty(nameof(Name)); }
            set { setProperty(nameof(Name), value); }
        }

        private Location loc;
        [DisplayName("Location")]
        public Location Location
        {
            get
            {
                if (loc == null)
                    loc = new Location((Guid)(base.getProperty(nameof(Location) + "ID")));
                return loc;
            }
            set
            {
                loc = value;
                if (value != null)
                    base.setProperty(nameof(Location) + "ID", value.ID);
            }
        }

        public EventAttendanceJointCollection atendees { get; set;}

        //TODO use base class for datetime
        private DateTime _start { get; set; }
        private DateTime _end { get; set; }

        [DisplayName("Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate
        {
            get => DateTime.Parse(_start.ToShortDateString());
            set
            {
                _start = value.Add(StartTime.TimeOfDay);
            }
        }

        [DisplayName("Open Time")]
        [DataType(DataType.DateTime)]
        public DateTime StartTime
        {
            get => DateTime.Parse(_start.ToShortTimeString());
            set
            {
                _start = _start.Date.Add(value.TimeOfDay);
            }
        }

        [DisplayName("End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate
        {
            get => DateTime.Parse(_end.ToShortDateString());
            set
            {
                _end = value.Add(EndTime.TimeOfDay);
            }
        }

        [DisplayName("Close Time")]
        [DataTypeTime(DataType.Date)]
        public DateTime EndTime
        {
            get => DateTime.Parse(_end.ToShortTimeString());
            set
            {
                _end = _end.Date.Add(value.TimeOfDay);
            }
        }

        //created on the fly
        [DisplayName("Status")]
        public string CharityEventStatus
        {
            get
            {
                if (DateTime.Now < StartDate)
                    return "Upcoming";
                else if (DateTime.Now > _start &&
                        DateTime.Now < _end)
                {
                    if (DateTime.Parse(DateTime.Now.ToShortTimeString()) > StartTime &&
                        DateTime.Parse(DateTime.Now.ToShortTimeString()) < EndTime)
                        return "Ongoing";
                    else
                        return "Closed";
                }
                else
                    return "Completed";
            }
        }

        public PL.CharityEvent toPL()
        {
            return new PL.CharityEvent {
                ID = this.ID,
                Name = this.Name,
                CharityEmail = this.CharityEmail,
                LocationID = this.Location.ID,
                StartDate = this.StartDate,
                Description = this.Description,
                Requirements = this.Requirements,
                EndDate = this.EndDate
           };
        }

        //display fields
        private Charity chrty;
        public Charity Charity
        {
            get
            {
                if (chrty == null)
                    chrty = new Charity((string)(base.getProperty(nameof(Charity) + "Email")),true);
                return chrty;
            }
            set
            {
                chrty = value;
                if (value != null)
                    base.setProperty(nameof(Charity) + "Email", value.Email);
            }
        }
        public EventAttendee Member_Attendance { get; set; }


        public void AddMember(string email, Status status)
        {
            EventAttendee atendee = new EventAttendee(this.ID, email)
            {VolunteerStatus = status};
            atendee.Insert();
            this.atendees.Add(atendee);
        }
        public void RemoveMember(string email)
        {
            this.atendees.Remove(email);
        }

        [DisplayName("Requirements")]
        public string Requirements
        {
            get { return (string)base.getProperty(nameof(Requirements)); }
            set { setProperty(nameof(Requirements), value); }
        }
        [DisplayName("Description")]
        public string Description
        {
            get { return (string)base.getProperty(nameof(Description)); }
            set { setProperty(nameof(Description), value); }
        }

        //TODO refactor
        public CharityEvent() :
            base()
        {
            Clear();
        }
        public CharityEvent(PL.CharityEvent entry) :
           base(entry)
        {
            this.atendees = new EventAttendanceJointCollection(this.ID,false);
        }
        public CharityEvent(Guid id, bool preloaded = true) :
           base(JsonDatabase.GetTable<PL.CharityEvent>(), id, preloaded)
        {
            this.ID = id;
            if (preloaded)
                LoadId(true);
        }
        //public static implicit operator CharityEvent(PL.CharityEvent entry)
        //{ return new CharityEvent(entry,true); }

        public CharityEvent(Guid charityEventID, Charity charity = null, string memberEmail = "")
        {
            this.Charity = charity;
            if (!string.IsNullOrEmpty(memberEmail))
                this.Member_Attendance = new EventAttendee(charityEventID, memberEmail);
            this.ID = charityEventID;
            Clear();
            LoadId();
        }


        //public static implicit operator CharityEvent(PL.CharityEvent entry)
        //{ return new CharityEvent(entry.ID); }

        //public static implicit operator CharityEvent(PL.Charities entry)
        //{ return new CharityEvent(entry.Charity_Contact_ID); }
        public static bool Exists(CCEntities dc, Guid eventID)
        {
            if (dc == null)
                throw new ArgumentNullException(nameof(dc));
            return dc.CharityEvents.Where(c => c.ID == eventID).FirstOrDefault() != null;
        }

        private new void Clear()
        {
            this.Location = new PL.Location();
            this.atendees = new EventAttendanceJointCollection(this.ID);
        }

        protected void setEventInfo(PL.CharityEvent charEvent)
        {
            if (charEvent == null)
                throw new ArgumentNullException(nameof(charEvent));

            this.ID = charEvent.ID;
            this.Name = charEvent.Name;
            this.Requirements = charEvent.Requirements;
            this.CharityEmail = charEvent.CharityEmail;
            this._start = (DateTime)charEvent.StartDate;
            this._end = (DateTime)charEvent.EndDate;
            if (charEvent.LocationID == null)
                throw new Exception("Charity Event ID " + this.ID + " doesn't not have a location set");
            this.Location = new Location((Guid)charEvent.LocationID);
            this.Description = charEvent.Description;
            this.atendees = new EventAttendanceJointCollection(charEvent.ID);
        }
        protected void setEventInfo(CharityEvent evnt)
        {
            if (evnt is null)
                throw new ArgumentNullException(nameof(evnt));


            this.ID           = evnt.ID;
            this.Name         = evnt.Name;
            this.Requirements = evnt.Requirements;
            this.CharityEmail = evnt.CharityEmail;
            this._start       = evnt._start;
            this._end         = evnt._end;
            this.Location     = evnt.Location;
            this.Description  = evnt.Description;
            this.atendees     = new EventAttendanceJointCollection(evnt.ID);
        }

        //TODO refactor to use CC.Abstract
        public int Insert()
        {
            this.Location.Insert();

            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                ID = Guid.NewGuid();
                PL.CharityEvent cevent = new PL.CharityEvent
                {
                    ID = this.ID,
                    CharityEmail = this.CharityEmail,
                    LocationID =  this.Location.ID,
                    StartDate = this._start,
                    EndDate = this._end,
                    Requirements = this.Requirements,
                    Name = this.Name,
                    Description = this.Description
                };
                

                CharityEventCollection.AddToInstance(this);
                if (false)
                    using (CCEntities dc = new CCEntities())
                    {
                        dc.CharityEvents.Add(cevent);
                        return dc.SaveChanges();
                    }
                else
                {
                    JsonDatabase.GetTable<PL.CharityEvent>().Add(cevent);
                    JsonDatabase.SaveChanges();
                    return 1;
                }
            }
            catch (DbEntityValidationException e)
            {
                //throw e;
                throw new Exception(e.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage);
            }
            catch (Exception) { throw; }
        }
        public int Delete()
        {
            try
            {
                if(false)
                using (CCEntities dc = new CCEntities())
                {
                    dc.CharityEvents.Remove(dc.CharityEvents.Where(c => c.ID == this.ID).FirstOrDefault());
                    Location.Delete();
                    atendees.DeleteAttendance();
                    CharityEventCollection.RemoveInstance(this);
                    return dc.SaveChanges();
                }
                else
                {
                    JsonDatabase.GetTable<PL.CharityEvent>().Remove(JsonDatabase.GetTable<PL.CharityEvent>().Where(c => c.ID == this.ID).FirstOrDefault());
                    Location.Delete();
                    atendees.DeleteAttendance();
                    CharityEventCollection.RemoveInstance(this);
                    JsonDatabase.SaveChanges();
                    return 1;
                }
            }
            catch (Exception) { throw; }
        }
        public int Update()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                if(false)
                using (CCEntities dc = new CCEntities())
                {
                    PL.CharityEvent entry = dc.CharityEvents.Where(c => c.ID == this.ID).FirstOrDefault()
                        ?? throw new Exception("Could not find Charity Event with ID: " + this.ID);
                    entry.EndDate = _end;
                    entry.StartDate = _start;
                    entry.Name = Name;
                    entry.Requirements = Requirements;
                    entry.StartDate = StartDate;
                    entry.Description = Description;

                    CharityEventCollection.RemoveInstance(this);
                    return dc.SaveChanges();
                }
                else
                {
                    PL.CharityEvent entry = JsonDatabase.GetTable<PL.CharityEvent>().Where(c => c.ID == this.ID).FirstOrDefault()
                       ?? throw new Exception("Could not find Charity Event with ID: " + this.ID);
                    entry.EndDate = _end;
                    entry.StartDate = _start;
                    entry.Name = Name;
                    entry.Requirements = Requirements;
                    entry.StartDate = StartDate;
                    entry.Description = Description;

                    CharityEventCollection.RemoveInstance(this);
                    JsonDatabase.SaveChanges();
                    return 1;
                }
            }
            catch (Exception) { throw; }
        }
        public void LoadId(bool preloaded = true)
        {
            if (preloaded == false)//definitly needs to be taken from database
                try
                {
                    using (CCEntities dc = new CCEntities())
                    {
                        //if (this.ID == Guid.Empty)
                        //    throw new Exception("ID is invalid");

                        PL.CharityEvent entry = dc.CharityEvents.FirstOrDefault(c => c.ID == this.ID)
                            ?? throw new Exception("Event does not exist ID: " + ID);
                        //if (entry.CharityEventContactInfo_ID == null)
                        //    throw new Exception("Event does not have a Contact Info");
                        setEventInfo(entry);//LOADS
                        atendees.LoadByEvent(entry.ID);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            else
            {
                CharityEvent loadC = CharityEventCollection.INSTANCE.Where(c => c.ID == this.ID).FirstOrDefault();
                if (loadC != null)//retreive from existing
                    this.setEventInfo(loadC);
                else//load from database
                    LoadId(false);
            }

        }


        public bool IsGoing(String email)
        {
            return atendees.Where(c => c.VolunteerEmail == email).FirstOrDefault().VolunteerStatus == Status.GOING;
        }
        public bool IsInterested(String email)
        {
            return atendees.Where(c => c.VolunteerEmail == email).FirstOrDefault().VolunteerStatus == Status.INTERESTED;
        }
    }

    public class CharityEventCollection
        : CrudModelList<CharityEvent,PL.CharityEvent>
    {
        private static CharityEventCollection ins = new CharityEventCollection();
        public static CharityEventCollection INSTANCE
        {
            get
            {
                if (ins == null || ins.Count == 0)
                    ins = LoadInstance();
                return ins;
            }
            private set => ins = value;
        }

        public static explicit operator CharityEventCollection(CharityEvent[] carray)
        {
            CharityEventCollection ret = new CharityEventCollection();
            if(!(carray is null))
                ret.AddRange(carray);
            return ret;
        }

        //Everything is loaded from here when first needed!
        public static CharityEventCollection LoadInstance()
        {
            try
            {
                ins = new CharityEventCollection();
                using (CCEntities dc = new CCEntities())
                {
                    foreach (var c in dc.CharityEvents.ToList())
                        ins.Add(new CharityEvent(c));
                }
                return ins;
            }
            catch (EntityException e) { throw e.InnerException; }
        }
        public static void AddToInstance(CharityEvent category)
        {
            ins.Add(category);
        }
        //Might be able to optimize better
        internal static void UpdateInstance(CharityEvent category)
        {
            RemoveInstance(category);
            AddToInstance(category);
        }

        internal static void RemoveInstance(CharityEvent category)
        {
            ins.Remove(category);
        }

        public CharityEvent[] LoadAll()
        {
            this.Clear();
            LoadInstance();//make sure Instance is filled
            this.AddRange(ins);
            return ins.ToArray();
        }

        //only used for Event lists
        private object Sort_ID { get; set; }
        private SortBy sorter { get; set; }
        private Volunteer userPref { get; set; }

        private const string Event_LOAD_ERROR = "Events not loaded, please loadEvents with a Charity ID";

        public CharityEventCollection()
        {
            sorter = SortBy.NONE;
        }
        public CharityEventCollection(int id, SortBy sort, Volunteer userPref = default)
        {
            Sort_ID = id;
            sorter = sort;
            this.userPref = userPref;
            LoadAll();
        }

        public void setPreferences(Volunteer userPref)
        {
            this.userPref = userPref;
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
                    filter.Whitelist_Remaining_Events(new List<Guid> { (Guid)id }, new List<string>(), new List<Guid>());
                    this.Clear();
                    foreach (var item in filter.GetRemainingEvents())
                    {
                        this.Add(item);
                    }
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
        public void LoadWithPreferences(Volunteer user_preferences)
        {
            userPref = user_preferences;
            Clear();
            Filterer filter = new Filterer();
            filter.CutEventByPreferences(userPref);
            foreach (var item in filter.GetRemainingEvents())
                this.Add(item, true);
        }



        internal void DeleteAllEvents()
        {
            if (Sort_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            if(false)
            using (CCEntities dc = new CCEntities())
            {
                dc.CharityEvents.RemoveRange(dc.CharityEvents.Where(c =>
                c.CharityEmail == (string)Sort_ID).ToList());
                this.Clear();
                dc.SaveChanges();
            }
            else
            {
                JsonDatabase.GetTable<PL.CharityEvent>().RemoveAll(c =>
                c.CharityEmail == (string)Sort_ID);
                this.Clear();
                JsonDatabase.SaveChanges();
            }
        }

        public void AddEvent(CharityEvent evnt)
        {
            if (evnt is null)
                throw new ArgumentNullException(nameof(evnt));
            

            if (Sort_ID == null)
                throw new Exception(Event_LOAD_ERROR);
            if (CharityEvent.Exists(new CCEntities(), evnt.ID));
                throw new Exception("Event ID: " + evnt.ID + " is already registered as an Charity Event");

            PL.CharityEvent cevent = new PL.CharityEvent
            {
                ID = evnt.ID,
                CharityEmail = evnt.CharityEmail,
                EndDate = evnt.EndDate,
                LocationID = evnt.Location.ID,
                Name = evnt.Name,
                Requirements = evnt.Requirements,
                StartDate = evnt.StartDate,
                Description = evnt.Description
            };

            if(false)
            using (CCEntities dc = new CCEntities())
            {
                
                dc.CharityEvents.Add(cevent);
                dc.SaveChanges();
            }
            else
            {
                JsonDatabase.GetTable<PL.CharityEvent>().Add(cevent);
                JsonDatabase.SaveChanges();
            }
            this.Add(evnt, true);
        }

        public void DeleteEvent(Guid eventID)
        {
            if (Sort_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            if(false)
            using (CCEntities dc = new CCEntities())
            {
                PL.CharityEvent cevent = dc.CharityEvents.Where(
                    c => c.CharityEmail == (string)Sort_ID &&
                    c.ID == eventID).FirstOrDefault()
                    ?? throw new Exception("Event : " + eventID + " does not exist");

                this.Remove(new CharityEvent(eventID, true), true);
                dc.CharityEvents.Remove(cevent);
                dc.SaveChanges();
            }
            else
            {
                PL.CharityEvent cevent = JsonDatabase.GetTable<PL.CharityEvent>().Where(
                    c => c.CharityEmail == (string)Sort_ID &&
                    c.ID == eventID).FirstOrDefault()
                    ?? throw new Exception("Event : " + eventID + " does not exist");

                this.Remove(new CharityEvent(eventID, true), true);
                JsonDatabase.GetTable<PL.CharityEvent>().Remove(cevent);
                JsonDatabase.SaveChanges();
            }
        }
        //public void UpdateCategory(Category cat, string description)
        //{
        //    Category cthis = this.Where(c => c.ID == cat.ID).FirstOrDefault();
        //    cthis.Desc = description;
        //    cthis.Update();
        //
        //}
        //public void UpdateCategory(int catID, string description)
        //{
        //    Category cthis = this.Where(c => c.ID == catID).FirstOrDefault();
        //    cthis.Desc = description;
        //    cthis.Update();
        //}

        private static bool MemberExists(CCEntities dc, string member_ID)
        {
            if(false)
            return dc.MemberActions.Where(c => c.MemberEmail == member_ID
            ).FirstOrDefault() != null;
            else
                return JsonDatabase.GetTable<PL.MemberAction>().Where(c => c.MemberEmail == member_ID
            ).FirstOrDefault() != null;
        }


        public new void Clear()
        {
            base.Clear();
            Sort_ID = null;
        }

        private void Add(CharityEvent item, bool overrideMethod = true)
        {
            if (item.ID != null)
                base.Add(item);
        }
        private void Remove(CharityEvent item, bool overrideMethod = true)
        {
            base.Remove(item);
        }
        public new void Add(CharityEvent item)
        {
            if (Sort_ID != null)
                throw new Exception("Currently being used as a preference list. Please use AddPreference instead");
            base.Add(item);
        }
        public new void Remove(CharityEvent item)
        {
            if (Sort_ID != null)
                throw new Exception("Currently being used as a preference list. Please use DeletePreference instead");
            base.Remove(item);
        }

        public static int getCount()
        {
            using (CCEntities dc = new CCEntities())
            {
                return dc.CharityEvents.Count();
            }
        }
    }
}
