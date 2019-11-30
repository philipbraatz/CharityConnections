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

namespace CC.Connections.BL
{
    public class CharityEvent
    {
        private static fvtcEntities1 dc;

        public int Event_ID { get; set; }
        public int Charity_ID { get; set; }//only the id 
        [DisplayName("Charity Event")]
        public string CharityEventName { get; set; }
        [DisplayName("Location")]
        public AbsLocation Location { get; set; }

        public EventAttendanceList atendees { get; set; }

        //TODO make PRIVATE

        public DateTime _start { get; set; }
        public DateTime _end { get; set; }

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
        public Charity charity { get; set; }
        public AbsEventAtendee Member_Attendance { get; set; }


        public void AddMember(string email, Status status)
        {
            AbsEventAtendee atendee = new AbsEventAtendee(this.Event_ID, email);
            atendee.Status = status;
            atendee.Insert();
            this.atendees.Add(atendee);
        }
        public void RemoveMember(string email)
        {
            this.atendees.Remove(email);
        }

        [DisplayName("Requirements")]
        public string CharityEventRequirements { get; set; }
        [DisplayName("Description")]
        public string Description { get; set; }

        //TODO refactor
        public CharityEvent()
        {
            Clear();
        }
        public CharityEvent(int charity_event_ID)
        {
            this.Event_ID = charity_event_ID;
            Clear();
            LoadId();
            this.charity = new Charity(this.Charity_ID);
        }
        public CharityEvent(Charity charity, int charity_event_ID)
        {
            this.charity = charity;
            this.Event_ID = charity_event_ID;
            Clear();
            LoadId();
        }
        public CharityEvent(Charity charity, int charity_event_ID, string member_email)
        {
            this.charity = charity;
            this.Member_Attendance = new AbsEventAtendee(charity_event_ID, member_email);
            this.Event_ID = charity_event_ID;
            Clear();
            LoadId();
        }

        public CharityEvent(Charity_Event c)
        {
            this.setEventInfo(c);
            this.charity = new Charity((int)c.CharityEventCharity_ID);
        }

        public static implicit operator CharityEvent(PL.Charity_Event entry)
        { return new CharityEvent(entry.CharityEvent_ID); }

        //public static implicit operator CharityEvent(PL.Charity entry)
        //{ return new CharityEvent(entry.Charity_Contact_ID); }
        public static bool Exists(fvtcEntities1 dc, int eventID)
        {
            return dc.Charity_Event.Where(c => c.CharityEvent_ID == eventID).FirstOrDefault() != null;
        }

        private void Clear()
        {
            this.Location = new Location();
            this.atendees = new EventAttendanceList(this.Event_ID);
        }

        protected void setEventInfo(PL.Charity_Event char_event)
        {
            this.Event_ID = char_event.CharityEvent_ID;
            this.CharityEventName = char_event.CharityEventName;
            this.CharityEventRequirements = char_event.CharityEventRequirements;
            this.Charity_ID = (int)char_event.CharityEventCharity_ID;
            this._start = (DateTime)char_event.CharityEventStartDate;
            this._end = (DateTime)char_event.CharityEventEndDate;
            if (char_event.CharityEventLocation_ID == null)
                throw new Exception("Charity Event ID " + this.Event_ID + " doesn't not have a location set");
            this.Location = new AbsLocation((int)char_event.CharityEventLocation_ID);
            this.Description = char_event.CharityEventDescription;
            this.atendees = new EventAttendanceList(char_event.CharityEvent_ID);
        }
        protected void setEventInfo(CharityEvent evnt)
        {
            this.Event_ID = evnt.Event_ID;
            this.CharityEventName = evnt.CharityEventName;
            this.CharityEventRequirements = evnt.CharityEventRequirements;
            this.Charity_ID = evnt.Charity_ID;
            this._start = evnt._start;
            this._end = evnt._end;
            this.Location = evnt.Location;
            this.Description = evnt.Description;
            this.atendees = new EventAttendanceList(evnt.Event_ID);
        }

