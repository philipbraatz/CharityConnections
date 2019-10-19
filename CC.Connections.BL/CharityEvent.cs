using System;
using System.Collections.Generic;
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
        public AbsLocation location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CharityEventStatus { get; set; }
        public string CharityEventRequirements { get; set; }


        public CharityEvent() { }
        public CharityEvent(Contact_Info contact)
        {
            base.ContactInfo_Email = contact.ContactInfo_Email;
            base.ContactInfo_FName = contact.ContactInfo_FName;
            base.contact_ID = contact.Contact_Info_ID;
            base.ContactInfo_LName = contact.ContactInfo_LName;
            base.ContactInfo_Phone = contact.ContactInfo_Phone;
            base.DateOfBirth = (DateTime)contact.DateOfBirth;
        }
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
            return dc.Charity_Event.Where(c => c.CharityEventCharity_ID == eventID).FirstOrDefault() != null;
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
                        Event_ID = (int)dc.Charity_Event.Max(c => c.CharityEventCharity_ID) + 1;//unique id
                    else
                        Event_ID = 0;

                    dc.Charity_Event.Add(new Charity_Event{
                        CharityEvent_ID = this.Event_ID,
                        CharityEventCharity_ID =this.Charity_ID,
                        CharityEventLocation_ID = this.location.ID,
                        CharityEventContactInfo_ID = this.contact_ID,
                        CharityEventStartDate = this.StartDate,
                        CharityEventEndDate = this.EndDate,
                        CharityEventStatus = this.CharityEventStatus,
                        CharityEventRequirements = this.CharityEventRequirements
                    });
                    return dc.SaveChanges();
                }
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

                    PL.Charity_Event entry = dc.Charity_Event.Where(c => c.CharityEventCharity_ID == this.Event_ID).FirstOrDefault();
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

                    PL.Charity_Event entry = dc.Charity_Event.FirstOrDefault(c => c.CharityEventCharity_ID == this.Event_ID);
                    if (entry == null)
                        //if (!debug)
                            throw new Exception("Category does not exist ID: " + Event_ID);
                        //else
                        //{
                        //    Debug.WriteLine("Category does not exist ID: " + CharityEvent_ID);
                        //    entry = new PL.Charity_Event { CharityEventCharity_ID = CharityEvent_ID, Category_Desc = "DEBUG: not found" };
                        //}

                    //Category_Desc = entry.Category_Desc;
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
                        dc.Charity_Event.ToList().ForEach(c => this.Add(new BL.CharityEvent
                        {
                            Event_ID = c.CharityEvent_ID,
                            Charity_ID = (int)c.CharityEventCharity_ID,
                            contact_ID = (int)c.CharityEventContactInfo_ID
                        }));
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
                             }));
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

        public void AddEvent(CharityEvent evnt, bool debug = false)
        {
            if (charity_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                if (!CharityEvent.Exists(dc, evnt.Event_ID))
                    throw new Exception("Event ID: " + evnt.Event_ID + " does not exist");

                int charEventID = 0;
                if (dc.Charity_Event.ToList().Count != 0)
                    charEventID = dc.Charity_Event.Max(c => c.CharityEvent_ID) + 1;

                dc.Charity_Event.Add(new Charity_Event {
                    CharityEventCharity_ID = evnt.Charity_ID,
                    CharityEventContactInfo_ID =evnt.contact_ID,
                    CharityEventEndDate =evnt.EndDate,
                    CharityEventLocation_ID =evnt.location.ID,
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

        public void DeleteEvent(int actionID)
        {
            if (charity_ID == null)
                throw new Exception(Event_LOAD_ERROR);

            using (fvtcEntities1 dc = new fvtcEntities1())
            {
                //if (!AbsHelping_Action.Exists(dc, actionID))
                //    throw new Exception("Helping Action ID: " + actionID + " does not exist");

                dc.Member_Action.Remove(dc.Member_Action.Where(
                    c => c.MemberActionMember_ID == charity_ID &&
                    c.MemberActionAction_ID == actionID).FirstOrDefault());
                dc.SaveChanges();
                this.Remove(new CharityEvent(actionID), true);
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
