using CC.Connections.PL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CC.Abstract
{
    public class JsonDatabase
    {
        public Dictionary<String, DbSet> table = new Dictionary<string, DbSet>();
        public void test()
        {
            CCEntities clp = new CCEntities();
            //clp.
        }
        
        public void SaveChanges()
        => File.WriteAllText(
                "JsonDB.json",
                JsonConvert.SerializeObject(table));

        public void LoadDatabase()
        => table = JsonConvert.DeserializeObject<Dictionary<string, DbSet>>
            (File.ReadAllText("SomeFile.txt"));

        public void CreateJsonDatabase(CCEntities db)
        {
            table.Add(nameof(db.Categories),db.Categories);
            table.Add(nameof(db.CharityEvents), db.CharityEvents);
            table.Add(nameof(db.Charities), db.Charities);
            table.Add(nameof(db.Locations), db.Locations);
            table.Add(nameof(db.ContactInfoes), db.ContactInfoes);
            table.Add(nameof(db.HelpingActions), db.HelpingActions);
            table.Add(nameof(db.LogIns), db.LogIns);
            table.Add(nameof(db.MemberActions), db.MemberActions);
            table.Add(nameof(db.Preferences), db.Preferences);
            table.Add(nameof(db.Volunteers), db.Volunteers);

            SaveChanges();
        }
    }
}
