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
        private int getID_fromDesc(string desc,bool isNull =false)
        {
            CharityList allTable = new CharityList();
            allTable.LoadAll();

            Charity cat = allTable.Where(c => c.Charity_Email.Equals(desc)).FirstOrDefault();
            if (cat == null)
            {
                Charity cat2 = allTable.Where(c => c.Charity_Email.Equals(desc)).FirstOrDefault();
                if (cat2 == null)
                {
                    if (!isNull)
                        Assert.Fail("Could not find email \"" + desc + "\" in Charity table");
                    return -12345;
                }
                else
                    return cat2.ID;
            }
            else
                return cat.ID;
        }
        
        [TestMethod]
        public void Insert()
        {
            Charity newt = new Charity()
            {
                Charity_Name = VALUE1,//Name of charity
                Charity_EIN = VALUE1,
                Charity_Deductibility =true,
                Charity_URL = testingSite,
                Charity_Cause = VALUE1,
                Category = new AbsCategory(CATEGORY_ID),
                Location = new AbsLocation(LOCATION_ID),
                Charity_Requirements = VALUE1,
                Charity_Email = testing_ID1
            };
            newt.Insert(new Password(VALUE1,VALUE2,MemberType.CHARITY,false));
        }

        [TestMethod]
        public void LoadAll()
        {
            CharityList table = new CharityList();
            table.LoadAll();

            Assert.AreNotEqual(0, table.Count);

            Assert.AreEqual(testing_ID1, table.Find(f => f.Charity_Name == VALUE1).Charity_Email);
        }
        [TestMethod]
        public void Load()
        {
            test = new Charity(getID_fromDesc(testing_ID2));

            Assert.IsTrue(test.Location.ContactInfoCity== new AbsLocation(LOCATION_ID).ContactInfoCity);
            Assert.IsTrue(test.Category.ID == CATEGORY_ID);

            Assert.IsTrue(test.Charity_EIN == VALUE1);
            Assert.IsTrue(test.Charity_Cause == VALUE1);
            Assert.IsTrue(test.Charity_Requirements == VALUE1);
            Assert.IsTrue(test.Charity_Email == testing_ID1);
            Assert.IsTrue(test.Charity_Deductibility == true);
            Assert.IsTrue(test.Charity_URL== testingSite);
            //Assert.AreNotEqual(0, test.Prefered_Charity_ID_List.Count);
            Assert.IsTrue(test.Location.ContactInfoCity == "Appleton");
        }
        [TestMethod]
        public void Update()
        {
            test = new Charity(getID_fromDesc( testing_ID2));
            Charity updated = new Charity(getID_fromDesc(testing_ID2));

            updated.Charity_EIN = VALUE2;
            updated.Charity_Deductibility = false;
            updated.Charity_Cause = VALUE2;
            updated.Charity_Requirements = VALUE2;
           
            //updated.Member_Type.Desc = VALUE2;
            updated.Location.ContactInfoCity= VALUE2;

            updated = null;//clear
            updated = new Charity(getID_fromDesc(testing_ID2));//load again

            Assert.IsTrue(updated.Charity_EIN == VALUE2);
            //Assert.IsTrue(updated.Member_Type.Desc == VALUE2);
            Assert.IsFalse(updated.Charity_Deductibility);
            Assert.IsTrue(updated.Charity_Cause == VALUE2);
            Assert.IsTrue(updated.Charity_Requirements== VALUE2);
        }
        [TestMethod]
        public void Delete()
        {
            test = new Charity(getID_fromDesc(testing_ID2));
            test.Delete(new Password(test.Charity_Email,true));//delete

            CharityList table = new CharityList();
            table.LoadAll();//load updated table

            Assert.AreEqual(-12345,getID_fromDesc(testing_ID2,true));
            //TODO check that users preferences are gone
        }
    }
}
