using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC.Connections.BL;

namespace CC.Connections.BL.Test
{
    //Tests Members contact info, location, and password fully
    [TestClass]
    public class Charity_UT
    {

        public Charity test;
        public string testingID = "test@test.com";
        public const string VALUE1 = "test";
        public const string VALUE2 = "updated";
        public const int INT1 = -7;
        public const int INT2 = -22;

        public const int CATEGORY_ID = 0;
        public const int LOCATION_ID = 0;
        public const int ACTION1 = 0;
        public const int ACTION2 = 1;
        public const int GUEST_ID = 3;


        [TestMethod]
        public void Insert()
        {

            Charity newt = new Charity()
            {
                Cause = VALUE1,
                Ch_Email = testingID,
                Deductibility = true,
                EIN = VALUE1,
                LocationID = LOCATION_ID,
                Requirements = VALUE1,
                URL ="www."+ testingID
            };
            newt.LoadId(testingID);//load contact
            newt.Insert();

            //CharityList loadChar = new CharityList();
            //loadChar.load();
            //newt.Prefered_Charity_ID_List.Add(loadChar[0].ID);


            CharityEvent newEvent = new CharityEvent
            {
                ContactInfo_Phone = "1234567",
                ContactInfo_FName = "",
                ContactInfo_LName = "",
                CharityEventName = "",
                CharityEventRequirements = "",
                CharityEventStatus = "",
                Charity_ID = newt.ID,
                DateOfBirth = DateTime.Now,
                EndDate = DateTime.Now,
                StartDate = DateTime.Now,
                //Location = newloc
            };
            newt.charityEvents.Add(newEvent);
        }

        [TestMethod]
        public void LoadAll()
        {
            CharityList table = new CharityList();
            table.LoadList();

            Assert.AreNotEqual(0, table.Count);

            Assert.AreEqual(testingID, table.Find(f => f.ContactInfo_FName == VALUE1).ContactInfo_Email);
        }
        [TestMethod]
        public void Load()
        {
            test = new Charity();

            Assert.IsTrue(test.ContactInfo_Phone == "1234567");
            Assert.IsFalse(test.Password.Pass == VALUE1.Trim());
            Assert.IsTrue(test.Pref.Distance == INT1);

            Assert.IsTrue(test.Prefered_helping_Actions.Count > 0);
            Assert.IsTrue(test.Prefered_Categories.Count > 0);
            Assert.IsTrue(test.Prefered_Categories[0].Category_Desc == VALUE1);
            //Assert.AreNotEqual(0, test.Prefered_Charity_ID_List.Count);
            Assert.IsTrue(test.Location.ContactInfoCity == VALUE1);
        }
        [TestMethod]
        public void Update()
        {
            test = new Charity(testingID);
            Charity updated = new Charity(testingID)
            {
                ContactInfo_LName = VALUE2
            };
            //updated.Member_Type.Desc = VALUE2;
            updated.Password.Pass = VALUE2;
            updated.Pref.Distance = INT2;
            updated.Location.ContactInfoCity = VALUE2;
            //updated.Prefered_Charity_ID_List.Clear();

            updated.Update();//update database
            updated = null;//clear
            updated = new Charity(testingID);//load again

            Assert.IsTrue(updated.ContactInfo_LName == VALUE2);
            //Assert.IsTrue(updated.Member_Type.Desc == VALUE2);
            Assert.IsFalse(updated.Password.Pass == test.Password.Pass);
            Assert.IsTrue(updated.Pref.Distance == INT2);
            Assert.IsTrue(updated.Location.ContactInfoCity == VALUE2);
        }
        [TestMethod]
        public void Delete()
        {
            test = new Charity(testingID);
            test.Delete();//delete

            MemberList table = new MemberList();
            table.LoadList();//load updated table

            Assert.IsNull(table.Find(f => f.ContactInfo_FName == VALUE1));
            //TODO check that users prefrences are gone
        }
    }
}
