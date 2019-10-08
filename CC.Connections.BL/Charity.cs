using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;
using System.ComponentModel;



// setting the charity class with a inherit from contactinfo

namespace CC.Connections.BL
{
    public class Charity : ContactInfo
    {
        public int ID { get; set; }
        public int Contact_ID { get; set; }
        [DisplayName("Federal Tax ID#")]
        public string EIN { get; set; }
        [DisplayName("Is Deductible")]
        public bool Deductibility { get; set; }
        [DisplayName("Charity Web Site")]
        public string URL { get; set; }
        [DisplayName("Charities Cause")]
        public string Cause { get; set; }
        public int Category_ID { get; set; }
        public int LocationID { get; set; }
        [DisplayName("Charities Requirements")]
        public string Requirements { get; set; }
        
        public string ContactName;
        public string CategoryName;
        public string LocationZip;
        public string LocationCityState;

        internal static bool Exists(DBconnections dc, int id)
        {
            return dc.Charities.Where(c => c.Charity_ID == id).FirstOrDefault() != null;
        }

        public Charity()
        {
            Clear();
        }

        //only clears MEMBER values
        private new void Clear()
        {
            Contact_ID = 0;
            EIN = string.Empty;
            Deductibility = false;
            URL = string.Empty;
            Cause = string.Empty;
            Category_ID = 0;
            LocationID = 0;
            Requirements = string.Empty;
            ContactName = string.Empty;
            CategoryName = string.Empty;
            LocationZip = string.Empty;
            LocationCityState = string.Empty;
        }

        public void LoadContactName(int contactID)
        {
            this.Contact_ID = contactID;
            base.LoadId();
            ContactName = this.FullName;
        }    
        
        public void LoadLocation(int locationID)
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    Clear();

