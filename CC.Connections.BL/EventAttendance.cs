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
        public new int ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }

        //other parameters from PL.Categories
        public int Event_ID
        {
            get { return (int)base.getProperty("Event_ID"); }
            set { setProperty("Event_ID", value); }
        }
        public int Member_ID
        {
            get { return (int)base.getProperty("Member_ID"); }
            set { setProperty("Member_ID", value); }
        }

        public string email { get; set; }

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
        public AbsEventAtendee(int event_id, string email) :
            base()
        {

            this.Event_ID = event_id;
            this.email = email;
            this.Member_ID = new Volunteer(email).ID;
            //this.Status = Status.NOT_GOING;//default
            TryFindMatching();
        }
        public AbsEventAtendee(PL.Event_Attendance entry) :
            base(entry)
        {
            Volunteer v = new Volunteer();
            v.LoadId(entry.Member_ID);
            this.email = v.ContactInfo_Email;
            //this.Status = Status.NOT_GOING;
        }
        public AbsEventAtendee(int id) :
            base(new CCEntities().Event_Attendance, id)
        {
            Volunteer v = new Volunteer();
            v.LoadId(this.Member_ID);
            this.email = v.ContactInfo_Email;
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
                return dc.Event_Attendance.Where(c => c.Event_ID == this.Event_ID && c.Member_ID == this.Member_ID).FirstOrDefault() != null;
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
                Event_Attendance eat = dc.Event_Attendance.Where(c => c.Event_ID == this.Event_ID && c.Member_ID == this.Member_ID).FirstOrDefault();
                if (eat == null)
                {
                    this.ID = -1;
                    this.Status = Status.NOT_GOING;
                    return false;
                }

                this.ID = eat.EventAttendance_ID;
                this.Member_ID = (int)eat.Member_ID;
                this.Event_ID = (int)eat.Event_ID;
                this.Status = (Status)eat.Status;
                return true;
            }
        }

        public void LoadId(int id)
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

    public class AbsEventAttendanceList : AbsList<AbsEventAtendee, Event_Attendance>
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
    public class EventAttendanceList : AbsListJoin<AbsEventAtendee, Event_Attendance, CharityEvent>
    {
        private static Random rand = new Random();
        private int myRandG, myRandI;
        int eventID
        {
            get { return (int)joinGrouping_ID; }
            set { joinGrouping_ID = value; }
        }
        [DisplayName("Going: ")]
        public int CountGoing { get => this.Where(c => c.Status == Status.GOING).Count() + myRandG; }//TODO remove when more users attend
        [DisplayName("Interested: ")]
        public int CountInterested { get => this.Where(c => c.Status == Status.INTERESTED).Count() + myRandI; }//TODO remove when more users attend

        public EventAttendanceList(int event_id)
            : base("MemberCat_Category_ID", event_id, "MemberCat_Member_ID")
        {
            Charity c = new Charity { ID = event_id };
            base.joinGrouping_ID = c.ID;
            LoadByEvent();

            myRandG = rand.Next(5, 30);
            myRandI = rand.Next(3, 15);
        }

        public void LoadByEvent()
        { LoadByEvent((int)base.joinGrouping_ID); }
        public void LoadByEvent(int event_id)
        {
            using (CCEntities dc = new CCEntities())
            {
                eventID = event_id;
                if (dc.Event_Attendance.ToList().Count != 0)
                {
                    List<Charity_Event> debugList = dc.Charity_Event.Where(c => c.CharityEvent_ID == eventID).ToList();
                    dc.Charity_Event
                        .Where(c => c.CharityEvent_ID == eventID).ToList()
                        .ForEach(b =>
                        {
                            dc.Event_Attendance
                                .Where(d =>
                                   d.Event_ID == b.CharityEvent_ID
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
                    if (item.Event_ID == (int?)base.joinGrouping_ID)
                        dc.Event_Attendance.Remove(item);
                dc.SaveChanges();
                this.Clear();
            }
        }
        public void Add(string attendee)
        {
            using (CCEntities dc = new CCEntities())
            {
                int newID = 0;
                if (dc.Event_Attendance.ToList().Count != 0)
                    newID = dc.Event_Attendance.Max(c => c.EventAttendance_ID) + 1;

                AbsEventAtendee evntAtt = new AbsEventAtendee((int)base.joinGrouping_ID, attendee);
                dc.Event_Attendance.Add(evntAtt.GetPL());
                this.Add(evntAtt);

            }
        }
        public new void Remove(string category)
        {
            using (CCEntities dc = new CCEntities())
            {
                foreach (var item in dc.Event_Attendance)
                    if (item.Event_ID == (int?)base.joinGrouping_ID)
                    {
                        dc.Event_Attendance.Remove(item);
                    }
                dc.SaveChanges();
            }
        }
        public new void RemoveAttendee(string category)
        {
            using (CCEntities dc = new CCEntities())
            {
                foreach (var item in dc.Event_Attendance)
                    if (item.Event_ID == (int?)base.joinGrouping_ID)
                    {
                        dc.Event_Attendance.Remove(item);
                    }
                dc.SaveChanges();
            }
        }
    }
}
