using Doorfail.Connections.PL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//TODO Abstract
namespace Doorfail.DataConnection
{
    public static class JsonDatabase
    {
        public static bool inintalized = false;

        private static Dictionary<Type, List<Object>> Data = new Dictionary<Type, List<Object>>();

        public static List<TEntity> GetTable<TEntity>() where TEntity : class
        {
            List<TEntity> ret = new List<TEntity>();
            if (Data.ContainsKey(typeof(TEntity)))
                foreach (var col in Data[typeof(TEntity)])
                    ret.Add((TEntity)col);
            return ret;
        }
        public static void SetTable<TEntity>(List<TEntity> value) where TEntity : class
        => Data[typeof(TEntity)] = (List<Object>)(Object)value;

        public static void SaveChanges()
        {
            string[] serialized = new string[Data.Count+2];
            KeyValuePair<Type,List<Object>>[] d = Data.ToArray();

            serialized            [0] = "[";
            for (int i = 1; i < Data.Count-1; i++)
                serialized        [i] = JsonConvert.SerializeObject(d)+",";
            serialized   [Data.Count] = JsonConvert.SerializeObject(d);
            serialized[Data.Count + 1]= "]";

            File.WriteAllLines("JsonDB_keyValue.json",serialized);
        }

        public static void LoadDatabase()
        {
            Data.Clear();
            try
            {
                JsonConvert.DeserializeObject<KeyValuePair<Type, List<Object>>[]>
                    (File.ReadAllText("JsonDB_keyValue.json")).ToList()
                    .ForEach(c => Data.Add(c.Key, c.Value));
            }
            catch (Exception e) {
                try
                {
                    using (CCEntities db = new CCEntities())
                    {
                        CreateJsonDatabase(new List<List<Object>> {
                        db.Categories    .ToList().Cast<Object>().ToList(),
                        db.Charities     .ToList().Cast<Object>().ToList(),
                        db.Locations     .ToList().Cast<Object>().ToList(),
                        db.CharityEvents .ToList().Cast<Object>().ToList(),
                        db.ContactInfoes .ToList().Cast<Object>().ToList(),
                        //db.HelpingActions.ToList().Cast<Object>().ToList(),
                        db.LogIns        .ToList().Cast<Object>().ToList(),
                        //db.MemberActions .ToList().Cast<Object>().ToList(),
                        db.Preferences   .ToList().Cast<Object>().ToList(),
                        db.Volunteers    .ToList().Cast<Object>().ToList()
                    });
                    }
                }
                catch(Exception badthing)
                {
                    throw badthing.InnerException;
                }
            }
            JsonDatabase.inintalized = true;
        }



        public static void CreateJsonDatabase(List<List<Object>> alldata)
        {
            Data.Clear();
            foreach (List<Object> table in alldata)
                Data.Add(table.First().GetType(),table);

            SaveChanges();
        }


        internal static void SaveChanges<TEntity>(List<TEntity> table)
            where TEntity : class
        {
            Data[typeof(TEntity)] = table.ToList().Cast<Object>().ToList();
            SaveChanges();
        }
    }
}
