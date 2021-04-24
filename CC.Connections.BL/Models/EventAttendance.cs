using Doorfail.DataConnection;
using Doorfail.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doorfail.Connections.BL
{
    public enum Status
    {
        #pragma warning disable CA1707
        NOT_GOING = 0,
        INTERESTED = 1,
        GOING = 2
        #pragma warning restore CA1707
    }

    public class EventAttendee : CrudModel_Json<PL.EventAttendance>
    {
        //Parameters

        //id
        public new Guid ID
        {
            get { return (Guid)base.ID; }
            set { base.ID = value; }
        }

        //other parameters from PL.Categories
        public Guid EventID
        {
            get { return (Guid)base.getProperty(nameof(EventID)); }
            set { setProperty(nameof(EventID), value); }
        }
        public string VolunteerEmail
        {
            get { return (string)base.getProperty(nameof(VolunteerEmail)); }
            set { setProperty(nameof(VolunteerEmail), value); }
        }

        [DisplayName("Status")]
        //must be the same name as the PL class
        public Status VolunteerStatus
        {
            get { return (Status)base.getProperty(nameof(VolunteerStatus)); }
            set { setProperty(nameof(VolunteerStatus), (value),true); }
        }

        public EventAttendee() :
            base()
        { }
        public EventAttendee(Guid EventID, string email) :
            base()
        {

            this.EventID = EventID;
            this.VolunteerEmail = email;
            TryFindMatching();
        }
        public EventAttendee(PL.EventAttendance entry) :
            base(entry)
        {
            //Volunteer v = new Volunteer(entry.VolunteerEmail);
            //v.LoadId(entry.VolunteerEmail);
        }
        public EventAttendee(int id) :
            base(JsonDatabase.GetTable<PL.EventAttendance>(), id)
        {
            //Volunteer v = new Volunteer();
            //v.LoadId(this.Member_ID);
        }
        //turns a PL class into a BL equivelent example: 
        //Category pl = new Category();
        //...
        //AbsCategory bl = (AbsCategory)pl
        public static implicit operator EventAttendee(PL.EventAttendance entry)
        { return new EventAttendee(entry); }

        public bool Exists()
        {
            if(false)
            using (CCEntities dc = new CCEntities())
            {
                return dc.EventAttendances.Where(c => c.EventID == this.EventID && c.VolunteerEmail == this.VolunteerEmail).FirstOrDefault() != null;
            }
            else
                return JsonDatabase.GetTable<PL.EventAttendance>().Where(c => c.EventID == this.EventID && c.VolunteerEmail == this.VolunteerEmail).FirstOrDefault() != null;
        }
    
        public void LoadId()
        {
        if(false)
            using (CCEntities dc = new CCEntities())
            {
                base.LoadId(JsonDatabase.GetTable<PL.EventAttendance>());
            }
        else
            base.LoadId(JsonDatabase.GetTable<PL.EventAttendance>());
        }
        public bool TryFindMatching()
        {
            EventAttendance eat = null;
            List<EventAttendance> debugList = null;
            if (false)
            using (CCEntities dc = new CCEntities())
            {
                debugList = dc.EventAttendances.ToList();
                eat = dc.EventAttendances.Where(c => c.EventID == this.EventID && c.VolunteerEmail == this.VolunteerEmail).FirstOrDefault();
            }
            else
            {
                debugList = JsonDatabase.GetTable<PL.EventAttendance>().ToList();
                eat = JsonDatabase.GetTable<PL.EventAttendance>().Where(c => c.EventID == this.EventID && c.VolunteerEmail == this.VolunteerEmail).FirstOrDefault();
               
            } 
            if (eat == null)
                {
                    this.ID = Guid.NewGuid();
                    this.VolunteerStatus = Status.NOT_GOING;
                    return false;
                }

                this.ID = eat.ID;
                this.VolunteerEmail = (string)eat.VolunteerEmail;
                this.EventID = (Guid)eat.EventID;
                this.VolunteerStatus = (Status)eat.VolunteerStatus;
                return true;
        }

        public void LoadId(Guid id)
        {
            ID = id;
            LoadId();
        }
        public int Insert()
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    return base.Insert(dc, dc.EventAttendances);
                }
            else
                return base.Insert(JsonDatabase.GetTable<PL.EventAttendance>());
        }
        public int Update(Status status)
        {
            this.VolunteerStatus = status;
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    return base.Update(dc, dc.EventAttendances);
                }
            else
                return base.Update(JsonDatabase.GetTable<PL.EventAttendance>());
        }
        public int Update()
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    return base.Update(dc, dc.EventAttendances);
                }
            else
                return base.Update(JsonDatabase.GetTable<PL.EventAttendance>());
        }
        public int Delete()
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    //dc.Categories.Remove(this);
                    //return dc.SaveChanges();
                    return base.Delete(dc, dc.EventAttendances);
                }
            else
                return base.Delete(JsonDatabase.GetTable<PL.EventAttendance>());
        }

        public PL.EventAttendance GetPL()
        {
            return this.GetPL();
        }
    }

    public class EventAttendanceCollection : CrudModelList<EventAttendee, EventAttendance>
    {
        public void LoadAll()
        {
            if(false)
            using (CCEntities dc = new CCEntities())
            {
                //base.LoadAll(dc.Categories);
                foreach (var c in dc.EventAttendances.ToList())
                    base.Add(new EventAttendee(c));
            }
            else
                foreach (var c in JsonDatabase.GetTable<PL.EventAttendance>().ToList())
                    base.Add(new EventAttendee(c));
        }
    }
    public class EventAttendanceJointCollection : AbsListJoin<EventAttendee, EventAttendance, CharityEvent>
    {
        private static Random rand = new Random();
        private int myRandG, myRandI;
        Guid eventID
        {
            get { return (Guid)joinGroupingID; }
            set { joinGroupingID = value; }
        }
        [DisplayName("Going: ")]
        public int CountGoing { get => this.Where(c => c.VolunteerStatus == Status.GOING).Count() ; }
        [DisplayName("Interested: ")]
        public int CountInterested { get => this.Where(c => c.VolunteerStatus == Status.INTERESTED).Count(); }
        public EventAttendanceJointCollection(Guid EventID,bool preload = true)
            : base("MemberCat_CategoryID", EventID, "MemberCat_Member_ID")
        {
            base.joinGroupingID =eventID;
            LoadByEvent();

            myRandG = rand.Next(5, 30);
            myRandI = rand.Next(3, 15);
        }

        public void LoadByEvent()
        { LoadByEvent((Guid)base.joinGroupingID); }
        public void LoadByEvent(Guid EventID)
        {
            eventID = EventID;
            if(false)
            using (CCEntities dc = new CCEntities())
            {
                if (dc.EventAttendances.ToList().Count != 0)
                {
                    List<PL.CharityEvent> debugList = dc.CharityEvents.Where(c => c.ID == eventID).ToList();
                    dc.CharityEvents
                        .Where(c => c.ID == eventID).ToList()
                        .ForEach(b =>
                        {
                            List<EventAttendance> attendances = dc.EventAttendances
                                .Where(d => d.EventID == b.ID).ToList();
                            attendances.ForEach(eat => 
                                    base.Add(new EventAttendee(eat)));
                        });
                }      
            }
            else
            {
                if (JsonDatabase.GetTable<PL.EventAttendance>().ToList().Count != 0)
                {
                    List<PL.CharityEvent> debugList = JsonDatabase.GetTable<PL.CharityEvent>().Where(c => c.ID == eventID).ToList();
                    JsonDatabase.GetTable<PL.CharityEvent>()
                        .Where(c => c.ID == eventID).ToList()
                        .ForEach(b =>
                        {
                            List<EventAttendance> attendances = JsonDatabase.GetTable<PL.EventAttendance>()
                                .Where(d => d.EventID == b.ID).ToList();
                            attendances.ForEach(eat =>
                                    base.Add(new EventAttendee(eat)));
                        });
                }
            }
        }
        public void DeleteAttendance()
        {
            if(false)
            using (CCEntities dc = new CCEntities())
            {
                foreach (var item in dc.EventAttendances)
                    if (item.EventID == (Guid)base.joinGroupingID)
                        dc.EventAttendances.Remove(item);
                dc.SaveChanges();
                this.Clear();
            }
            else
            {
                foreach (var item in JsonDatabase.GetTable<PL.EventAttendance>())
                    if (item.EventID == (Guid)base.joinGroupingID)
                        JsonDatabase.GetTable<PL.EventAttendance>().Remove(item);
                JsonDatabase.SaveChanges();
                this.Clear();
            }
        }
        public void Add(string attendee)
        {
            Guid newID = Guid.NewGuid(); 
            EventAttendee evntAtt = new EventAttendee((Guid)base.joinGroupingID, attendee);

            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    dc.EventAttendances.Add(evntAtt.GetPL());

                }
            else
                JsonDatabase.GetTable<PL.EventAttendance>().Add(evntAtt.GetPL());

            this.Add(evntAtt);
        }
        public void Remove(string email)
        {
            if (false)
                using (CCEntities dc = new CCEntities())
                {
                    foreach (var item in dc.EventAttendances)
                        if (item.VolunteerEmail == email)
                        {
                            dc.EventAttendances.Remove(item);
                            break;
                        }
                    dc.SaveChanges();
                }
            else
            {
                foreach (var item in JsonDatabase.GetTable<PL.EventAttendance>())
                    if (item.VolunteerEmail == email)
                    {
                        JsonDatabase.GetTable<PL.EventAttendance>().Remove(item);
                        break;
                    }
                JsonDatabase.SaveChanges();
            }
        }
    }
}
