using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC.Connections.BL;
using System.Linq;
using CC.Connections.PL;

namespace CC.Connections.BL.Test
{
    [TestClass]
    public class Helping_ActionUT
    {
        public AbsHelping_ActionList allTable;

        public AbsHelping_Action test;
        public const int TEST_MEMBER_ID = -1;
        public string memberEmail = "test@test.com";
        public const string INSERT_1 = "test 1 for helping_actions";
        public const string INSERT_2 = "second test for helping_actions";
        public const string UPDATE_1 = "oven for helping_actions";
        public const string UPDATE_2 = "toaster for helping_actions";

        //in the real code you would already have the ID you want
        //so you would not need to find the matching discription
        private int getID_fromDesc(string desc)
        {
            if (allTable == null)
                allTable = new BL.AbsHelping_ActionList();
            allTable.LoadAll();

            AbsHelping_Action cat = allTable.Where(c => c.Action == desc).FirstOrDefault();
            if (cat == null)
                Assert.Fail();
            return cat.ID;
        }

        [TestMethod]
        public void Insert()
        {
            AbsHelping_Action newt = new AbsHelping_Action
            {
                Action = INSERT_1
            };

            newt.Insert();

            newt = new AbsHelping_Action
            {
                Action = INSERT_2
            };

            newt.Insert();
        }

        [TestMethod]
        public void LoadAll()
        {
            AbsHelping_ActionList table = new BL.AbsHelping_ActionList();
            table.LoadAll();

            Assert.AreNotEqual(0, table.Count);

            Assert.IsNotNull(table.Where(f => f.Action == INSERT_1));
        }

        [TestMethod]
        public void Update()
        {
            if (allTable == null)
                allTable = new BL.AbsHelping_ActionList();
            allTable.LoadAll();
            int tableCount = allTable.Count;

            test = new AbsHelping_Action(getID_fromDesc(INSERT_1));
            AbsHelping_Action updated = new AbsHelping_Action(getID_fromDesc(INSERT_1))
            {
                Action = UPDATE_1
            };
            updated.Update();//update database

            //update both helping_actions
            updated = new AbsHelping_Action(getID_fromDesc(INSERT_2))
            {
                Action = UPDATE_2
            };
            updated.Update();//update database

            updated = null;//clear
            updated = new AbsHelping_Action(getID_fromDesc(UPDATE_2));//load again with new value
            //Assert.IsTrue(updated.Action == UPDATE_2);
            allTable.LoadAll();
            //Assert.AreEqual(tableCount,allTable.Count);//count unchanged
        }

        [TestMethod]
        public void Load()
        {
            test = new AbsHelping_Action(getID_fromDesc(INSERT_1));

            Assert.IsTrue(test.Action == INSERT_1);
        }

        [TestMethod]
        public void Delete()
        {
            test = new AbsHelping_Action(getID_fromDesc(UPDATE_1));
            test.Delete();//delete
            test = new AbsHelping_Action(getID_fromDesc(UPDATE_2));
            test.Delete();//delete both to avoid testing overflow

            AbsHelping_ActionList table = new AbsHelping_ActionList();
            table.LoadAll();//load updated table

            Assert.IsNull(table.Find(f => f.Action == UPDATE_2));//may need to test for different nonexistant value
        }
    }
}
