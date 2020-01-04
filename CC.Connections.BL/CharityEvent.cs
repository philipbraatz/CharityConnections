using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;
using CC.Connections.BL;
using System.Data.Entity.Core;
using CC.Abstract;

namespace CC.Connections.BL
{
    public class CharityEvent : ColumnEntry<PL.Charity_Event>
    {
        private static CCEntities dc;

        public new Guid ID
        {
            get { return (Guid)base.ID; }
            set { base.ID = value; }
        }
        public String Charity_Email
        {
            get { return (string)base.getProperty(nameof(Charity_Email)); }
            set { setProperty(nameof(Charity_Email), value); }
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
                    loc = new Location((Guid)(base.getProperty(nameof(Location) + "_ID")));
                return loc;
            }
            set
            {
                loc = value;
                if (value != null)
                    base.setProperty(nameof(Location) + "_ID", value.ID);
            }
        }

        public EventAttendanceJointList atendees
        {
            get; set;
        }

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

        //display fields
        private Charity chrty;
        public Charity Charity
        {
            get
            {
                if (chrty == null)
                    chrty = new Charity((string)(base.getProperty(nameof(Charity) + "_Email")),true);
                return chrty;
            }
            set
            {
                chrty = value;
                if (value != null)
                    base.setProperty(nameof(Charity) + "_Email", value.Email);
            }
        }
        public AbsEventAtendee Member_Attendance { get; set; }


