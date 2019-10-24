using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    public class CharityEvent : AbsContactInfo
    {
        private static fvtcEntities1 dc;

        public int Event_ID { get; set; }
        public int Charity_ID { get; set; }//only the id 
        public string CharityEventName { get; set; }
        public AbsLocation Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CharityEventStatus { get; set; }
        public string CharityEventRequirements { get; set; }


        public CharityEvent() { }
        public CharityEvent(Contact_Info contact) => setContactInfo(contact);
        public CharityEvent(int charity_event_ID)
        {
            this.Event_ID = charity_event_ID;
            LoadId();
        }

        public static implicit operator CharityEvent(PL.Charity_Event entry)
        { return new CharityEvent(entry.CharityEvent_ID); }

        //public static implicit operator CharityEvent(PL.Charity entry)
        //{ return new CharityEvent(entry.Charity_Contact_ID); }
        public static bool Exists(fvtcEntities1 dc, int eventID)
        {
            return dc.Charity_Event.Where(c => c.CharityEvent_ID == eventID).FirstOrDefault() != null;
        }

        protected void setContactInfo(Contact_Info contact)
        {
            base.ContactInfo_Email = contact.ContactInfo_Email;
            base.ContactInfo_FName = contact.ContactInfo_FName;
            base.contact_ID = contact.Contact_Info_ID;
            base.ContactInfo_LName = contact.ContactInfo_LName;
            base.ContactInfo_Phone = contact.ContactInfo_Phone;
            base.DateOfBirth = (DateTime)contact.DateOfBirth;
        }
        protected void setEventInfo(PL.Charity_Event char_event)
        {
            this.Event_ID = char_event.CharityEvent_ID;
            this.CharityEventName = char_event.CharityEventName;
            this.CharityEventRequirements = char_event.CharityEventRequirements;
            this.CharityEventStatus = char_event.CharityEventStatus;
            this.Charity_ID = (int)char_event.CharityEventCharity_ID;
            this.StartDate = (DateTime)char_event.CharityEventStartDate;
            this.EndDate = (DateTime)char_event.CharityEventEndDate;
            if (char_event.CharityEventLocation_ID == null)
                throw new Exception("Charity Event ID "+ this.Event_ID+" doesnt not have a location set");
            this.Location = new AbsLocation((int)char_event.CharityEventLocation_ID);
        }

        public new int Insert()
        {
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

                    dc.Charity_Event.Add(new Charity_Event{
                        CharityEvent_ID = this.Event_ID,
                        CharityEventCharity_ID =this.Charity_ID,
                        CharityEventLocation_ID = this.Location.ID,
                        CharityEventContactInfo_ID = this.contact_ID,
                        CharityEventStartDate = this.StartDate,
                        CharityEventEndDate = this.EndDate,
                        CharityEventStatus = this.CharityEventStatus,
                        CharityEventRequirements = this.CharityEventRequirements,
                        CharityEventName =this.CharityEventName
                    });
                    return dc.SaveChanges();
                }
            }
            catch (DbEntityValidationException e)
            {
                //throw e;
                throw new Exception( e.EntityValidationErrors.FirstOrDefault().ValidationErrors.FirstOrDefault().ErrorMessage);
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
                    //    throw new Exception("ID is invaild");

                    dc.Charity_Event.Remove(dc.Charity_Event.Where(c=> c.CharityEvent_ID == this.Event_ID).FirstOrDefault());
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
                    //    throw new Exception("ID is invaild");

                    PL.Charity_Event entry = dc.Charity_Event.Where(c => c.CharityEvent_ID == this.Event_ID).FirstOrDefault() 
                        ?? throw new Exception("Could not find Charity Event with ID: "+ this.Event_ID);
                    entry.CharityEventEndDate = EndDate;
                    entry.CharityEventName = CharityEventName;
                    entry.CharityEventRequirements = CharityEventRequirements;
                    entry.CharityEventStartDate = StartDate;
                    entry.CharityEventStatus = CharityEventStatus;

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
                    //    throw new Exception("ID is invaild");

                    PL.Charity_Event entry = dc.Charity_Event.FirstOrDefault(c => c.CharityEvent_ID == this.Event_ID) 
                        ?? throw new Exception("Event does not exist ID: " + Event_ID);
                    if (entry.CharityEventContactInfo_ID == null)
                        throw new Exception("Event does not have a Contact Info");
                    PL.Contact_Info contact = dc.Contact_Info.Where(c => c.Contact_Info_ID == entry.CharityEventContactInfo_ID).FirstOrDefault()
                        ?? throw new Exception("Event Contact Info could not be found with ID: "+entry.CharityEventContactInfo_ID);
                    setContactInfo(contact);
                    setEventInfo(entry);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }

    public class CharityEventList
        : List<CharityEvent>
    {
        //only used for Event lists
        int? charity_ID { get; set; }

        private const string Event_LOAD_ERROR = "Events not loaded, please loadEvents with a Charity ID";
        public void LoadList()
        {
            try
            {
                this.Clear();
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    if (dc.Charity_Event.ToList().Count != 0)
                        dc.Charity_Event.ToList().ForEach(c => this.Add(c));
                }
            }
            catch (Exception e) { throw e; }
        }

        public CharityEventList LoadEvents(int charityID, bool debug = false)
        {
            try
            {
                this.Clear();
                charity_ID = (int?)charityID;
                using (fvtcEntities1 dc = new fvtcEntities1())
                {
                    //if (!MemberExists(dc, memberID))
                    //    throw new Exception("Member ID: "+ memberID + " does not have any Actions");

                    if (dc.Charity_Event.ToList().Count != 0)
                        dc.Charity_Event.Where(d => d.CharityEventCharity_ID == charityID).ToList().ForEach(c =>
                             this.Add(new CharityEvent {
                                 Event_ID =c.CharityEvent_ID,
                                 Charity_ID = (int)c.CharityEventCharity_ID,
                                 contact_ID = (int)c.CharityEventContactInfo_ID,
                                 StartDate = (DateTime)c.CharityEventStartDate,
                                 EndDate = (DateTime)c.CharityEventEndDate,
                                 CharityEventStatus = c.CharityEventStatus,
                                 CharityEventRequirements = c.CharityEventRequirements
                             },true));
                }
                return this;
            }
            catch (Exception e) { throw e; }
        }

        internal void DeleteAllEvents()
        {
            if (charity_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                dc.Charity_Event.RemoveRange(dc.Charity_Event.Where(c =>
                c.CharityEventCharity_ID == charity_ID).ToList());
                this.Clear();
                dc.SaveChanges();
            }
        }

        public void AddEvent(CharityEvent evnt)
        {
            if (charity_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                if (CharityEvent.Exists(dc, evnt.Event_ID))
                    throw new Exception("Event ID: " + evnt.Event_ID + " is already registered as an Charity Event");

                evnt.Event_ID = 0;
                if (dc.Charity_Event.ToList().Count != 0)
                    evnt.Event_ID = dc.Charity_Event.Max(c => c.CharityEvent_ID) + 1;

                dc.Charity_Event.Add(new Charity_Event {
                    CharityEventCharity_ID = evnt.Charity_ID,
                    CharityEventContactInfo_ID =evnt.contact_ID,
                    CharityEventEndDate =evnt.EndDate,
                    CharityEventLocation_ID =evnt.Location.ID,
                    CharityEventName = evnt.CharityEventName,
                    CharityEventRequirements =evnt.CharityEventRequirements,
                    CharityEventStartDate =evnt.StartDate,
                    CharityEventStatus =evnt.CharityEventStatus,
                    CharityEvent_ID =evnt.Event_ID
                });
                dc.SaveChanges();
                this.Add(evnt, true);
            }
        }

        public void DeleteEvent(int eventID)
        {
            if (charity_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            using (fvtcEntities1 dc = new fvtcEntities1())
            {

                PL.Charity_Event cevent =dc.Charity_Event.Where(
                    c => c.CharityEventCharity_ID == charity_ID &&
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
            charity_ID = null;
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
            if (charity_ID != null)
                throw new Exception("Currently being used as a prefrence list. Please use AddPrefrence instead");
            base.Add(item);
        }
        public new void Remove(CharityEvent item)
        {
            if (charity_ID != null)
                throw new Exception("Currently being used as a prefrence list. Please use DeletePrefrence instead");
            base.Remove(item);
        }
    }
}
