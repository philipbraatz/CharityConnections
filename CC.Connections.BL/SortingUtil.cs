using Doorfail.Connections.PL;
using System;
using System.Collections.Generic;
using System.Linq;

#pragma warning disable CA1707 // Identifiers should not contain underscores
namespace Doorfail.Connections.BL
{
    public enum SortBy
    {
        NONE,
        CATEGORY,
        HelpingAction,
        DEDUCTIBILITY,
        LOCATION_CITY,
        LOCATION_STATE,
        LOCATION_ZIP,
        RADIUS_DISTANCE,

        //Events only
        CHARITY,
        DATE_RANGE,
    }

    public class Filterer
    {
        public List<Charity> filterCharities = new List<Charity>();
        public List<CharityEvent> filterEvents = new List<CharityEvent>();
        public static readonly List<Guid> NONE_GUID = new List<Guid>();
        public static readonly List<string> NONE_STR = new List<string>();
        public void FillFilter(List<Charity> charities) { filterCharities = charities; }
        public void FillFilter(List<CharityEvent> charities) { filterEvents = charities; }

        public int Whitelist_from_Preferences(Volunteer member_preferences)
        {
            int size = 0;
            if (filterCharities.Count > 0)
                size += CutCharitiesByPreferences(member_preferences);
            if (filterEvents.Count > 0)
                size += CutEventByPreferences(member_preferences);
            return size;
        }

        public int Whitelist_Remaining(bool deductable)
        {
            int size = 0;
            if (filterCharities.Count > 0)
                size += CutCharitiesWithFilter(NONE_GUID, NONE_GUID, new Location(), null, null, deductable);
            if (filterEvents.Count > 0)
                size += CutEventsByFilter(NONE_GUID, NONE_GUID, NONE_STR, new Location(), null, null, null, deductable);
            return size;
        }
        public int Whitelist_Remaining(List<Guid> category, List<Guid> HelpingAction)
        {
            int size = 0;
            if (filterCharities.Count > 0)
                size += CutCharitiesWithFilter(category, HelpingAction, new Location(), null, null, null);
            if (filterEvents.Count > 0)
                size += CutEventsByFilter(category, HelpingAction, NONE_STR, new Location(), null, null, null, null);
            return size;
        }
        public int Whitelist_Remaining_Events(List<String> charity)
        {
            if (filterEvents.Count > 0)
                return CutEventsByFilter(NONE_GUID, NONE_GUID, charity, new Location(), null, null, null, null);
            else
                return 0;
        }
        public int Whitelist_Remaining_Events(List<Guid> category, List<String> charity, List<Guid> HelpingAction)
        {
            if (filterEvents.Count > 0)
                return CutEventsByFilter(category, HelpingAction, charity, new Location(), null, null, null, null);
            else
                return 0;
        }
        public int Whitelist_Remaining(Location location, double distance)
        {
            int size = 0;
            if (filterCharities.Count > 0)
                size += CutCharitiesWithFilter(NONE_GUID, NONE_GUID, location, distance, null, null);
            if (filterEvents.Count > 0)
                size += CutEventsByFilter(NONE_GUID, NONE_GUID, NONE_STR, location, distance, null, null, null);
            return size;
        }
        public int Whitelist_Remaining(DateTime? start, DateTime? end)
        {
            if (filterEvents.Count > 0)
                return CutEventsByFilter(NONE_GUID, NONE_GUID, NONE_STR, new Location(), null, start, end, null);
            else
                return 0;
        }
        public int Whitelist_Remaining(List<Guid> category, List<Guid> helpingAction,
                                        Location location, double? distance,
                                         bool? deductable)
        {
            int size = 0;
            if (filterCharities.Count > 0)
                size += CutCharitiesWithFilter(category, helpingAction, location, distance, null, deductable);
            if (filterEvents.Count > 0)
                size += CutEventsByFilter(NONE_GUID, NONE_GUID, NONE_STR, location, distance, null, null, null);
            return size;
        }
        public int Whitelist_Remaining_Events(List<Guid> category, List<Guid> helpingAction, List<string> charity,
                                    Location location, double? distance,
                                    DateTime? start, DateTime? end, bool? deductable)
        {
            return CutEventsByFilter(category, helpingAction, charity, location, distance, start, end, deductable);
        }

