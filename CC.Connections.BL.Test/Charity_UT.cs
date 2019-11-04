using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC.Connections.BL;
using System.Linq;

namespace CC.Connections.BL.Test
{


    [TestClass]
    public class Charity_UT
    {

        public Charity test;
        public string testing_ID1 = "test@charity.com";
        public string testing_ID2 = "help@support.com";
        public string testingSite = "www.test.com";
        public const string VALUE1 = "test";
        public const string VALUE2 = "updated";
        public const int INT1 = -7;
        public const int INT2 = -22;

        public const int CATEGORY_ID = 1;
        public const int LOCATION_ID = 1;
        public const int ACTION1 = 1;
        public const int ACTION2 = 1;
        public const int CONTACT_ID = 1;

        //in the real code you would already have the ID you want
        //so you would not need to find the matching discription
        private int getID_fromDesc(string desc)
        {
            CharityList allTable = new CharityList();
            allTable.LoadList();

            Charity cat = allTable.Where(c => c.ContactInfo_Email.Equals(desc)).FirstOrDefault();
            if (cat == null)
                Assert.Fail("Could not find item with CharityEmail \"" + desc + "\" in Charity table");
            return cat.ID;
        }
        
        [TestMethod]
        public void Insert()
        {
            Charity newt = new Charity()
            {
                ContactInfo_Email = testing_ID1,
                ContactInfo_FName = VALUE1,//Name of charity
                ContactInfo_Phone = VALUE1,
                DateOfBirth = DateTime.Now,//Start of charity orginization
                EIN = VALUE1,
                Deductibility =true,
                URL = testingSite,
                Cause = VALUE1,
                Category = new AbsCategory(CATEGORY_ID),
                Location = new AbsLocation(LOCATION_ID),
                Requirements = VALUE1,
                CharityEmail = testing_ID2,
                Password = new Password(testing_ID1,VALUE1)
            };
            newt.Insert();
        }

        [TestMethod]
        public void LoadAll()
        {
            CharityList table = new CharityList();
            table.LoadList();

            Assert.AreNotEqual(0, table.Count);

            Assert.AreEqual(testing_ID2, table.Find(f => f.ContactInfo_FName == VALUE1).ContactInfo_Email);
        }
        [TestMethod]
        public void Load()
        {
            test = new Charity(getID_fromDesc(testing_ID2));

            Assert.IsTrue(test.ContactInfo_Phone == VALUE1);
            Assert.IsFalse(test.Password.Pass == VALUE1);

            Assert.IsTrue(test.Location.ContactInfoCity== new AbsLocation(LOCATION_ID).ContactInfoCity);
            Assert.IsTrue(test.Category.ID == CATEGORY_ID);

            Assert.IsTrue(test.EIN == VALUE1);
            Assert.IsTrue(test.Cause == VALUE1);
            Assert.IsTrue(test.Requirements == VALUE1);
            Assert.IsTrue(test.ContactInfo_Email == testing_ID2);
            Assert.IsTrue(test.Deductibility == true);
            Assert.IsTrue(test.URL== testingSite);
            //Assert.AreNotEqual(0, test.Prefered_Charity_ID_List.Count);
            Assert.IsTrue(test.Location.ContactInfoCity == "Appleton");
        }
        [TestMethod]
        public void Update()
        {
            test = new Charity(getID_fromDesc( testing_ID2));
            Charity updated = new Charity(getID_fromDesc(testing_ID2));

            updated.EIN = VALUE2;
            updated.Deductibility = false;
            updated.Cause = VALUE2;
            updated.Requirements = VALUE2;
           
            //updated.Member_Type.Desc = VALUE2;
            updated.Password.Pass = VALUE2;
            updated.Location.ContactInfoCity= VALUE2;

            updated.Update();//update database
            updated = null;//clear
            updated = new Charity(getID_fromDesc(testing_ID2));//load again

            Assert.IsTrue(updated.EIN == VALUE2);
            //Assert.IsTrue(updated.Member_Type.Desc == VALUE2);
            Assert.IsFalse(updated.Deductibility);
            Assert.IsTrue(updated.Cause == VALUE2);
            Assert.IsTrue(updated.Requirements== VALUE2);
        }
        [TestMethod]
        public void Delete()
        {
            test = new Charity(getID_fromDesc(testing_ID2));
            test.Delete();//delete

            CharityList table = new CharityList();
            table.LoadList();//load updated table

            Assert.IsNull(table.Find(f => f.CharityEmail == testing_ID2));
            //TODO check that users prefrences are gone
        }
    }
}
