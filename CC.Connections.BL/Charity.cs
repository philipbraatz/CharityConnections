using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;



// setting the charity class with a inherit from contactInfo

namespace CC.Connections.BL
{
    public class Charity : ColumnEntry<PL.Charities>
    {
        //id
        public new int ID
        {
            get { return (int)base.ID; }
            set { base.ID = value; }
        }

        [DisplayName("Charity Email")]
        public string Charity_Email
        {
            get { return (string)base.getProperty("Charity_Email"); }
            set { setProperty("Charity_Email", value); }
        }

        [DisplayName("Charity Name")]
        public string Charity_Name
        {
            get { return (string)base.getProperty("Charity_Name"); }
            set { setProperty("Charity_Name", value); }
        }

        [DisplayName("Federal Tax ID# (EIN)")]
        public string Charity_EIN
        {
            get { return (string)base.getProperty("Charity_EIN"); }
            set { setProperty("Charity_EIN", value); }
        }

        [DisplayName("Is Deductible")]
        public bool Charity_Deductibility
        {
            get { return false; }//{ return (bool)base.getProperty("Charity_Deductibility"); }//
            set { setProperty("Charity_Deductibility", value); }
        }

        [DisplayName("Charity Web Site")]
        public string Charity_URL
        {
            get { return (string)base.getProperty("Charity_URL"); }
            set { setProperty("Charity_URL", value); }
        }

        [DisplayName("Charities Cause")]
        public string Charity_Cause
        {
            get { return (string)base.getProperty("Charity_Cause"); }
            set { setProperty("Charity_Cause", value); }
        }
        [DisplayName("Charities Requirements")]
        public string Charity_Requirements
        {
            get { return (string)base.getProperty("Charity_Requirements"); }
            set { setProperty("Charity_Requirements", value); }
        }

        public AbsCategory Category { get; set; }//never delete
        public AbsLocation Location { get; set; }//delete when removed

        internal static bool Exists(CCEntities dc, int id)
        {
            return dc.Charities.Where(c => c.Charity_ID == id).FirstOrDefault() != null;
        }

        public Charity() { }
        //does not verify password
        public Charity(Password p) : this(p.email)
        {
        }
        public Charity(PL.Charities entry) : base(entry)
        {
            this.Category = new AbsCategory((int)entry.Charity_Category_ID);
            this.Location = new AbsLocation((int)entry.Location_ID);

        }
        public Charity(int id) : base(new CCEntities().Charities, id)
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    PL.Charities charityPL = dc.Charities.Where(c => c.Charity_ID == id).FirstOrDefault();
                    this.Category = new AbsCategory((int)charityPL.Charity_Category_ID);
                    this.Location = new AbsLocation((int)charityPL.Location_ID);
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public void setCharityInfo(Charity charityInfo)
        {
            this.Charity_Name = charityInfo.Charity_Name ?? "";
            this.Charity_Email = charityInfo.Charity_Email ?? "";
            this.Charity_Cause = charityInfo.Charity_Cause ?? "";
            this.Charity_URL = charityInfo.Charity_URL ?? "";
            this.Charity_Deductibility = charityInfo.Charity_Deductibility;
            this.Charity_EIN = charityInfo.Charity_EIN;
            this.Charity_Requirements = charityInfo.Charity_Requirements;
        }
        public void setCharityInfo(PL.Charities charityInfo)
        {
            this.ID = charityInfo.Charity_ID;
            this.Charity_Name = charityInfo.Charity_Name ?? "";
            this.Charity_Email = charityInfo.Charity_Email ?? "";
            this.Charity_Cause = charityInfo.Charity_Cause ?? "";
            this.Charity_URL = charityInfo.Charity_URL ?? "";
            this.Charity_Deductibility = (bool)charityInfo.Charity_Deductibility;
            this.Charity_EIN = charityInfo.Charity_EIN;
            this.Charity_Requirements = charityInfo.Charity_Requirements;
        }

        public Charity(string charityEmail, string password = "", bool hashed = false, bool debug = false)
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    Clear();

                    //set new id for preferred lists
                    if (dc.Charities.ToList().Count > 0)
                        ID = dc.Charities.Max(c => c.Charity_ID) + 1;
                    else
                        ID = 0;//first entry

                    //try to load existing ID
                    PL.Charities charityPL = dc.Charities.Where(c => c.Charity_Email == charityEmail).FirstOrDefault();
                    if (charityPL != null)
                        setCharityInfo(charityPL);
                    else
                        Clear();//new Charity
                    Password newPassword = new Password(charityEmail, password, MemberType.CHARITY, false);
                    newPassword.Insert();

                    this.Category = new AbsCategory((int)charityPL.Charity_Category_ID);
                    this.Location = new AbsLocation((int)charityPL.Location_ID);
                }
            }
            catch (Exception e)
            {
                if (e.Message != "The underlying provider failed on Open.")
                    throw e;
                else
                    throw e.InnerException;//database error

            }
        }


        //turns a PL class into a BL equivelent example: 
        //Category pl = new Category();
        //...
        //AbsCategory bl = (AbsCategory)pl
        public static implicit operator Charity(PL.Charities entry)
        { return new Charity(entry); }

        public void LoadId()
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    base.LoadId(dc.Charities);

                    PL.Charities entry = dc.Charities.Where(c => c.Charity_ID == this.ID).FirstOrDefault();
                    this.Category = new AbsCategory(entry.Charity_Category_ID.Value);
                    this.Location = new AbsLocation(entry.Location_ID.Value);
                }
            }
            catch (Exception e)
            { throw e; }
        }
        public void LoadId(int id)
        {
            ID = id;
            LoadId();
        }
        public int Insert(Password password)
        {

            using (CCEntities dc = new CCEntities())
            {
                return base.Insert(dc, dc.Charities) +
                    this.Location.Insert() +
                    (password.Insert() ? 1 : 0);
            }
        }
        public int Update(Password password)
        {
            using (CCEntities dc = new CCEntities())
            {
                return base.Update(dc, dc.Charities) +
                    this.Location.Update() +
                    password.Update();
            }
        }
        public int Delete(Password password)
        {
            using (CCEntities dc = new CCEntities())
            {
                //dc.Categories.Remove(this);
                //return dc.SaveChanges();
                return base.Delete(dc, dc.Charities) +
                    this.Location.Delete() +
                    password.Delete();
            }
        }

        internal static Charity fromCharityID(int charity_ID)
        {
            try
            {
                using (CCEntities dc = new CCEntities())
                {
                    return new Charity(dc.Charities.FirstOrDefault(c => c.Charity_ID == charity_ID));
                }
            }
            catch (Exception e) { throw e; }
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
                using (CCEntities dc = new CCEntities())
                {
                    if (dc.Charities.ToList().Count != 0)
                        dc.Charities.ToList().ForEach(c =>
                        {
                            this.Add(new Charity(c.Charity_ID));
                        });
                }
            }
            catch (Exception e) { throw e; }
        }
        public void load()
        {
            throw new NotImplementedException();
        }

        public static int getCount()
        {
            using (CCEntities dc = new CCEntities())
            {
                return dc.Charities.Count();
            }
        }
    }
}