        public List<Charity> GetRemainingCharities()
        {
            return filterCharities;
        }
        public List<CharityEvent> GetRemainingEvents()
        {
            return filterEvents;
        }

        private int CutEventsByFilter(List<Guid> category, List<Guid> helpingAction, List<String> charity,
                                        Location location, double? distance,
                                        DateTime? start, DateTime? end, bool? deductable)
        {
            List<CharityEvent> events = new List<CharityEvent>();
            using (CCEntities dc = new CCEntities())
            {
                if (dc.CharityEvents.ToList().Count != 0)
                    foreach (var c in filterEvents)
                    {
                        bool valid = true;
                        string debugInvalidator = "";

                        //list of IF statements to try to make it invalid
                        foreach (var item in category)
                            if (category.Count != 0 &&
                                new Charity(c.CharityEmail,true).Category.ID != item)
                            {
                                valid = false;
                                debugInvalidator = "category: " + category;
                                continue;
                            }

                        //if (helpingAction != null &&
                        //    new Charity((int)c.CharityEventCharity_ID).HelpingAction.ID == helpingAction)
                        //    valid = false;
                        foreach (var item in charity)
                            if (charity.Count != 0 &&
                                c.CharityEmail != item)
                            {
                                valid = false;
                                debugInvalidator = "charity: " + charity;
                                continue;
                            }
                        if (start != null &&
                            c.StartDate >= start)
                        {
                            valid = false;
                            debugInvalidator = "start: " + start;
                            continue;
                        }
                        if (distance != null &&
                            c.Location.distanceFrom(
                                    location) > distance)
                        {
                            valid = false;
                            debugInvalidator = "distance: " + distance;
                            continue;
                        }
                        if (end != null &&
                            c.EndDate <= end)
                        {
                            valid = false;
                            debugInvalidator = "end: " + end;
                            continue;
                        }
                        if (deductable != null &&
                            new Charity(c.CharityEmail,true).Deductibility != deductable)
                        {
                            valid = false;
                            debugInvalidator = "deductible: " + deductable;
                            continue;
                        }
                        if (location.City != null &&
                            new Charity(c.CharityEmail,true).Location.City != location.City)
                        {
                            valid = false;
                            debugInvalidator = "location: city " + location;
                            continue;
                        }
                        if (location.State != null &&
                            new Charity(c.CharityEmail,true).Location.State != location.State)
                        {
                            valid = false;
                            debugInvalidator = "location: state " + location;
                            continue;
                        }
                        if (location.Zip != null &&
                            new Charity(c.CharityEmail,true).Location.Zip != location.Zip)
                        {
                            valid = false;
                            debugInvalidator = "location: zip " + location;
                            continue;
                        }

                        if (valid)
                        {
                            events.Add(c);
                            debugInvalidator = "VALID";
                        }
                    }
            }
            filterEvents = events;
            return filterEvents.Count;
        }

