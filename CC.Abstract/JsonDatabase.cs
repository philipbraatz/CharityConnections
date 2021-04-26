using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Doorfail.DataConnection
{
    public static class JsonDatabase
    {
        public static bool inintalized = false;
        public static string file = "JsonDB_keyValue.json";

        private static Dictionary<Type, List<Object>> Data = new Dictionary<Type, List<Object>>();

        public static List<TEntity> GetTable<TEntity>() where TEntity : class
        {
            List<TEntity> ret = new List<TEntity>();
            if (Data.ContainsKey(typeof(TEntity)))
                foreach (var col in Data[typeof(TEntity)])
                    if (typeof(TEntity) != col.GetType())
                        ret.Add(((JObject)col).ToObject<TEntity>());
                    else
                        ret.Add((TEntity)col);
            return ret;
        }
        public static void SetTable<TEntity>(List<TEntity> value) where TEntity : class
        => Data[typeof(TEntity)] = value.ToList().Cast<Object>().ToList();

        public static void SaveChanges()
        {
            string[] serialized = new string[Data.Count];
            KeyValuePair<Type, List<Object>>[] d = Data.ToArray();

            //serialized            [0] = "[";
            for (int i = 0; i < Data.Count; i++)
                serialized[i] = JsonConvert.SerializeObject(d[i]);//+",";
            //serialized   [Data.Count] = JsonConvert.SerializeObject(d);
            //serialized[Data.Count + 1]= "]";

            File.WriteAllLines(file, serialized);
        }

        public static void LoadDatabase()
        {
            Data.Clear();
            try
            {
                foreach (string line in File.ReadLines(file))
                {
                    KeyValuePair<Type, List<Object>> kvp = JsonConvert.DeserializeObject<KeyValuePair<Type, List<Object>>>(line);
                    {
                        //Object f = Convert.ChangeType(kvp.Value, kvp.Key);
                        Data.Add(kvp.Key, kvp.Value);
                    }
                }
            }
            catch (Exception e)
            {
                //throw e;
                //try
                //{
                //    using (CCEntities db = new CCEntities())
                //    {
                //        CreateJsonDatabase(new List<List<Object>> {
                //        db.Categories    .ToList().Cast<Object>().ToList(),
                //        db.Charities     .ToList().Cast<Object>().ToList(),
                //        db.Locations     .ToList().Cast<Object>().ToList(),
                //        db.CharityEvents .ToList().Cast<Object>().ToList(),
                //        db.ContactInfoes .ToList().Cast<Object>().ToList(),
                //        //db.HelpingActions.ToList().Cast<Object>().ToList(),
                //        db.LogIns        .ToList().Cast<Object>().ToList(),
                //        //db.MemberActions .ToList().Cast<Object>().ToList(),
                //        db.Preferences   .ToList().Cast<Object>().ToList(),
                //        db.Volunteers    .ToList().Cast<Object>().ToList()
                //    });
                //    }
                //}
                //catch(Exception badthing)
                //{
                //    if (badthing.InnerException != null)
                //        throw badthing.InnerException;
                //    else
                //        throw badthing;
                //}
            }
            JsonDatabase.inintalized = true;
        }



        public static void CreateJsonDatabase(List<List<Object>> alldata)
        {
            Data.Clear();
            foreach (List<Object> table in alldata)
                Data.Add(table.First().GetType(), table);

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
