using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Doorfail.Connections.BL;
using System.Linq;
using Doorfail.Connections.PL;

namespace Doorfail.Connections.BL.Test
{
    //TODO outdated code needs to be rewritten
    //[TestClass]
    //public class CategoryUT
    //{
    //    public CategoryList allTable;
    //
    //    public Category test;
    //    public const int TEST_MEMBER_ID = -1;
    //    public string memberEmail = "test@test.com";
    //    public const string INSERT_1 = "test 1 for categories";
    //    public const string INSERT_2 = "second test for categories";
    //    public const string UPDATE_1 = "oven for categories";
    //    public const string UPDATE_2 = "toaster for categories";
    //
    //    //in the real code you would already have the ID you want
    //    //so you would not need to find the matching discription
    //    private Guid getID_fromDesc(string desc)
    //    {
    //        if (allTable == null)
    //            allTable = new BL.CategoryList();
    //        allTable.LoadAll();
    //
    //        Category cat = allTable.Where(c => c.Category_Desc == desc).FirstOrDefault();
    //        if (cat == null)
    //            Assert.Fail();
    //        return cat.ID;
    //    }
    //
    //    [TestMethod]
    //    public void Insert()
    //    {
    //        Category newt = new Category
    //        {
    //            Category_Desc = INSERT_1
    //        };
    //
    //        newt.Insert();
    //
    //        newt = new Category
    //        {
    //            Category_Desc = INSERT_2
    //        };
    //
    //        newt.Insert();
    //    }
    //
    //    [TestMethod]
    //    public void LoadAll()
    //    {
    //        CategoryList table = new BL.CategoryList();
    //        table.LoadAll();
    //
    //        Assert.AreNotEqual(0, table.Count);
    //
    //        Assert.IsNotNull(table.Where(f => f.Category_Desc == INSERT_1));
    //    }
    //
    //    [TestMethod]
    //    public void insertPreferences_AndLoad()
    //    {
    //        Volunteer mtest = new Volunteer(memberEmail);//get a member id
    //        CategoryPreferences table = new BL.CategoryPreferences(mtest.ID);
    //        table.LoadPreferences();
    //        //Assert.AreEqual(0, table.Count);//make sure its empty
    //
    //        CategoryList allTable = new BL.CategoryList();
    //        allTable.LoadAll();
    //
    //        table.Add(new Category(getID_fromDesc(INSERT_1)));//add a AbsCategory ID
    //        table.Add(new Category(getID_fromDesc(INSERT_2)));//add a AbsCategory ID
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
    //            allTable = new BL.CategoryList();
    //        allTable.LoadAll();
    //        int tableCount = allTable.Count;
    //
    //        test = new Category(getID_fromDesc(INSERT_1));
    //        Category updated = new Category(getID_fromDesc(INSERT_1))
    //        {
    //            Category_Desc = UPDATE_1
    //        };
    //        updated.Update();//update database
    //
    //        //update both categories
    //        updated = new Category(getID_fromDesc(INSERT_2))
    //        {
    //            Category_Desc = UPDATE_2
    //        };
    //        updated.Update();//update database
    //
    //        updated = null;//clear
    //        updated = new Category(getID_fromDesc(UPDATE_2));//load again with new value
    //        //Assert.IsTrue(updated.Category_Desc == UPDATE_2);
    //        allTable.LoadAll();
    //        //Assert.AreEqual(tableCount,allTable.Count);//count unchanged
    //    }
    //
    //    /// <summary>
    //    /// CAll after Update
    //    /// </summary>
    //    [TestMethod]
    //    public void removePreferences()
    //    {
    //        Volunteer mtest = new Volunteer(memberEmail);
    //        CategoryPreferences table = new BL.CategoryPreferences(mtest.ID);
    //
    //        CategoryList allTable = new BL.CategoryList();
    //        allTable.LoadAll();
    //
    //        table.LoadPreferences();
    //        //Assert.AreEqual(2, table.Count);
    //
    //        table.Remove(new Category(getID_fromDesc(UPDATE_1)));
    //        //Assert.AreEqual(1, table.Count);
    //
    //        table.Remove(new Category(getID_fromDesc(UPDATE_2)));
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
    //        test = new Category(getID_fromDesc(INSERT_1));
    //
    //        Assert.IsTrue(test.Category_Desc == INSERT_1);
    //    }
    //    [TestMethod]
    //    public void LoadPreferences()
    //    {
    //        CategoryPreferences allTable = new BL.CategoryPreferences(getID_fromDesc(INSERT_1));
    //        allTable.LoadPreferences();
    //
    //        test = new Category(getID_fromDesc(INSERT_1));
    //
    //        Assert.IsTrue(test.Category_Desc == INSERT_1);
    //    }
    //    
    //    [TestMethod]
    //    public void Delete()
    //    {
    //        test = new Category(getID_fromDesc(UPDATE_1));
    //        test.Delete();//delete
    //        test = new Category(getID_fromDesc(UPDATE_2));
    //        test.Delete();//delete both to avoid testing overflow
    //        
    //        CategoryList table = new CategoryList();
    //        table.LoadAll();//load updated table
    //        
    //        Assert.IsNull(table.Find(f => f.Category_Desc == UPDATE_2));//may need to test for different nonexistant value
    //    }
    //}
}
