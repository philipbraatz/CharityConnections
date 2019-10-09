﻿//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using CC.Connections.BL;
//
//namespace CC.Connections.BL.Test
//{
//    [TestClass]
//    public class Helping_ActionUT
//    {
//
//        public Helping_Action test;
//        public const int testingID = -3;
//        public const string VALUE1 = "test";
//        public const string VALUE2 = "updated";
//        public const int INT1 = -7;
//        public const int INT2 = -22;
//
//        [TestMethod]
//        public void Insert()
//        {
//            CategoryList cl = new CategoryList();
//            cl.LoadAll();
//
//            Helping_Action newt = new Helping_Action(testingID);
//            newt.Action = VALUE1;
//            newt.category =
//            newt.Contact.LastName = VALUE2;
//            newt.helping_Action_List.Add(new Helping_Action {
//                category =new Category {Desc =VALUE1 }, Action = VALUE2 });
//            newt.helping_Action_List.Add(new Helping_Action { Action = VALUE1 });
//            newt.Member_Type.Desc = VALUE1;
//            newt.Pref.distance = INT1;
//            newt.Prefered_Categories.Add(new Category {Desc = VALUE1 });
//            newt.Location.City = VALUE1;
//
//            //CharityList loadChar = new CharityList();
//            //loadChar.load();
//            //newt.Prefered_Charity_ID_List.Add(loadChar[0].ID);
//
//            newt.Insert();
//        }
//
//        [TestMethod]
//        public void LoadAll()
//        {
//            MemberList table = new MemberList();
//            table.LoadList();
//
//            Assert.AreNotEqual(0, table.Count);
//
//            Assert.AreEqual(testingID, table.Find(f => f.Contact.FirstName == VALUE1).Contact.Email);
//        }
//        [TestMethod]
//        public void Load()
//        {
//            test = new Member(testingID);
//
//            Assert.IsTrue(test.Contact.Phone == "1234567");
//            //Assert.IsTrue(test.helping_Action_List[1].Action == VALUE1);
//            //Assert.IsTrue(test.helping_Action_List[0].category.Desc == VALUE1);
//            Assert.IsTrue(test.Member_Type.Desc == VALUE1);
//            Assert.IsFalse(test.Password.Pass == VALUE1.Trim());
//            Assert.IsTrue(test.Pref.distance == INT1);
//            //Assert.IsTrue(test.Prefered_Categories[0].Desc == VALUE1);
//            //Assert.AreNotEqual(0, test.Prefered_Charity_ID_List.Count);
//            Assert.IsTrue(test.Location.City == VALUE1);
//        }
//        [TestMethod]
//        public void Update()
//        {
//            test = new Member(testingID);
//            Member updated = new Member(testingID);
//            updated.Contact.LastName = VALUE2;
//            updated.helping_Action_List[0].Action = VALUE2;
//            updated.helping_Action_List.Add(new Helping_Action {
//                Action =VALUE1,category =new Category { Desc = VALUE2 } });
//            updated.Member_Type.Desc = VALUE2;
//            updated.Password.Pass = VALUE2;
//            updated.Pref.distance = INT2;
//            updated.Prefered_Categories[0].Desc = VALUE2;
//            updated.Location.City = VALUE2;
//            //updated.Prefered_Charity_ID_List.Clear();
//
//            updated.Update();//update database
//            updated = null;//clear
//            updated = new Member(testingID);//load again
//
//            Assert.IsTrue(updated.Contact.LastName == VALUE2);
//            //TODO fix helping action
//            //Assert.IsTrue(updated.helping_Action_List[0].Action == VALUE2);
//            //Assert.IsTrue(updated.helping_Action_List[1].category.Desc == VALUE2);
//            Assert.IsTrue(updated.Member_Type.Desc == VALUE2);
//            Assert.IsFalse(updated.Password.Pass == test.Password.Pass);
//            Assert.IsTrue(updated.Pref.distance == INT2);
//            Assert.IsTrue(updated.Prefered_Categories[0].Desc == VALUE2);
//            Assert.IsTrue(updated.Location.City == VALUE2);
//            //Assert.AreEqual(0, updated.Prefered_Charity_ID_List.Count);
//        }
//        [TestMethod]
//        public void Delete()
//        {
//            test = new Member(testingID);
//            test.Delete();//delete
//
//            MemberList table = new MemberList();
//            table.LoadList();//load updated table
//
//            Assert.IsNull(table.Find(f => f.Contact.FirstName == VALUE1));//may need to test for different nonexistant value
//        }
//    }
//}
//