                    PL.Location locID = dc.Locations.Where(c => c.Location_ID == locationID).FirstOrDefault();
                    if (locID == null)
                        throw new Exception("Location ID " + locationID + " does not have a location that exist");
                    LocationZip = locID.ContactInfoZip;
                    LocationCityState = locID.ContactInfoCity + ", " + locID.ContactInfoState;
                }
            }
            catch (Exception e)
            {
                throw e;
            }

        }
         

        public void LoadCategoryName(int categoryID)
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    Clear();

                    PL.Category catID = dc.Categories.Where(c => c.Category_ID == categoryID).FirstOrDefault();
                    if (catID == null)
                        throw new Exception("Category ID " + categoryID + " does not have a category that exist");
                    CategoryName = catID.Category_Desc;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        internal static List<int> LoadMembersIdList(DBconnections dc, int member_ID)
        {
            throw new NotImplementedException();
        }

        public void LoadCharityId(int iD)
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    PL.Charity entry = dc.Charities.Where(c => c.Charity_ID == this.ID).FirstOrDefault();
                    if (entry != null)
                        this.ID = entry.Charity_ID;
                    else
                        throw new Exception("Charity ID " + iD + " does not have a Charity associated with it");

                    Clear();

                    this.Contact_ID = entry.Charity_Contact_ID.Value;
                    this.EIN = entry.Charity_EIN;
                    this.Deductibility = entry.Charity_Deductibility.Value;
                    this.URL = entry.Charity_URL;
                    this.Cause = entry.Charity_Cause;
                    this.ContactInfo_Email = entry.Charity_Email;
                    this.Category_ID = entry.Charity_Category_ID.Value;
                    this.LocationID = entry.Location_ID.Value;
                    this.Requirements = entry.Charity_Requirements;

                    LoadCategoryName(entry.Charity_Category_ID.Value);
                    LoadContactName(entry.Charity_Contact_ID.Value);
                    LoadLocation(entry.Location_ID.Value);                    
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public int InsertCharity()
        {
            {
                base.Insert();

                try
                {
                    //if (ID == string.Empty)
                    //    throw new Exception("Description cannot be empty");
                    using (DBconnections dc = new DBconnections())
                    {
                        //double check ID before insert
                        if (dc.Charities.Where(c => c.Charity_ID == ID) != null)
                            ID = dc.Charities.Max(c => c.Charity_ID) + 1;
                        
                        PL.Charity entry = new PL.Charity
                        {
                            Charity_ID = ID,
                            Charity_Contact_ID = Contact_ID,
                            Charity_EIN = EIN,
                            Charity_Deductibility = Deductibility,
                            Charity_URL = URL,
                            Charity_Cause = Cause,
                            Charity_Email = ContactInfo_Email,
                            Charity_Category_ID = Category_ID,
                            Location_ID = LocationID,
                            Charity_Requirements = Requirements
                        };
                        dc.Charities.Add(entry);
                        
                        return dc.SaveChanges();
                    }
                }
                catch (Exception e) { throw e; }
            }
        }

        public int UpdateCharity()
        {
            try
            {           
                using (DBconnections dc = new DBconnections())
                {
                    PL.Charity entry = dc.Charities.Where(c => c.Charity_ID == this.ID).FirstOrDefault();
                    base.Update();
                    
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        public int DeleteCharity()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {                  
                    base.Delete();

                    dc.Charities.Remove(dc.Charities.Where(c => c.Charity_ID == ID).FirstOrDefault());
                    
                    Clear();
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }

        internal static Charity fromCharityID(int? charity_ID)
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    Charity retChar = new Charity();
                    PL.Charity entryCharity = dc.Charities.FirstOrDefault(c => c.Charity_ID == charity_ID);
                    if (entryCharity == null)
                        throw new Exception("Charity does not exist: Charity ID '" + retChar.ID + "'"); ;

                    retChar.ID = entryCharity.Charity_ID;
                    retChar.Category_ID = entryCharity.Charity_Category_ID.Value;
                    retChar.Contact_ID = entryCharity.Charity_Contact_ID.Value;
                    retChar.Deductibility = entryCharity.Charity_Deductibility.Value;
                    retChar.EIN = entryCharity.Charity_EIN;
                    retChar.ContactInfo_Email = entryCharity.Charity_Email;
                    retChar.LocationID = entryCharity.Location_ID.Value;
                    retChar.URL = entryCharity.Charity_URL;
                    retChar.Requirements = entryCharity.Charity_Requirements;
                    retChar.Cause = entryCharity.Charity_Cause;
                    return retChar;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }    

    public class CharityList :
        List<Charity>
    {
        public string locationCity;
        public string locationState;

        public void LoadList()
        {
            try
            {
                using (DBconnections dc = new DBconnections())
                {
                    if(dc.Charities.ToList().Count != 0)
                        dc.Charities.ToList().ForEach(c =>
                        {
                            Charity newCharity = Charity.fromCharityID(c.Charity_ID);
                            newCharity.ID = c.Charity_ID;
                            newCharity.EIN = c.Charity_EIN;
                            newCharity.ContactInfo_Email = c.Charity_Email;
                            newCharity.URL = c.Charity_URL;
                            if (c.Charity_Deductibility == false)
                            {
                                newCharity.Deductibility = false ;
                            }
                            else
                            { 
                                newCharity.Deductibility = true;
                            }
                            newCharity.Cause = c.Charity_Cause;
                            newCharity.Requirements = c.Charity_Requirements;
                            
                            newCharity.CategoryName = dc.Categories.FirstOrDefault(f => f.Category_ID == c.Charity_Category_ID).Category_Desc;

                            newCharity.LoadLocation(c.Location_ID.Value);

                            newCharity.LoadContactName(c.Charity_Contact_ID.Value);

                            
                            this.Add(newCharity);
                        });
                }
            }
            catch (Exception e) { throw e; }
        }
        public void load()
        {
            throw new NotImplementedException();
        }     
    }
}
