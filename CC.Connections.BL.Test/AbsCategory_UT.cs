using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC.Connections.BL;
using System.Linq;
using CC.Connections.PL;

namespace CC.Connections.BL.Test
{
    [TestClass]
    public class AbsCategoryUT
    {
        public AbsCategoryList allTable;

        public AbsCategory test;
        public const int TEST_MEMBER_ID = -1;
        public string memberEmail = "test@test.com";
        public const string INSERT_1 = "test 1 for categories";
        public const string INSERT_2 = "second test for categories";
        public const string UPDATE_1 = "oven for categories";
        public const string UPDATE_2 = "toaster for categories";

        //in the real code you would already have the ID you want
        //so you would not need to find the matching discription
        private int getID_fromDesc(string desc)
        {
            if (allTable == null)
                allTable = new BL.AbsCategoryList();
            allTable.LoadAll();

            AbsCategory cat = allTable.Where(c => c.Category_Desc == desc).FirstOrDefault();
            if (cat == null)
                Assert.Fail();
            return cat.ID;
        }

        [TestMethod]
        public void Insert()
        {
            AbsCategory newt = new AbsCategory
            {
                Category_Desc = INSERT_1
            };

            newt.Insert();

            newt = new AbsCategory
            {
                Category_Desc = INSERT_2
            };

            newt.Insert();
        }

        [TestMethod]
        public void LoadAll()
        {
            AbsCategoryList table = new BL.AbsCategoryList();
            table.LoadAll();

            Assert.AreNotEqual(0, table.Count);

            Assert.IsNotNull(table.Where(f => f.Category_Desc == INSERT_1));
        }

        [TestMethod]
        public void insertPreferences_AndLoad()
        {
            BLMember mtest = new BLMember(memberEmail);//get a member id
            AbsCategoryPreferences table = new BL.AbsCategoryPreferences(mtest.ID);
            table.LoadPreferences();
            //Assert.AreEqual(0, table.Count);//make sure its empty

            AbsCategoryList allTable = new BL.AbsCategoryList();
            allTable.LoadAll();

            table.Add(new AbsCategory(getID_fromDesc(INSERT_1)));//add a AbsCategory ID
            table.Add(new AbsCategory(getID_fromDesc(INSERT_2)));//add a AbsCategory ID

            //compare to all

            //Assert.AreEqual(2, table.Count);
            Assert.IsTrue(allTable.Count >= table.Count);//test table should be a subset of all table
            
            //LOAD test
            table.Clear();
            table.LoadPreferences(mtest.ID);

            //Assert.AreEqual(2, table.Count);
        }

        [TestMethod]
        public void Update()
        {
            if (allTable == null)
                allTable = new BL.AbsCategoryList();
            allTable.LoadAll();
            int tableCount = allTable.Count;

            test = new AbsCategory(getID_fromDesc(INSERT_1));
            AbsCategory updated = new AbsCategory(getID_fromDesc(INSERT_1))
            {
                Category_Desc = UPDATE_1
            };
            updated.Update();//update database

            //update both categories
            updated = new AbsCategory(getID_fromDesc(INSERT_2))
            {
                Category_Desc = UPDATE_2
            };
            updated.Update();//update database

            updated = null;//clear
            updated = new AbsCategory(getID_fromDesc(UPDATE_2));//load again with new value
            //Assert.IsTrue(updated.Category_Desc == UPDATE_2);
            allTable.LoadAll();
            //Assert.AreEqual(tableCount,allTable.Count);//count unchanged
        }

        /// <summary>
        /// CAll after Update
        /// </summary>
        [TestMethod]
        public void removePreferences()
        {
            BLMember mtest = new BLMember(memberEmail);
            AbsCategoryPreferences table = new BL.AbsCategoryPreferences(mtest.ID);

            AbsCategoryList allTable = new BL.AbsCategoryList();
            allTable.LoadAll();

            table.LoadPreferences();
            //Assert.AreEqual(2, table.Count);

            table.Remove(new AbsCategory(getID_fromDesc(UPDATE_1)));
            //Assert.AreEqual(1, table.Count);

            table.Remove(new AbsCategory(getID_fromDesc(UPDATE_2)));
            //Assert.AreEqual(0, table.Count);

            //CLEAR
            table.Clear();
            table.LoadPreferences(mtest.ID);
            //Assert.AreEqual(0, table.Count);

        }
        [TestMethod]
        public void Load()
        {
            test = new AbsCategory(getID_fromDesc(INSERT_1));

            Assert.IsTrue(test.Category_Desc == INSERT_1);
        }
        [TestMethod]
        public void LoadPreferences()
        {
            AbsCategoryPreferences allTable = new BL.AbsCategoryPreferences(getID_fromDesc(INSERT_1));
            allTable.LoadPreferences();

            test = new AbsCategory(getID_fromDesc(INSERT_1));

            Assert.IsTrue(test.Category_Desc == INSERT_1);
        }
        
        [TestMethod]
        public void Delete()
        {
            test = new AbsCategory(getID_fromDesc(UPDATE_1));
            test.Delete();//delete
            test = new AbsCategory(getID_fromDesc(UPDATE_2));
            test.Delete();//delete both to avoid testing overflow
            
            AbsCategoryList table = new AbsCategoryList();
            table.LoadAll();//load updated table
            
            Assert.IsNull(table.Find(f => f.Category_Desc == UPDATE_2));//may need to test for different nonexistant value
        }
    }
}
