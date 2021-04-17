using CC.Connections.PL;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//TODO Abstract
namespace CC.DataConnection
{
    public static class JsonDatabase
    {
        public static bool inintalized = false;

        public static List<Category> Categories = new List<Category>();
        public static List<CharityEvent> CharityEvents = new List<CharityEvent>();
        public static List<Charity> Charities = new List<Charity>();
        public static List<Location> Locations = new List<Location>();
        public static List<ContactInfo> ContactInfos = new List<ContactInfo>();
        public static List<HelpingAction> HelpingActions = new List<HelpingAction>();
        public static List<LogIn> Logins = new List<LogIn>();
        public static List<MemberAction> MemberActions = new List<MemberAction>();
        public static List<Preference> Preferences = new List<Preference>();
        public static List<Volunteer> Volunteers = new List<Volunteer>();
        public static List<HelpingAction> helpingActions = new List<HelpingAction>();
        public static List<MemberAction> memberActions = new List<MemberAction>();
        public static List<EventAttendance> EventAttendances = new List<EventAttendance>();
             
        public static void SaveChanges()
        =>  File.WriteAllLines(
                "JsonDB.json",
                new string[] { 
                    "[",
                    JsonConvert.SerializeObject(Categories)+",",
                    JsonConvert.SerializeObject(CharityEvents)+"," ,
                    JsonConvert.SerializeObject(Charities)+"," ,
                    JsonConvert.SerializeObject(Locations)+"," ,
                    JsonConvert.SerializeObject(ContactInfos)+"," ,
                    //JsonConvert.SerializeObject(HelpingActions)+"," ,
                    JsonConvert.SerializeObject(Logins)+"," ,
                    //JsonConvert.SerializeObject(MemberActions)+"," ,
                    JsonConvert.SerializeObject(Volunteers)+",",
                    JsonConvert.SerializeObject(Locations),
                    "]"}
                );

        public static void LoadDatabase()
        {
            string temp = File.ReadAllText("JsonDB.json");
            Newtonsoft.Json.Linq.JArray[] arr = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JArray[]>
              (File.ReadAllText("JsonDB.json"));
            Categories = arr[0].ToObject<List<Category>>();
            CharityEvents = arr[1].ToObject<List<CharityEvent>>();
            Charities = arr[2].ToObject<List<Charity>>();
            Locations = arr[3].ToObject<List<Location>>();
            ContactInfos = arr[4].ToObject<List<ContactInfo>>();
            //HelpingActions = JsonConvert.DeserializeObject<List<HelpingAction>>(arr[5]);
            Logins =    arr[5].ToObject<List<LogIn>>();
            //MemberActions = JsonConvert.DeserializeObject<List<MemberAction>>(arr[7]);
            Volunteers = arr[6].ToObject<List<Volunteer>>();
            Locations = arr[7].ToObject<List<Location>>();

            JsonDatabase.inintalized = true;
        }

        public static void CreateJsonDatabase(CCEntities db)
        { 
            Categories =db.Categories.ToList();
            CharityEvents=db.CharityEvents.ToList();
            Charities=db.Charities.ToList();
            Locations=db.Locations.ToList();
            ContactInfos=db.ContactInfoes.ToList();
            //HelpingActions=db.HelpingActions.ToList();
            Logins=db.LogIns.ToList();
            //MemberActions=db.MemberActions.ToList();
            Preferences=db.Preferences.ToList();
            Volunteers=db.Volunteers.ToList();

            SaveChanges();
        }
    }
}