        //TODO add more user preference properties ------------------------------------------------
        public int CutEventByPreferences(Volunteer userPref)
        {
            try
            {
                List<CharityEvent> events = new List<CharityEvent>();
                using (CCEntities dc = new CCEntities())
                {
                    if (dc.CharityEvents.ToList().Count != 0)
                        foreach (var c in filterEvents)
                        {
                            bool valid = true;
                            string debugInvalidator = "";

                            ///list of IF statements to try to make it invalid
                            //if (userPref.Pref.StartDate != null &&
                            //    c.CharityEventStartDate >= userPref.Pref.StartDate)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "start: " + userPref.Pref.StartDate;
                            //    continue;
                            //}
                            if (c.Location.distanceFrom(
                                        userPref.Location) > (double)userPref.Pref.Distance)
                            {
                                valid = false;
                                debugInvalidator = "distance: " + userPref.Pref.Distance;
                                continue;
                            }
                            //if (c.CharityEventEndDate <= userPref.Pref.EndDate)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "end: " + userPref.Pref.EndDate;
                            //    continue;
                            //}
                            //if (new Charity((int)c.CharityEventCharity_ID).Deductibility != userPref.Pref.Deductable)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "deductable: " + userPref.Pref.Deductable;
                            //    continue;
                            //}
                            if (new Charity(c.CharityEmail,true).Location.City != userPref.Location.City)
                            {
                                valid = false;
                                debugInvalidator = "location: city " + userPref.Location.City;
                                continue;
                            }
                            if (new Charity(c.CharityEmail,true).Location.State != userPref.Location.State)
                            {
                                valid = false;
                                debugInvalidator = "location: state " + userPref.Location.State;
                                continue;
                            }
                            if (new Charity(c.CharityEmail,true).Location.Zip != userPref.Location.Zip)
                            {
                                valid = false;
                                debugInvalidator = "location: zip " + userPref.Location.Zip;
                                continue;
                            }

                            //bool categoryValid = false;
                            //foreach (int catid in userPref.Pref.CategoryIDList)
                            //    if (new Charity((int)c.CharityEventCharity_ID).Category.ID == catid)
                            //    {
                            //        categoryValid = true;
                            //        break;
                            //    }
                            //if(!categoryValid)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "Category: Member is filtering out Category "+ new Charity((int)c.CharityEventCharity_ID).Category.ID;
                            //    continue;
                            //}

                            //bool charityValid = false;
                            //foreach (int charid in userPref.Pref.Charity_IDList)
                            //    if (c.CharityEventCharity_ID == charid)
                            //    {
                            //        charityValid = true;
                            //        debugInvalidator = "charity: " + charid;
                            //        break;
                            //    }
                            //if(!charityValid)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "Charity: Member is filtering out Charity "+ c.CharityEventCharity_ID;
                            //    continue;
                            //}

                            //bool helpingValid = false;
                            //foreach (int helpid in userPref.Prefered_HelpingActions)
                            //    if (c.CharityEventHelpingAction == helpid)
                            //    {
                            //        helpingValid = true;
                            //        break;
                            //    }
                            //if(!helpingValid)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "Helping Action: Member is filtering out Helping Action " + c.CharityEventHelpingAction;
                            //    continue;
                            //}

                            if (valid)
                            {
                                events.Add(c);
                                debugInvalidator = "VALID";
                            }
                        }
                }
                filterEvents = events;
                return filterEvents.Count;
            }
            catch (Exception) { throw; }
        }

        private int CutCharitiesWithFilter(List<Guid> category, List<Guid> helpingAction, Location location, double? distance,
                                        DateTime? startDate, bool? deductable)
        {
            List<Charity> charities = new List<Charity>();
            using (CCEntities dc = new CCEntities())
            {
                if (dc.CharityEvents.ToList().Count != 0)
                    foreach (var c in dc.Charities.ToList())
                    {
                        bool valid = true;
                        string debugInvalidator = "";

                        //list of IF statements to try to make it invalid
                        foreach (var item in category)
                            if (category.Count != 0 &&
                                new Charity((string)c.CharityEmail,true).Category.ID != item)
                            {
                                valid = false;
                                debugInvalidator = "category: " + category;
                                continue;
                            }
                        //if (helpingAction != null &&
                        //    new Charity((int)c.Charity_ID).HelpingAction.ID == helpingAction)
                        //    valid = false;
                        if (startDate != null &&
                            new Contact(c.CharityEmail).DateOfBirth >= startDate)
                            //AbsContact.fromNumID(c.Charity_Contact_ID).DateOfBirth >= startDate)
                        {
                            valid = false;
                            debugInvalidator = "startDate: " + startDate;
                            continue;
                        }
                        if (distance != null &&
                            new Location((Guid)c.LocationID).distanceFrom(
                                    location) > distance)
                        {
                            valid = false;
                            debugInvalidator = "distance: " + distance;
                            continue;
                        }
                        if (deductable != null &&
                            new Charity((string)c.CharityEmail, true).Deductibility != deductable)
                        {
                            valid = false;
                            debugInvalidator = "deductable: " + deductable;
                            continue;
                        }
                        if (location.City != null &&
                            new Charity((string)c.CharityEmail, true).Location.City != location.City)
                        {
                            valid = false;
                            debugInvalidator = "location: city " + location;
                            continue;
                        }
                        if (location.State != null &&
                            new Charity((string)c.CharityEmail, true).Location.State != location.State)
                        {
                            valid = false;
                            debugInvalidator = "location: state " + location;
                            continue;
                        }
                        if (location.Zip != null &&
                            new Charity((string)c.CharityEmail, true).Location.Zip != location.Zip)
                        {
                            valid = false;
                            debugInvalidator = "location: zip " + location;
                            continue;
                        }

                        if (valid)
                        {
                            charities.Add(new Charity(c));
                            debugInvalidator = "VALID";
                        }
                    }
            }
            filterCharities = charities;
            return filterCharities.Count;
        }

