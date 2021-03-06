using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Doorfail.Connections.BL;
using System.Linq;

//namespace Doorfail.Connections.BL.Test
//{
//    //NOTE PB:
//    //  .5 hours creating
//    [TestClass]
//    public class CharityEventUT
//    {
//        public CharityEventCollection allTable;
//    
//        public CharityEvent test;
//        public const int CHARITY_ID = 1;
//        public const int LOC_ID = 1;
//        public const string CONTACT_EMAIL = "test@gmail.com";
//        public const string INSERT_1 = "test for charityEvents";
//        public const string INSERT_3 = "test";
//        public const string NAME_OF_EVENT = "title name for charityEvents";
//        public const string UPDATE_1 = "oven for charityEvents";
//    
//        //in the real code you would already have the ID you want
//        //so you would not need to find the matching discription
//        private int getID_fromDesc(string desc)
//        {
//            if (allTable == null)
//                allTable = new BL.CharityEventCollection();
//            allTable.LoadAll();
//    
//            CharityEvent cat = allTable.Where(c => c.Name == desc).FirstOrDefault();
//            if (cat == null)
//                Assert.Fail("Could not find item with discription \""+desc+"\" in table");
//            return cat.ID;
//        }
//    
//        [TestMethod]
//        public void Insert()
//        {
//            CharityEvent newt = new CharityEvent
//            {
//                Name = NAME_OF_EVENT,
//                Requirements= INSERT_1,
//                CharityEmail =CHARITY_ID,
//                EndDate = DateTime.Now,
//                StartDate = DateTime.Now,
//                Location = new Location(LOC_ID)
//            };
//            newt.Insert();
//        }
//    
//        [TestMethod]
//        public void LoadAll()
//        {
//            CharityEventCollection table = new BL.CharityEventCollection();
//            table.LoadAll();
//    
//            Assert.AreNotEqual(0, table.Count);
//    
//            Assert.IsNotNull(table.Where(f => f.Name == INSERT_1));
//        }
//    
//        [TestMethod]
//        public void insertEvent_AndLoad()
//        {
//            CharityEvent newt = new CharityEvent
//            {
//                Name = NAME_OF_EVENT,
//                Requirements = INSERT_1,
//                CharityEmail = CHARITY_ID,
//                EndDate = DateTime.Now,
//                StartDate = DateTime.Now,
//                Location = new Location(LOC_ID)
//            };
//
//            CharityEventCollection table = new BL.CharityEventCollection();
//            table.LoadWithFilter(CHARITY_ID,SortBy.CHARITY);
//            int charityID_Event_count = table.Count;
//            //Assert.AreEqual(0, table.Count);//make sure its empty
//
//            CharityEventCollection allTable = new BL.CharityEventCollection();
//            allTable.LoadAll();
//            int initalCount = allTable.Count;
//
//            table.AddEvent(newt);//add a CharityEvent ID
//            allTable.LoadAll();
//            int newCount = allTable.Count;
//
//            //compare to all
//
//            //Assert.AreEqual(2, table.Count);
//            Assert.IsTrue(allTable.Count >= table.Count);//test table should be a subset of all table
//            Assert.AreEqual(initalCount +1, newCount);
//
//            //LOAD test
//            table.Clear();
//            table.LoadWithFilter(CHARITY_ID,SortBy.CHARITY);
//            int new_charityID_Event_count = table.Count;
//            Assert.AreEqual(charityID_Event_count+1,new_charityID_Event_count);
//        }
//    
//        [TestMethod]
//        public void Update()
//        {
//            if (allTable == null)
//                allTable = new BL.CharityEventCollection();
//            allTable.LoadAll();
//            int tableCount = allTable.Count;
//    
//            test = new CharityEvent(getID_fromDesc(NAME_OF_EVENT));
//            CharityEvent updated = new CharityEvent(getID_fromDesc(NAME_OF_EVENT))
//            {
//                Name = UPDATE_1,
//                Location = new Location()
//            };
//            updated.Update();//update database
//   
//    
//            updated = null;//clear
//            updated = new CharityEvent(getID_fromDesc(UPDATE_1));//load again with new value
//            Assert.IsTrue(updated.CharityEventName == UPDATE_1);
//            allTable.LoadAll();
//            Assert.AreEqual(tableCount, allTable.Count);//count unchanged
//        }
//    
//        /// <summary>
//        /// CAll after Update
//        /// </summary>
//        [TestMethod]
//        public void removeEvent()
//        {
//            CharityEventCollection table = new BL.CharityEventCollection();
//    
//            CharityEventCollection allTable = new BL.CharityEventCollection();
//            allTable.LoadAll();
//    
//            table.LoadWithFilter(CHARITY_ID,SortBy.CHARITY);
//            //Assert.AreEqual(1, table.Count);
//    
//            table.DeleteEvent(getID_fromDesc(NAME_OF_EVENT));
//            //Assert.AreEqual(0, table.Count);
//    
//    
//            //CLEAR
//            table.Clear();
//            table.LoadWithFilter(CHARITY_ID,SortBy.CHARITY);
//            //Assert.AreEqual(0, table.Count);
//    
//        }
//        [TestMethod]
//        public void Load()
//        {
//            CharityEventCollection allTable = new BL.CharityEventCollection();
//            allTable.LoadAll();
//    
//            test = new CharityEvent(getID_fromDesc(NAME_OF_EVENT));
//    
//            Assert.IsTrue(test.CharityEventName == NAME_OF_EVENT);
//        }
//    
//        [TestMethod]
//        public void Delete()
//        {
//            test = new CharityEvent(getID_fromDesc(UPDATE_1));
//            test.Delete();//delete
//    
//            CharityEventCollection table = new CharityEventCollection();
//            table.LoadAll();//load updated table
//    
//            //Assert.IsNull(table.Find(f => f.EventID == getID_fromDesc(UPDATE_1)));//may need to test for different nonexistant value
//        }
//    }
//}
//