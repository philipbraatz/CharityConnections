using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC.Connections.BL;

namespace CC.Connections.BL.Test
{
    //Tests Members contact info, location, and password fully
    [TestClass]
    public class Member_UT
    {

        public BLMember test;
        public string testingID = "test@test.com";
        public const string VALUE1 = "test";
        public const string VALUE2 = "updated";
        public const int INT1 = -7;
        public const int INT2 = -22;

        public const int CATEGORY_ID = 0;
        public const int ACTION_ID = 0;
        public const int ACTION1 = 0;
        public const int ACTION2 = 1;
        public const int GUEST_ID = 3;


        [TestMethod]
        public void Insert()
        {
            BLMember newt = new BLMember(testingID, VALUE1, GUEST_ID,false,true)
            {
                ContactInfo_Phone = "1234567",
                ContactInfo_FName = VALUE1,
                ContactInfo_LName = VALUE2
            };
            newt.Member_Type.MemberTypeDescription = VALUE1;
            newt.Pref.Distance = INT1;

            newt.Location.ContactInfoCity = VALUE1;

            newt.Insert();

            //CharityList loadChar = new CharityList();
            //loadChar.load();
            //newt.Prefered_Charity_ID_List.Add(loadChar[0].ID);

            newt.Prefered_helping_Actions.Add(new AbsHelping_Action(ACTION1));
            newt.Prefered_Categories.Add(new AbsCategory(CATEGORY_ID));
            newt.Prefered_helping_Actions.Add(new AbsHelping_Action(ACTION_ID));
        }

        [TestMethod]
        public void LoadAll()
        {
            MemberList table = new MemberList();
           table.LoadList();

            Assert.AreNotEqual(0, table.Count);

            Assert.AreEqual(testingID, table.Find(f => f.ContactInfo_FName == VALUE1).ContactInfo_Email);
        }
        [TestMethod]
        public void Load()
        {
            test = new BLMember(testingID);

            Assert.IsTrue(test.ContactInfo_Phone == "1234567");
            Assert.IsFalse(test.Password.Pass == VALUE1.Trim());
            Assert.IsTrue(test.Pref.Distance == INT1);

            Assert.IsTrue(test.Prefered_helping_Actions.Count > 0);
            Assert.IsTrue(test.Prefered_Categories.Count >0);
            Assert.IsTrue(test.Prefered_Categories[0].Category_Desc == VALUE1);
            //Assert.AreNotEqual(0, test.Prefered_Charity_ID_List.Count);
            Assert.IsTrue(test.Location.ContactInfoCity == VALUE1);
        }
        [TestMethod]
        public void Update()
        {
            test = new BLMember(testingID);
            BLMember updated = new BLMember(testingID)
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
            updated = new BLMember(testingID);//load again

            Assert.IsTrue(updated.ContactInfo_LName == VALUE2);
            //Assert.IsTrue(updated.Member_Type.Desc == VALUE2);
            Assert.IsFalse(updated.Password.Pass == test.Password.Pass);
            Assert.IsTrue(updated.Pref.Distance == INT2);
            Assert.IsTrue(updated.Location.ContactInfoCity == VALUE2);
        }
        [TestMethod]
        public void Delete()
        {
            test = new BLMember(testingID);
            test.Delete();//delete

            MemberList table = new MemberList();
            table.LoadList();//load updated table

            Assert.IsNull(table.Find(f => f.ContactInfo_FName == VALUE1));
            //TODO check that users prefrences are gone
        }

        [TestMethod]
        public void CreateContact()
        {
            AbsContactInfo c = new AbsContactInfo();
            //AbsContactInfo d = 
        }
    }
}
