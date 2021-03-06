using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Doorfail.Connections.BL;
using System.Linq;

namespace Doorfail.Connections.BL.Test
{
    //[TestClass]
    //public class HelpingActionUT
    //{
    //    public HelpingActionCollection allTable;
    //
    //    public HelpingAction test;
    //    public const int TEST_MEMBER_ID = -1;
    //    private const string V = "test@test.com";
    //    public string memberEmail = V;
    //    public const string INSERT_1 = "test 1 for HelpingActions";
    //    public const string INSERT_2 = "second test for HelpingActions";
    //    public const string UPDATE_1 = "oven for HelpingActions";
    //    public const string UPDATE_2 = "toaster for HelpingActions";
    //
    //    //in the real code you would already have the ID you want
    //    //so you would not need to find the matching discription
    //    private Guid getID_fromDesc(string desc)
    //    {
    //        if (allTable == null)
    //            allTable = new BL.HelpingActionCollection();
    //        allTable.LoadAll();
    //
    //        HelpingAction cat = allTable.Where(c => c.Description == desc).FirstOrDefault();
    //        if (cat == null)
    //            Assert.Fail();
    //        return cat.ID;
    //    }
    //
    //    [TestMethod]
    //    public void Insert()
    //    {
    //        HelpingAction newt = new HelpingAction
    //        {
    //            Description = INSERT_1
    //        };
    //
    //        newt.Insert();
    //
    //        newt = new HelpingAction
    //        {
    //            Description = INSERT_2
    //        };
    //
    //        newt.Insert();
    //    }
    //
    //    [TestMethod]
    //    public void LoadAll()
    //    {
    //        HelpingActionCollection table = new BL.HelpingActionCollection();
    //        table.LoadAll();
    //
    //        Assert.AreNotEqual(0, table.Count);
    //
    //        Assert.IsNotNull(table.Where(f => f.Description == INSERT_1));
    //    }
    //
    //    [TestMethod]
    //    public void Update()
    //    {
    //        if (allTable == null)
    //            allTable = new BL.HelpingActionCollection();
    //        allTable.LoadAll();
    //        int tableCount = allTable.Count;
    //
    //        test = new AbsHelpingAction(getID_fromDesc(INSERT_1));
    //        HelpingAction updated = new AbsHelpingAction(getID_fromDesc(INSERT_1))
    //        {
    //            Description = UPDATE_1
    //        };
    //        updated.Update();//update database
    //
    //        //update both HelpingActions
    //        updated = new AbsHelpingAction(getID_fromDesc(INSERT_2))
    //        {
    //            Description = UPDATE_2
    //        };
    //        updated.Update();//update database
    //
    //        updated = null;//clear
    //        updated = new AbsHelpingAction(getID_fromDesc(UPDATE_2));//load again with new value
    //        //Assert.IsTrue(updated.Action == UPDATE_2);
    //        allTable.LoadAll();
    //        //Assert.AreEqual(tableCount,allTable.Count);//count unchanged
    //    }
    //
    //    [TestMethod]
    //    public void Load()
    //    {
    //        test = new AbsHelpingAction(getID_fromDesc(INSERT_1));
    //
    //        Assert.IsTrue(test.Description == INSERT_1);
    //    }
    //
    //    [TestMethod]
    //    public void Delete()
    //    {
    //        test = new AbsHelpingAction(getID_fromDesc(UPDATE_1));
    //        test.Delete();//delete
    //        test = new AbsHelpingAction(getID_fromDesc(UPDATE_2));
    //        test.Delete();//delete both to avoid testing overflow
    //
    //        HelpingActionCollection table = new HelpingActionCollection();
    //        table.LoadAll();//load updated table
    //
    //        Assert.IsNull(table.Find(f => f.Description == UPDATE_2));//may need to test for different nonexistant value
    //    }
    //}
}
