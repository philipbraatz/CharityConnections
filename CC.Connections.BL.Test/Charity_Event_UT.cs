using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CC.Connections.BL;
using System.Linq;

namespace CC.Connections.BL.Test
{
    //NOTE PB:
    //  .5 hours creating
    //[TestClass]
    //public class Charity_EventUT
    //{
    //    public CharityEventList allTable;
    //
    //    public CharityEvent test;
    //    public const int CHARITY_ID = -1;
    //    public const int LOC_ID = -1;
    //    public const string INSERT_1 = "test for charityEvents";
    //    public const string NAME_OF_EVENT = "title name for charityEvents";
    //    public const string UPDATE_1 = "oven for charityEvents";
    //    public const string UPDATE_2 = "toaster for charityEvents";
    //
    //    //in the real code you would already have the ID you want
    //    //so you would not need to find the matching discription
    //    private int getID_fromDesc(string desc)
    //    {
    //        if (allTable == null)
    //            allTable = new BL.CharityEventList();
    //        allTable.LoadList();
    //
    //        CharityEvent cat = allTable.Where(c => c.CharityEventName == desc).FirstOrDefault();
    //        if (cat == null)
    //            Assert.Fail();
    //        return cat.Event_ID;
    //    }
    //
    //    [TestMethod]
    //    public void Insert()
    //    {
    //        CharityEvent newt = new CharityEvent
    //        {
    //            ContactInfo_Phone = "1234567",
    //            ContactInfo_FName = INSERT_1,
    //            ContactInfo_LName = INSERT_1,
    //            CharityEventName = NAME_OF_EVENT,
    //            CharityEventRequirements= INSERT_1,
    //            CharityEventStatus= INSERT_1,
    //            Charity_ID =CHARITY_ID,
    //            ContactInfo_Email =INSERT_1,
    //            DateOfBirth =DateTime.Now,
    //            EndDate = DateTime.Now,
    //            StartDate = DateTime.Now,
    //            location = new Location(LOC_ID)
    //        };
    //        newt.Insert();
    //    }
    //
    //    [TestMethod]
    //    public void LoadAll()
    //    {
    //        CharityEventList table = new BL.CharityEventList();
    //        table.LoadList();
    //
    //        Assert.AreNotEqual(0, table.Count);
    //
    //        Assert.IsNotNull(table.Where(f => f.CharityEventName == INSERT_1));
    //    }
    //
    //    [TestMethod]
    //    public void insertPreferences_AndLoad()
    //    {
    //        CharityEventList mtest = new DbSet<TEntity> tableCharityEventList(memberEmail);//get a member id
    //        CharityEventList table = new BL.CharityEventList();
    //
    //        table.LoadPreferences(mtest.ID);
    //        //Assert.AreEqual(0, table.Count);//make sure its empty
    //
    //        CharityEventList allTable = new BL.CharityEventList();
    //        allTable.LoadList();
    //
    //        table.AddPreference(getID_fromDesc(INSERT_1));//add a CharityEvent ID
    //        table.AddPreference(getID_fromDesc(UPDATE_1));//add a CharityEvent ID
    //
    //        //compare to all
    //
    //        //Assert.AreEqual(2, table.Count);
    //        Assert.IsTrue(allTable.Count >= table.Count);//test table should be a subset of all table
    //
    //        //LOAD test
    //        table.Clear();
    //        table.LoadPreferences(mtest.ID);
    //
    //        //Assert.AreEqual(2, table.Count);
    //    }
    //
    //    [TestMethod]
    //    public void Update()
    //    {
    //        if (allTable == null)
    //            allTable = new BL.CharityEventList();
    //        allTable.LoadList();
    //        int tableCount = allTable.Count;
    //
    //        test = new CharityEvent(getID_fromDesc(NAME_OF_EVENT));
    //        CharityEvent updated = new CharityEvent(getID_fromDesc(NAME_OF_EVENT))
    //        {
    //            CharityEventName = NAME_OF_EVENT,
    //            location = new Location()
    //        };
    //        updated.Update();//update database
    //
    //        //update both charityEvents
    //        updated = new CharityEvent(getID_fromDesc(UPDATE_1))
    //        {
    //            CharityEventName = UPDATE_2
    //        };
    //        updated.Update();//update database
    //
    //        updated = null;//clear
    //        updated = new CharityEvent(getID_fromDesc(UPDATE_2));//load again with new value
    //        Assert.IsTrue(updated.CharityEvent_Desc == UPDATE_2);
    //        allTable.LoadList();
    //        Assert.AreEqual(tableCount, allTable.Count);//count unchanged
    //    }
    //
    //    /// <summary>
    //    /// CAll after Update
    //    /// </summary>
    //    [TestMethod]
    //    public void removePreferences()
    //    {
    //        Member mtest = new Member(memberEmail);
    //        CharityEventList table = new BL.CharityEventList();
    //
    //        CharityEventList allTable = new BL.CharityEventList();
    //        allTable.LoadList();
    //
    //        table.LoadPreferences(mtest.ID);
    //        //Assert.AreEqual(2, table.Count);
    //
    //        table.DeletePreference(getID_fromDesc(NAME_OF_EVENT));
    //        //Assert.AreEqual(1, table.Count);
    //
    //        table.DeletePreference(getID_fromDesc(UPDATE_2));
    //        //Assert.AreEqual(0, table.Count);
    //
    //        //CLEAR
    //        table.Clear();
    //        table.LoadPreferences(mtest.ID);
    //        //Assert.AreEqual(0, table.Count);
    //
    //    }
    //    [TestMethod]
    //    public void Load()
    //    {
    //        CharityEventList allTable = new BL.CharityEventList();
    //        allTable.LoadList();
    //
    //        test = new CharityEvent(getID_fromDesc(INSERT_1));
    //
    //        Assert.IsTrue(test.CharityEvent_Desc == INSERT_1);
    //    }
    //
    //    [TestMethod]
    //    public void Delete()
    //    {
    //        test = new CharityEvent(getID_fromDesc(NAME_OF_EVENT));
    //        test.Delete();//delete
    //        test = new CharityEvent(getID_fromDesc(UPDATE_2));
    //        test.Delete();//delete both to avoid testing overflow
    //
    //        CharityEventList table = new CharityEventList();
    //        table.LoadList();//load updated table
    //
    //        Assert.IsNull(table.Find(f => f.Event_ID == getID_fromDesc(NAME_OF_EVENT)));//may need to test for different nonexistant value
    //    }
    //
    //    [TestMethod]
    //    public void AbstractionTest()
    //    {
    //        CharityEvent2<PL.CharityEvent> c = new BL.CharityEvent2<PL.CharityEvent>();
    //        c.Clear();
    //    }
    //}
}
