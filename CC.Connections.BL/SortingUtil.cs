using CC.Connections.PL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CC.Connections.BL
{
    public enum SortBy
    {
        NONE,
        CATEGORY,
        HELPING_ACTION,
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
        public static readonly List<int> NONE = new List<int>();
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
                size += CutCharitiesWithFilter(NONE, NONE, new AbsLocation(), null, null, deductable);
            if (filterEvents.Count > 0)
                size += CutEventsByFilter(NONE, NONE, NONE, new AbsLocation(), null, null, null, deductable);
            return size;
        }
        public int Whitelist_Remaining(List<int> category, List<int> helping_action)
        {
            int size = 0;
            if (filterCharities.Count > 0)
                size += CutCharitiesWithFilter(category, helping_action, new AbsLocation(), null, null, null);
            if (filterEvents.Count > 0)
                size += CutEventsByFilter(category, helping_action, NONE, new AbsLocation(), null, null, null, null);
            return size;
        }
        public int Whitelist_Remaining_Events(List<int> charity)
        {
            if (filterEvents.Count > 0)
                return CutEventsByFilter(NONE, NONE, charity, new AbsLocation(), null, null, null, null);
            else
                return 0;
        }
        public int Whitelist_Remaining_Events(List<int> category, List<int> charity, List<int> helping_action)
        {
            if (filterEvents.Count > 0)
                return CutEventsByFilter(category, helping_action, charity, new AbsLocation(), null, null, null, null);
            else
                return 0;
        }
        public int Whitelist_Remaining(AbsLocation location, double distance)
        {
            int size = 0;
            if (filterCharities.Count > 0)
                size += CutCharitiesWithFilter(NONE, NONE, location, distance, null, null);
            if (filterEvents.Count > 0)
                size += CutEventsByFilter(NONE, NONE, NONE, location, distance, null, null, null);
            return size;
        }
        public int Whitelist_Remaining(DateTime? start, DateTime? end)
        {
            if (filterEvents.Count > 0)
                return CutEventsByFilter(NONE, NONE, NONE, new AbsLocation(), null, start, end, null);
            else
                return 0;
        }
        public int Whitelist_Remaining(List<int> category, List<int> helpingAction,
                                        AbsLocation location, double? distance,
                                         bool? deductable)
        {
            int size = 0;
            if (filterCharities.Count > 0)
                size += CutCharitiesWithFilter(category, helpingAction, location, distance, null, deductable);
            if (filterEvents.Count > 0)
                size += CutEventsByFilter(NONE, NONE, NONE, location, distance, null, null, null);
            return size;
        }
        public int Whitelist_Remaining_Events(List<int> category, List<int> helpingAction, List<int> charity,
                                    AbsLocation location, double? distance,
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

        private int CutEventsByFilter(List<int> category, List<int> helpingAction, List<int> charity,
                                        AbsLocation location, double? distance,
                                        DateTime? start, DateTime? end, bool? deductable)
        {
            List<CharityEvent> events = new List<CharityEvent>();
            using (CCEntities dc = new CCEntities())
            {
                if (dc.Charity_Event.ToList().Count != 0)
                    foreach (var c in filterEvents)
                    {
                        bool valid = true;
                        string debugInvalidator = "";

                        //list of IF statements to try to make it invalid
                        foreach (var item in category)
                            if (category.Count != 0 &&
                                new Charity(c.Charity_ID).Category.ID != item)
                            {
                                valid = false;
                                debugInvalidator = "category: " + category;
                                continue;
                            }

                        //if (helpingAction != null &&
                        //    new Charity((int)c.CharityEventCharity_ID).Helping_Action.ID == helpingAction)
                        //    valid = false;
                        foreach (var item in charity)
                            if (charity.Count != 0 &&
                                c.Charity_ID != item)
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
                            new AbsLocation((int)c.Charity_ID).distanceFrom(
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
                            new Charity((int)c.Charity_ID).Deductibility != deductable)
                        {
                            valid = false;
                            debugInvalidator = "deductible: " + deductable;
                            continue;
                        }
                        if (location.ContactInfoCity != null &&
                            new Charity((int)c.Charity_ID).Location.ContactInfoCity != location.ContactInfoCity)
                        {
                            valid = false;
                            debugInvalidator = "location: city " + location;
                            continue;
                        }
                        if (location.ContactInfoState != null &&
                            new Charity((int)c.Charity_ID).Location.ContactInfoState != location.ContactInfoState)
                        {
                            valid = false;
                            debugInvalidator = "location: state " + location;
                            continue;
                        }
                        if (location.ContactInfoZip != null &&
                            new Charity((int)c.Charity_ID).Location.ContactInfoZip != location.ContactInfoZip)
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
                    if (dc.Charity_Event.ToList().Count != 0)
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
                            if (new AbsLocation((int)c.Charity_ID).distanceFrom(
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
                            if (new Charity((int)c.Charity_ID).Location.ContactInfoCity != userPref.Location.ContactInfoCity)
                            {
                                valid = false;
                                debugInvalidator = "location: city " + userPref.Location.ContactInfoCity;
                                continue;
                            }
                            if (new Charity((int)c.Charity_ID).Location.ContactInfoState != userPref.Location.ContactInfoState)
                            {
                                valid = false;
                                debugInvalidator = "location: state " + userPref.Location.ContactInfoState;
                                continue;
                            }
                            if (new Charity((int)c.Charity_ID).Location.ContactInfoZip != userPref.Location.ContactInfoZip)
                            {
                                valid = false;
                                debugInvalidator = "location: zip " + userPref.Location.ContactInfoZip;
                                continue;
                            }

                            //bool categoryValid = false;
                            //foreach (int catid in userPref.Pref.Category_IDList)
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
                            //foreach (int helpid in userPref.Prefered_helping_Actions)
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
            catch (Exception e) { throw e; }
        }

        private int CutCharitiesWithFilter(List<int> category, List<int> helpingAction, AbsLocation location, double? distance,
                                        DateTime? startDate, bool? deductable)
        {
            List<Charity> charities = new List<Charity>();
            using (CCEntities dc = new CCEntities())
            {
                if (dc.Charity_Event.ToList().Count != 0)
                    foreach (var c in dc.Charities.ToList())
                    {
                        bool valid = true;
                        string debugInvalidator = "";

                        //list of IF statements to try to make it invalid
                        foreach (var item in category)
                            if (category.Count != 0 &&
                                new Charity((int)c.Charity_ID).Category.ID != item)
                            {
                                valid = false;
                                debugInvalidator = "category: " + category;
                                continue;
                            }
                        //if (helpingAction != null &&
                        //    new Charity((int)c.Charity_ID).Helping_Action.ID == helpingAction)
                        //    valid = false;
                        if (startDate != null &&
                            AbsContact.fromNumID(c.Charity_ID).DateOfBirth >= startDate)
                            //AbsContact.fromNumID(c.Charity_Contact_ID).DateOfBirth >= startDate)
                        {
                            valid = false;
                            debugInvalidator = "startDate: " + startDate;
                            continue;
                        }
                        if (distance != null &&
                            new AbsLocation((int)c.Charity_ID).distanceFrom(
                                    location) > distance)
                        {
                            valid = false;
                            debugInvalidator = "distance: " + distance;
                            continue;
                        }
                        if (deductable != null &&
                            new Charity((int)c.Charity_ID).Deductibility != deductable)
                        {
                            valid = false;
                            debugInvalidator = "deductable: " + deductable;
                            continue;
                        }
                        if (location.ContactInfoCity != null &&
                            new Charity((int)c.Charity_ID).Location.ContactInfoCity != location.ContactInfoCity)
                        {
                            valid = false;
                            debugInvalidator = "location: city " + location;
                            continue;
                        }
                        if (location.ContactInfoState != null &&
                            new Charity((int)c.Charity_ID).Location.ContactInfoState != location.ContactInfoState)
                        {
                            valid = false;
                            debugInvalidator = "location: state " + location;
                            continue;
                        }
                        if (location.ContactInfoZip != null &&
                            new Charity((int)c.Charity_ID).Location.ContactInfoZip != location.ContactInfoZip)
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
            try
            {
                List<Charity> charities = new List<Charity>();
                using (CCEntities dc = new CCEntities())
                {
                    if (dc.Charity_Event.ToList().Count != 0)
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
                            if (new AbsLocation((int)c.Charity_ID).distanceFrom(
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
                            //foreach (int catid in userPref.Pref.Category_IDList)
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
                            //foreach (int helpid in userPref.Prefered_helping_Actions)
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
            catch (Exception e) { throw e; }
        }
    }
}