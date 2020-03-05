using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC.Connections.BL;
using System.Linq;

namespace CC.Connections.BL.Test
{
//TODO outdated code, needs to be rewriten
//    //Tests Members contact info, location, and password fully
//    [TestClass]
//    public class Attendance_UT
//    {
//
//        public AbsEventAtendee test;
//        public string testingID = "test@test.com";
//        public const string NAME_OF_EVENT = "title name for charityEvents";
//        public const Status VALUE1 = Status.INTERESTED;
//        public const Status VALUE2 = Status.GOING;
//        public const int INT1 = -7;
//        public const int INT2 = -22;
//
//        public const int CategoryID = 0;
//        public const int ActionID = 0;
//        public const int ACTION1 = 0;
//        public const int ACTION2 = 1;
//        public const int GUEST_ID = 3;
//
//        //in the real code you would already have the ID you want
//        //so you would not need to find the matching discription
//        private Guid getEventID_fromDesc(string desc)
//        {
//            CharityEventList charityEvents = new CharityEventList();
//            charityEvents.LoadAll();
//
//            CharityEvent cat = charityEvents.Where(c => c.CharityEventName == desc).FirstOrDefault();
//            if (cat == null)
//                Assert.Fail("Could not find item with discription \"" + desc + "\" in table");
//            return cat.EventID;
//        }
//
//        private string getID_fromDesc(string desc)
//        {
//            EventAttendanceList eventAttenders = new EventAttendanceList();
//            eventAttenders.LoadAll();
//
//            AbsEventAtendee cat = eventAttenders.Where(c => c.Member_ID == desc).FirstOrDefault();
//            if (cat == null)
//                Assert.Fail("Could not find item with discription \"" + desc + "\" in table");
//            return cat.Member_ID;
//        }
//
//        [TestMethod]
//        public void Insert()
//        {
//            Volunteer v = new Volunteer(testingID);
//            CharityEvent evnt = new CharityEvent(getEventID_fromDesc(NAME_OF_EVENT));
//            test = new AbsEventAtendee(evnt.EventID, testingID)
//            {
//                Status = VALUE1
//            };
//            test.Insert();
//        }
//
//        [TestMethod]
//        public void LoadByEvent()
//        {
//            int eventID = getEventID_fromDesc(NAME_OF_EVENT);
//            int memberID = getID_fromDesc(testingID);
//            EventAttendanceJointList table = new EventAttendanceJointList(eventID);
//
//            Assert.AreNotEqual(0, table.Count);
//        AbsEventAtendee atendee = table.Where(c =>
//                  c.EventID == eventID &&
//                  c.Member_ID == memberID).FirstOrDefault();
//            Assert.IsNotNull(atendee);
//
//            Status result = atendee.Status;
//            Assert.AreEqual(Status.INTERESTED, result);
//        }
//        [TestMethod]
//        public void Load()
//        {
//            AbsEventAtendee eat = new AbsEventAtendee(getEventID_fromDesc(NAME_OF_EVENT),new Volunteer(testingID).ContactInfo_Email);
//            Assert.IsFalse(eat.ID ==-1);//didnt load attendee
//
//            Status result = eat.Status;
//            Assert.IsNotNull(eat.Status);
//            Assert.AreEqual(Status.INTERESTED, result);
//        }
//        [TestMethod]
//        public void Update()
//        {
//            AbsEventAtendee eat = new AbsEventAtendee(getEventID_fromDesc(NAME_OF_EVENT), new Volunteer(testingID).ContactInfo_Email);
//
//            eat.Status = Status.GOING;
//
//            eat.Update();//update database
//            eat = null;//clear
//            eat = new AbsEventAtendee(getEventID_fromDesc(NAME_OF_EVENT), new Volunteer(testingID).ContactInfo_Email);//load again
//
//            Assert.IsTrue(eat.Status == Status.GOING);
//        }
//        [TestMethod]
//        public void Delete()
//        {
//            AbsEventAtendee eat = new AbsEventAtendee(getEventID_fromDesc(NAME_OF_EVENT), new Volunteer(testingID).ContactInfo_Email);
//            Assert.IsFalse(eat.ID == -1);//didnt load attendee
//            eat.Delete();//delete
//
//            //MemberList table = new MemberList();
//            //table.LoadList();//load updated table
//
//            //Cannot load nonexistant item
//            //TODO compare before and after list size
//            //Assert.IsFalse(new AbsEventAtendee(getEventID_fromDesc(NAME_OF_EVENT), new Volunteer(testingID).ContactInfo_Email).ID == -1);
//        }
//    }
}