        public int CutCharitiesByPreferences(Volunteer userPref)
        {
            if (userPref == null)
                throw new ArgumentNullException(nameof(userPref));
            try
            {
                List<Charity> charities = new List<Charity>();
                using (CCEntities dc = new CCEntities())
                {
                    if (dc.CharityEvents.ToList().Count != 0)
                        foreach (var c in dc.Charities.ToList())
                        {
                            bool valid = true;
                            string debugInvalidator = "";

                            ///list of IF statements to try to make it invalid
                            //if (userPref.Pref.StartDate != null &&
                            //    c.CharityEventStartDate >= userPref.Pref.StartDate)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "start: " + userPref.Pref.StartDate;
                            //    continue;
                            //}
                            if (new Location((Guid)c.LocationID).distanceFrom(
                                        userPref.Location) > (double)userPref.Pref.Distance)
                            {
                                valid = false;
                                debugInvalidator = "distance: " + userPref.Pref.Distance;
                                continue;
                            }
                            //if (c.CharityEventEndDate <= userPref.Pref.EndDate)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "end: " + userPref.Pref.EndDate;
                            //    continue;
                            //}
                            //if (new Charity((int)c.CharityEventCharity_ID).Deductibility != userPref.Pref.Deductable)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "deductable: " + userPref.Pref.Deductable;
                            //    continue;
                            //}

                            //TODO put these result first
                            //if (new Charity((int)c.CharityEventCharity_ID).Location.ContactInfoState != userPref.Location.ContactInfoState)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "location: state " + userPref.Location.ContactInfoState;
                            //    continue;
                            //}
                            //if (new Charity((int)c.CharityEventCharity_ID).Location.ContactInfoZip != userPref.Location.ContactInfoZip)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "location: zip " + userPref.Location.ContactInfoZip;
                            //    continue;
                            //}

                            //bool categoryValid = false;
                            //foreach (int catid in userPref.Pref.CategoryIDList)
                            //    if (new Charity((int)c.CharityEventCharity_ID).Category.ID == catid)
                            //    {
                            //        categoryValid = true;
                            //        break;
                            //    }
                            //if(!categoryValid)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "Category: Member is filtering out Category "+ new Charity((int)c.CharityEventCharity_ID).Category.ID;
                            //    continue;
                            //}

                            //bool charityValid = false;
                            //foreach (int charid in userPref.Pref.Charity_IDList)
                            //    if (c.CharityEventCharity_ID == charid)
                            //    {
                            //        charityValid = true;
                            //        debugInvalidator = "charity: " + charid;
                            //        break;
                            //    }
                            //if(!charityValid)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "Charity: Member is filtering out Charity "+ c.CharityEventCharity_ID;
                            //    continue;
                            //}

                            //bool helpingValid = false;
                            //foreach (int helpid in userPref.Prefered_HelpingActions)
                            //    if (c.CharityEventHelpingAction == helpid)
                            //    {
                            //        helpingValid = true;
                            //        break;
                            //    }
                            //if(!helpingValid)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "Helping Action: Member is filtering out Helping Action " + c.CharityEventHelpingAction;
                            //    continue;
                            //}

                            //if(AbsContact.fromNumID(c.Charity_Contact_ID).DateOfBirth > userPref.Pref.CharityStartDate)
                            //{
                            //    valid = false;
                            //    debugInvalidator = "Charity Start Date: Member is filtering out Start Date " + AbsContact.fromNumID(c.Charity_Contact_ID).DateOfBirth;
                            //    continue;
                            //}

                            if (valid)
                            {
                                charities.Add(new Charity(c));
                                debugInvalidator = "VALID";
                            }
                        }
                }
                filterCharities = charities;
                return filterCharities.Count;
            }
            catch (Exception) { throw; }
        }
    }
}
#pragma warning restore CA1707 // Identifiers should not contain underscores