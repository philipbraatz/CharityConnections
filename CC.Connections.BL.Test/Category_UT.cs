using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Doorfail.Connections.BL;
using System.Linq;
using Doorfail.DataConnection;

namespace Doorfail.Connections.BL.Test
{
    [TestClass]
    public class CategoryUT
    {
        private string file = "C:\\inetpub\\testData.json";

        //in the real code you would already have the ID you want
        //so you would not need to find the matching discription
        private Guid getID_fromDesc(string desc)
        {
            Category cat = CategoryCollection.INSTANCE.Where(c => c.Desc == desc).FirstOrDefault();
            if (cat == null)
                Assert.Fail();
            return cat.ID;
        }

        [TestMethod]
        public void Insert()
        {
            JsonDatabase.setFile(file);

            Category newt = new Category
            {
                Desc = "this is mine"
            };

            newt.Insert();

            newt = new Category
            {
                Desc = "best test desc"
            };

            newt.Insert();
        }

        [TestMethod]
        public void LoadAll()
        {
            JsonDatabase.file = file;

            Assert.AreNotEqual(0, CategoryCollection.INSTANCE.Count);

            Assert.IsNotNull(CategoryCollection.INSTANCE.Where(f => f.Desc == "this is mine"));
        }


        [TestMethod]
        public void Update()
        {
            JsonDatabase.file = file;

            int tableCount = CategoryCollection.INSTANCE.Count;

            Category test = new Category(getID_fromDesc("this is mine"));//original
            Category updated = new Category(getID_fromDesc("this is mine"))//Updated instance
            {
                Desc = "hello there"
            };
            updated.Update();//update database

            updated = null;//clear
            updated = new Category(getID_fromDesc("hello there"));//load again with new value
            Assert.IsTrue(CategoryCollection.INSTANCE.Exists(c => c.Desc == "hello there"));
            Assert.AreEqual(tableCount, CategoryCollection.INSTANCE.Count);//count unchanged
        }


        [TestMethod]
        public void LoadOne()
        {
            JsonDatabase.file = file;

            Category test = new Category(getID_fromDesc("best test desc"));

            Assert.IsTrue(test.Desc == "best test desc");
        }

        [TestMethod]
        public void Delete()
        {
            Assert.IsNotNull(CategoryCollection.INSTANCE.Find(f => f.Desc == "this is mine"));
            Assert.IsNotNull(CategoryCollection.INSTANCE.Find(f => f.Desc == "best test desc"));

            Category test = new Category(getID_fromDesc("this is mine"));
            test.Delete();
            test = new Category(getID_fromDesc("best test desc"));
            test.Delete();


            Assert.IsNull(CategoryCollection.INSTANCE.Find(f => f.Desc == "best test desc"));
            Assert.IsNull(CategoryCollection.INSTANCE.Find(f => f.Desc == "this is mine"));
        }

        //[TestMethod]
        //public void getName()
        //{
        //System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
        //}
    }
}