        public new int Insert()
        {
            this.Location.Insert();

            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    if (dc.Charity_Event.ToList().Count > 0)
                        Event_ID = (int)dc.Charity_Event.Max(c => c.CharityEvent_ID) + 1;//unique id
                    else
                        Event_ID = 0;

                    dc.Charity_Event.Add(new Charity_Event
                    {
                        CharityEvent_ID = this.Event_ID,
                        CharityEventCharity_ID = this.Charity_ID,
                        CharityEventLocation_ID = this.Location.ID,
                        CharityEventStartDate = this._start,
                        CharityEventEndDate = this._end,
                        CharityEventRequirements = this.CharityEventRequirements,
                        CharityEventName = this.CharityEventName,
                        CharityEventDescription = this.Description
                    });
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
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");

                    dc.Charity_Event.Remove(dc.Charity_Event.Where(c => c.CharityEvent_ID == this.Event_ID).FirstOrDefault());
                    Location.Delete();
                    atendees.DeleteAttendance();
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
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");

                    PL.Charity_Event entry = dc.Charity_Event.Where(c => c.CharityEvent_ID == this.Event_ID).FirstOrDefault()
                        ?? throw new Exception("Could not find Charity Event with ID: " + this.Event_ID);
                    entry.CharityEventEndDate = _end;
                    entry.CharityEventStartDate = _start;
                    entry.CharityEventName = CharityEventName;
                    entry.CharityEventRequirements = CharityEventRequirements;
                    entry.CharityEventStartDate = StartDate;
                    entry.CharityEventDescription = Description;

                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
        public new void LoadId()
        {
            try
            {
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    //if (this.ID == Guid.Empty)
                    //    throw new Exception("ID is invalid");

                    PL.Charity_Event entry = dc.Charity_Event.FirstOrDefault(c => c.CharityEvent_ID == this.Event_ID)
                        ?? throw new Exception("Event does not exist ID: " + Event_ID);
                    //if (entry.CharityEventContactInfo_ID == null)
                    //    throw new Exception("Event does not have a Contact Info");
                    setEventInfo(entry);//LOADS
                    atendees.LoadByEvent(entry.CharityEvent_ID);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public bool IsGoing(String email)
        {
            return atendees.Where(c => c.email == email).FirstOrDefault().Status == Status.GOING;
        }
        public bool IsInterested(String email)
        {
            return atendees.Where(c => c.email == email).FirstOrDefault().Status == Status.INTERESTED;
        }
    }

    public class CharityEventList
        : List<CharityEvent>
    {
        //only used for Event lists
        private int? Sort_ID { get; set; }
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
            try
            {
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    dc.Charity_Event.ToList().ForEach(c => this.Add(c, true));
                }
            }
            catch (Exception) { throw; }
        }

        public void LoadWithFilter(int id, SortBy sort)
        {
            //throw new NotImplementedException();
            this.LoadAll();
            Filterer filter = new Filterer();
            filter.FillFilter(this);
            switch (sort)
            {
                case SortBy.CATEGORY:
                    filter.Whitelist_Remaining_Events(new List<int> { id }, new List<int>(), new List<int>());
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

            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                dc.Charity_Event.RemoveRange(dc.Charity_Event.Where(c =>
                c.CharityEventCharity_ID == Sort_ID).ToList());
                this.Clear();
                dc.SaveChanges();
            }
        }

        public void AddEvent(CharityEvent evnt)
        {
            if (Sort_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                if (CharityEvent.Exists(dc, evnt.Event_ID))
                    throw new Exception("Event ID: " + evnt.Event_ID + " is already registered as an Charity Event");

                evnt.Event_ID = 0;
                if (dc.Charity_Event.ToList().Count != 0)
                    evnt.Event_ID = dc.Charity_Event.Max(c => c.CharityEvent_ID) + 1;

                dc.Charity_Event.Add(new Charity_Event
                {
                    CharityEventCharity_ID = evnt.Charity_ID,
                    CharityEventEndDate = evnt.EndDate,
                    CharityEventLocation_ID = evnt.Location.ID,
                    CharityEventName = evnt.CharityEventName,
                    CharityEventRequirements = evnt.CharityEventRequirements,
                    CharityEventStartDate = evnt.StartDate,
                    CharityEvent_ID = evnt.Event_ID,
                    CharityEventDescription = evnt.Description
                });
                dc.SaveChanges();
                this.Add(evnt, true);
            }
        }

        public void DeleteEvent(int eventID)
        {
            if (Sort_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            using (fvtcEntities1 dc = new fvtcEntities1())
            {

                PL.Charity_Event cevent = dc.Charity_Event.Where(
                    c => c.CharityEventCharity_ID == Sort_ID &&
                    c.CharityEvent_ID == eventID).FirstOrDefault()
                    ?? throw new Exception("Event : " + eventID + " does not exist");

                this.Remove(new CharityEvent(eventID), true);
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

        private bool MemberExists(fvtcEntities1 dc, int? member_ID)
        {
            return dc.Member_Action.Where(c => c.MemberActionMember_ID == member_ID
            ).FirstOrDefault() != null;
        }

        public new void Clear()
        {
            base.Clear();
            Sort_ID = null;
        }

        private void Add(CharityEvent item, bool overrideMethod = true)
        {
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
            using (fvtcEntities1 dc = new fvtcEntities1())
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
