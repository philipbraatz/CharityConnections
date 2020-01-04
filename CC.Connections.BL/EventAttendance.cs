using CC.Abstract;
using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Connections.BL
{
    public enum Status
    {
        NOT_GOING = 0,
        INTERESTED = 1,
        GOING = 2
    }

    public class AbsEventAtendee : ColumnEntry<PL.Event_Attendance>
    {
        //Parameters

        //id
        public Guid ID
        {
            get { return (Guid)base.ID; }
            set { base.ID = value; }
        }

        //other parameters from PL.Categories
        public Guid Event_ID
        {
            get { return (Guid)base.getProperty("Event_ID"); }
            set { setProperty("Event_ID", value); }
        }
        public string Member_ID
        {
            get { return (string)base.getProperty("Member_ID"); }
            set { setProperty("Member_ID", value); }
        }

        [DisplayName("Status")]
        //must be the same name as the PL class
        public Status Status
        {
            get { return (Status)base.getProperty("Status"); }
            set { setProperty("Status", (value),true); }
        }

        public AbsEventAtendee() :
            base()
        { }
        public AbsEventAtendee(Guid event_id, string email) :
            base()
        {

            this.Event_ID = event_id;
            this.Member_ID = email;
            TryFindMatching();
        }
        public AbsEventAtendee(PL.Event_Attendance entry) :
            base(entry)
        {
            Volunteer v = new Volunteer();
            v.LoadId(entry.Volunteer_Email);
        }
        public AbsEventAtendee(int id) :
            base(new CCEntities().Event_Attendance, id)
        {
            Volunteer v = new Volunteer();
            v.LoadId(this.Member_ID);
        }
        //turns a PL class into a BL equivelent example: 
        //Category pl = new Category();
        //...
        //AbsCategory bl = (AbsCategory)pl
        public static implicit operator AbsEventAtendee(PL.Event_Attendance entry)
        { return new AbsEventAtendee(entry); }

        public bool Exists()
        {
            using (CCEntities dc = new CCEntities())
            {
                return dc.Event_Attendance.Where(c => c.Event_ID == this.Event_ID && c.Volunteer_Email == this.Member_ID).FirstOrDefault() != null;
            }
        }
        public void LoadId()
        {
            using (CCEntities dc = new CCEntities())
            {
                base.LoadId(dc.Event_Attendance);
            }
        }
        public bool TryFindMatching()
        {
            using (CCEntities dc = new CCEntities())
            {
                List<Event_Attendance> debugList = dc.Event_Attendance.ToList();
                Event_Attendance eat = dc.Event_Attendance.Where(c => c.Event_ID == this.Event_ID && c.Volunteer_Email == this.Member_ID).FirstOrDefault();
                if (eat == null)
                {
                    this.ID = Guid.NewGuid();
                    this.Status = Status.NOT_GOING;
                    return false;
                }

                this.ID = eat.ID;
                this.Member_ID = (string)eat.Volunteer_Email;
                this.Event_ID = (Guid)eat.Event_ID;
                this.Status = (Status)eat.Volunteer_Status;
                return true;
            }
        }

        public void LoadId(Guid id)
        {
            ID = id;
            LoadId();
        }
        public int Insert()
        {
            using (CCEntities dc = new CCEntities())
            {
                return base.Insert(dc, dc.Event_Attendance);
            }
        }
        public int Update(Status status)
        {
            this.Status = status;
            using (CCEntities dc = new CCEntities())
            {
                return base.Update(dc, dc.Event_Attendance);
            }
        }
        public int Update()
        {
            using (CCEntities dc = new CCEntities())
            {
                return base.Update(dc, dc.Event_Attendance);
            }
        }
        public int Delete()
        {
            using (CCEntities dc = new CCEntities())
            {
                //dc.Categories.Remove(this);
                //return dc.SaveChanges();
                return base.Delete(dc, dc.Event_Attendance);
            }
        }

        public PL.Event_Attendance GetPL()
        {
            return this.GetPL();
        }
    }

    public class EventAttendanceList : AbsList<AbsEventAtendee, Event_Attendance>
    {
        public new void LoadAll()
        {
            using (CCEntities dc = new CCEntities())
            {
                //base.LoadAll(dc.Categories);
                foreach (var c in dc.Event_Attendance.ToList())
                    base.Add(new AbsEventAtendee(c));
            }
        }
    }
    public class EventAttendanceJointList : AbsListJoin<AbsEventAtendee, Event_Attendance, CharityEvent>
    {
        private static Random rand = new Random();
        private int myRandG, myRandI;
        Guid eventID
        {
            get { return (Guid)joinGrouping_ID; }
            set { joinGrouping_ID = value; }
        }
        [DisplayName("Going: ")]
        public int CountGoing { get => this.Where(c => c.Status == Status.GOING).Count() ; }
        [DisplayName("Interested: ")]
        public int CountInterested { get => this.Where(c => c.Status == Status.INTERESTED).Count(); }
        public EventAttendanceJointList(Guid event_id)
            : base("MemberCat_Category_ID", event_id, "MemberCat_Member_ID")
        {
            Charity c = new Charity(new CharityEvent(eventID,true).Charity_Email,true);
            base.joinGrouping_ID = c.Email;
            LoadByEvent();

            myRandG = rand.Next(5, 30);
            myRandI = rand.Next(3, 15);
        }

        public void LoadByEvent()
        { LoadByEvent((Guid)base.joinGrouping_ID); }
        public void LoadByEvent(Guid event_id)
        {
            using (CCEntities dc = new CCEntities())
            {
                eventID = event_id;
                if (dc.Event_Attendance.ToList().Count != 0)
                {
                    List<Charity_Event> debugList = dc.Charity_Event.Where(c => c.ID == eventID).ToList();
                    dc.Charity_Event
                        .Where(c => c.ID == eventID).ToList()
                        .ForEach(b =>
                        {
                            dc.Event_Attendance
                                .Where(d =>
                                   d.Event_ID == b.ID
                                ).ToList().ForEach(eat => 
                                    base.Add(new AbsEventAtendee(eat)));
                        });
                }
                        
            }
        }
        public void DeleteAttendance()
        {
            using (CCEntities dc = new CCEntities())
            {
                foreach (var item in dc.Event_Attendance)
                    if (item.Event_ID == (Guid)base.joinGrouping_ID)
                        dc.Event_Attendance.Remove(item);
                dc.SaveChanges();
                this.Clear();
            }
        }
        public void Add(string attendee)
        {
            using (CCEntities dc = new CCEntities())
            {
                Guid newID = Guid.NewGuid();

                AbsEventAtendee evntAtt = new AbsEventAtendee((Guid)base.joinGrouping_ID, attendee);
                dc.Event_Attendance.Add(evntAtt.GetPL());
                this.Add(evntAtt);

            }
        }
        public new void Remove(string email)
        {
            using (CCEntities dc = new CCEntities())
            {
                foreach (var item in dc.Event_Attendance)
                    if (item.Volunteer_Email == email)
                    {
                        dc.Event_Attendance.Remove(item);
                        break;
                    }
                dc.SaveChanges();
            }
        }
    }
}
