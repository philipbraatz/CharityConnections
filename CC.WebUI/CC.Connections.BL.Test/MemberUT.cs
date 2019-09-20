using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC.Connections.BL;

namespace CC.Connections.BL.Test
{
    //NOTE PB:
    //  1 hour writing
    //  2.75 hours debugging
    [TestClass]
    public class MemberUT
    {

        public Member test;
        public int testingID;
        public const string VALUE1 = "test";
        public const string VALUE2 = "updated";
        public const int INT1 = -7;
        public const int INT2 = -22;

        [TestMethod]
        public void Insert()
        {
            Member newt = new Member();
            newt.Contact.Phone = "1234567";
            newt.Contact.FirstName = VALUE1;
            newt.Contact.LastName = VALUE2;
            newt.helping_Action_List.Add(new Helping_Action {category =new Category {Desc =VALUE1 } });
            newt.helping_Action_List.Add(new Helping_Action { Action = VALUE1 });
            newt.Member_Type.Desc = VALUE1;
            newt.Password.Hash = VALUE1;
            newt.Pref.distance = INT1;
            newt.Prefered_Categories.Add(new Category {Desc = VALUE1 });

            //CharityList loadChar = new CharityList();
            //loadChar.load();
            //newt.Prefered_Charity_ID_List.Add(loadChar[0].ID);

            newt.Insert();
            testingID = newt.ID;
        }

        [TestMethod]
        public void LoadAll()
        {
            MemberList table = new MemberList();
            table.Load();

            Assert.AreNotEqual(0, table.Count);

            Assert.AreEqual(testingID, table.Find(f => f.Contact.FirstName == VALUE1).ID);
        }
        [TestMethod]
        public void Load()
        {
            test = new Member {ID =testingID };
            test.LoadId();

            Assert.IsTrue(test.Contact.Phone == "1234567");
            Assert.IsTrue(test.helping_Action_List[1].Action == VALUE1);
            Assert.IsTrue(test.helping_Action_List[0].category.Desc == VALUE1);
            Assert.IsTrue(test.Member_Type.Desc == VALUE1);
            Assert.IsFalse(test.Password.Hash == VALUE1);
            Assert.IsTrue(test.Pref.distance == INT1);
            Assert.IsTrue(test.Prefered_Categories[0].Desc == VALUE1);
            //Assert.AreNotEqual(0, test.Prefered_Charity_ID_List.Count);
            //Assert.IsTrue(test.Role.Description == VALUE1);
        }
        [TestMethod]
        public void Update()
        {
            Member updated = test;
            updated.Contact.LastName = VALUE2;
            updated.helping_Action_List[0].Action = VALUE2;
            updated.helping_Action_List[1].category.Desc = VALUE2;
            updated.Member_Type.Desc = VALUE2;
            updated.Password.Hash = VALUE2;
            updated.Pref.distance = INT2;
            updated.Prefered_Categories[0].Desc = VALUE2;
            //updated.Prefered_Charity_ID_List.Clear();
            updated.Update();//update database

            updated = new Member { ID = testingID };//clear
            updated.LoadId();//load again

            Assert.IsTrue(updated.Contact.LastName == VALUE2);
            Assert.IsTrue(updated.helping_Action_List[0].Action == VALUE2);
            Assert.IsTrue(updated.helping_Action_List[1].category.Desc == VALUE2);
            Assert.IsTrue(updated.Member_Type.Desc == VALUE2);
            Assert.IsFalse(updated.Password.Hash == test.Password.Hash);
            Assert.IsTrue(updated.Pref.distance == INT2);
            Assert.IsTrue(updated.Prefered_Categories[0].Desc == VALUE2);
            //Assert.AreEqual(0, updated.Prefered_Charity_ID_List.Count);
        }
        [TestMethod]
        public void Delete()
        {
            test.Delete();//delete

            MemberList table = new MemberList();
            table.Load();//load updated table

            Assert.IsNull(table.Find(f => f.Contact.FirstName == VALUE1));//may need to test for different nonexistant value
        }
    }
}
