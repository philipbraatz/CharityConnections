using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Doorfail.DataConnection;
using System.Collections.Generic;
using System.Linq;
using Doorfail.Connections.PL;

namespace Doorfail.DataConnection.Test
{
    [TestClass]
    public class JsonDatabaseUT
    {
        [TestMethod]
        public void DataCreationTest()
        {
            JsonDatabase.file = "testData.json";

            Doorfail.DataConnection.JsonDatabase.CreateJsonDatabase( new List<List<object>> {
                new List<Category> { 
                    new Category { ID = Guid.NewGuid(),Color="BBBBBB" },
                    new Category{ ID=Guid.NewGuid(),Color="AAAAAA"}
                }.Cast<Object>().ToList(),
                 new List<ContactInfo> { 
                     new ContactInfo { MemberEmail = "myname@cool.com",DateOfBirth=DateTime.Now,FName="John",LName="Cenit" },
                     new ContactInfo { MemberEmail = "yees@ben.com",DateOfBirth=DateTime.Now,FName="lemen",LName="corn"  }
                }.Cast<Object>().ToList(),
            });
        }

        [TestMethod]
        public void LoadDatabaseTest()
        {
            JsonDatabase.file = "testData.json";
            Doorfail.DataConnection.JsonDatabase.LoadDatabase();

            List<Category> d = JsonDatabase.GetTable<Category>();
            List<ContactInfo> b = JsonDatabase.GetTable<ContactInfo>();
            Assert.IsTrue(d.Where(c => c.Color == "BBBBBB").Any());
            Assert.IsTrue(b.Where(m=>m.MemberEmail == "myname@cool.com").Any());
        }

        [TestMethod]
        public void SaveDatabaseUpdates()
        {
            //use test
            JsonDatabase.file = "testData.json";
            Doorfail.DataConnection.JsonDatabase.LoadDatabase();

            //load categories, add CCAACC
            List<Category> d = JsonDatabase.GetTable<Category>();
            Category updateMe = new Category { ID = new Guid(),Image="Llama", Color="CCAACC", Desc="test Lama" };
            d.Add(updateMe);

            //load contacts, delete myname@cool.com
            List<ContactInfo> b = JsonDatabase.GetTable<ContactInfo>();
            ContactInfo delete = b.Where(m => m.MemberEmail == "myname@cool.com").FirstOrDefault();
            b.Remove(delete);

            //objects exists before updating
            Assert.IsFalse(JsonDatabase.GetTable<Category>().Where(c => c.Color == "CCAACC").Any());
            Assert.IsTrue(JsonDatabase.GetTable<ContactInfo>().Where(m => m.MemberEmail == "myname@cool.com").Any());

            //update
            Doorfail.DataConnection.JsonDatabase.SetTable<Category>(d);
            JsonDatabase.SetTable<ContactInfo>(b);

            //objects no longer exist in database
            Assert.IsTrue(JsonDatabase.GetTable<Category>().Where(c => c.Color == "CCAACC").Any());
            Assert.IsFalse(JsonDatabase.GetTable<ContactInfo>().Where(m => m.MemberEmail == "myname@cool.com").Any());

            //Reload database
            JsonDatabase.SaveChanges();
            Doorfail.DataConnection.JsonDatabase.LoadDatabase();


            Assert.IsTrue(JsonDatabase.GetTable<Category>().Where(c => c.Color == "BBBBBB").Any());
            Assert.IsFalse(JsonDatabase.GetTable<ContactInfo>().Where(m => m.MemberEmail == "myname@cool.com").Any());
            Assert.IsTrue(JsonDatabase.GetTable<Category>().Where(c => c.Color == "CCAACC").Any());
        }
    }
}