        public void AddMember(string email, Status status)
        {
            AbsEventAtendee atendee = new AbsEventAtendee(this.ID, email);
            atendee.Status = status;
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
        public CharityEvent(PL.Charity_Event entry) :
           base(entry)
        {
        }
        public CharityEvent(Guid id, bool preloaded = true) :
           base(new CCEntities().Charity_Event, id, preloaded)
        {
            this.ID = id;
            if (preloaded)
                LoadId(true);
        }
        //public static implicit operator CharityEvent(PL.Charity_Event entry)
        //{ return new CharityEvent(entry,true); }

        public CharityEvent(Guid charity_event_ID, Charity charity = null, string member_email = "")
        {
            this.Charity = charity;
            if (!string.IsNullOrEmpty(member_email))
                this.Member_Attendance = new AbsEventAtendee(charity_event_ID, member_email);
            this.ID = charity_event_ID;
            Clear();
            LoadId();
        }

        //public CharityEvent(Charity_Event c)
        //{
        //    this.setEventInfo(c);
        //    this.charity = new Charity(c.Charity_Email,true);
        //}

        //public static implicit operator CharityEvent(PL.Charity_Event entry)
        //{ return new CharityEvent(entry.ID); }

        //public static implicit operator CharityEvent(PL.Charities entry)
        //{ return new CharityEvent(entry.Charity_Contact_ID); }
        public static bool Exists(CCEntities dc, Guid eventID)
        {
            return dc.Charity_Event.Where(c => c.ID == eventID).FirstOrDefault() != null;
        }

        private void Clear()
        {
            this.Location = new PL.Location();
            this.atendees = new EventAttendanceJointList(this.ID);
        }

        protected void setEventInfo(PL.Charity_Event char_event)
        {
            this.ID = char_event.ID;
            this.Name = char_event.Name;
            this.Requirements = char_event.Requirements;
            this.Charity_Email = char_event.Charity_Email;
            this._start = (DateTime)char_event.StartDate;
            this._end = (DateTime)char_event.EndDate;
            if (char_event.Location_ID == null)
                throw new Exception("Charity Event ID " + this.ID + " doesn't not have a location set");
            this.Location = new Location((Guid)char_event.Location_ID);
            this.Description = char_event.Description;
            this.atendees = new EventAttendanceJointList(char_event.ID);
        }
        protected void setEventInfo(CharityEvent evnt)
        {
            this.ID = evnt.ID;
            this.Name = evnt.Name;
            this.Requirements = evnt.Requirements;
            this.Charity_Email = evnt.Charity_Email;
            this._start = evnt._start;
            this._end = evnt._end;
            this.Location = evnt.Location;
            this.Description = evnt.Description;
            this.atendees = new EventAttendanceJointList(evnt.ID);
        }

        //TODO refactor to use CC.Abstract
        public new int Insert()
        {
            this.Location.Insert();

            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (CCEntities dc = new CCEntities())
                {
                    ID = Guid.NewGuid();

                    dc.Charity_Event.Add(new Charity_Event
                    {
                        ID = this.ID,
                        Charity_Email = this.Charity_Email,
                        Location_ID = this.Location.ID,
                        StartDate = this._start,
                        EndDate = this._end,
                        Requirements = this.Requirements,
                        Name = this.Name,
                        Description = this.Description
                    });
                    CharityEventList.AddToInstance(this);
                    return dc.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                //throw e;
                throw new Exception(e.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage);
            }
            catch (Exception e) { throw e; }
        }
        public new int Delete()
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");

                    dc.Charity_Event.Remove(dc.Charity_Event.Where(c => c.ID == this.ID).FirstOrDefault());
                    Location.Delete();
                    atendees.DeleteAttendance();
                    CharityEventList.RemoveInstance(this);
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public new int Update()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (CCEntities dc = new CCEntities())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");

                    PL.Charity_Event entry = dc.Charity_Event.Where(c => c.ID == this.ID).FirstOrDefault()
                        ?? throw new Exception("Could not find Charity Event with ID: " + this.ID);
                    entry.EndDate = _end;
                    entry.StartDate = _start;
                    entry.Name = Name;
                    entry.Requirements = Requirements;
                    entry.StartDate = StartDate;
                    entry.Description = Description;

                    CharityEventList.RemoveInstance(this);
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public new void LoadId(bool preloaded = true)
        {
            if (preloaded == false)//definitly needs to be taken from database
                try
                {
                    using (CCEntities dc = new CCEntities())
                    {
                        //if (this.ID == Guid.Empty)
                        //    throw new Exception("ID is invalid");

                        PL.Charity_Event entry = dc.Charity_Event.FirstOrDefault(c => c.ID == this.ID)
                            ?? throw new Exception("Event does not exist ID: " + ID);
                        //if (entry.CharityEventContactInfo_ID == null)
                        //    throw new Exception("Event does not have a Contact Info");
                        setEventInfo(entry);//LOADS
                        atendees.LoadByEvent(entry.ID);
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            else
            {
                CharityEvent loadC = CharityEventList.INSTANCE.Where(c => c.ID == this.ID).FirstOrDefault();
                if (loadC != null)//retreive from existing
                    this.setEventInfo(loadC);
                else//load from database
                    LoadId(false);
            }

        }


        public bool IsGoing(String email)
        {
            return atendees.Where(c => c.Member_ID == email).FirstOrDefault().Status == Status.GOING;
        }
        public bool IsInterested(String email)
        {
            return atendees.Where(c => c.Member_ID == email).FirstOrDefault().Status == Status.INTERESTED;
        }
    }

    public class CharityEventList
        : List<CharityEvent>
    {
        public static CharityEventList INSTANCE { get; private set; } = LoadInstance();
        public static CharityEventList LoadInstance()
        {
            try
            {
                INSTANCE = new CharityEventList();
                using (CCEntities dc = new CCEntities())
                {
                    foreach (var c in dc.Charity_Event.ToList())
                        INSTANCE.Add(new CharityEvent(c));
                }
                return INSTANCE;
            }
            catch (EntityException e) { throw e.InnerException; }
        }
        public static void AddToInstance(CharityEvent category)
        {
            INSTANCE.Add(category);
        }
        //Might be able to optimize better
        internal static void UpdateInstance(CharityEvent category)
        {
            RemoveInstance(category);
            AddToInstance(category);
        }

        internal static void RemoveInstance(CharityEvent category)
        {
            INSTANCE.Remove(category);
        }

        //only used for Event lists
        private object Sort_ID { get; set; }
        private SortBy sorter { get; set; }
        private Volunteer userPref { get; set; }

        private const string Event_LOAD_ERROR = "Events not loaded, please loadEvents with a Charity ID";

        public CharityEventList()
        {
            sorter = SortBy.NONE;
        }
        public CharityEventList(int id, SortBy sort, Volunteer user_pref = default)
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
            foreach (var item in INSTANCE)
            {
                this.Add(item, true);
            }
            //try
            //{
            //    using (CCEntities dc = new CCEntities())
            //    {
            //        var test = dc.Charity_Event.ToList();
            //        dc.Charity_Event.ToList().ForEach(c => this.Add(c, true));
            //    }
            //}
            //catch (Exception) { throw; }
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

            using (CCEntities dc = new CCEntities())
            {
                dc.Charity_Event.RemoveRange(dc.Charity_Event.Where(c =>
                c.Charity_Email == Sort_ID).ToList());
                this.Clear();
                dc.SaveChanges();
            }
        }

        public void AddEvent(CharityEvent evnt)
        {
            if (Sort_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            using (CCEntities dc = new CCEntities())
            {
                if (CharityEvent.Exists(dc, evnt.ID))
                    throw new Exception("Event ID: " + evnt.ID + " is already registered as an Charity Event");

                evnt.ID = Guid.NewGuid();

                dc.Charity_Event.Add(new Charity_Event
                {
                    ID = evnt.ID,
                    Charity_Email = evnt.Charity_Email,
                    EndDate = evnt.EndDate,
                    Location_ID = evnt.Location.ID,
                    Name = evnt.Name,
                    Requirements = evnt.Requirements,
                    StartDate = evnt.StartDate,
                    Description = evnt.Description
                });
                dc.SaveChanges();
                this.Add(evnt, true);
            }
        }

        public void DeleteEvent(Guid eventID)
        {
            if (Sort_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            using (CCEntities dc = new CCEntities())
            {

                PL.Charity_Event cevent = dc.Charity_Event.Where(
                    c => c.Charity_Email == (string)Sort_ID &&
                    c.ID == eventID).FirstOrDefault()
                    ?? throw new Exception("Event : " + eventID + " does not exist");

                this.Remove(new CharityEvent(eventID, true), true);
                dc.Charity_Event.Remove(cevent);
                dc.SaveChanges();
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

        private bool MemberExists(CCEntities dc, string member_ID)
        {
            return dc.Member_Action.Where(c => c.Member_Email == member_ID
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
                return dc.Charity_Event.Count();
            }
        }

        public static implicit operator List<object>(CharityEventList v)
        {
            throw new NotImplementedException();
        }
    }
}
