using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    interface CRUDclass { }

    class CategoryCRUD : PL.Category
    {
        protected bool Exists(DBconnections dc)
        {
            return dc.Categories.Where(c => c.Category_ID == Category_ID).FirstOrDefault() != null;
        }

        public int Insert()
        {
            try
            {
                //if (Description == string.Empty)
                //    throw new Exception("Description cannot be empty");
                using (DBconnections dc = new DBconnections())
                {
                    if (dc.Categories.ToList().Count > 0)
                        Category_ID = dc.Categories.Max(c => c.Category_ID) + 1;//unique id
                    else
                        Category_ID = 0;

                    PL.Category entry = new PL.Category
                    {
                        Category_ID = Category_ID,
                        Category_Desc = this.Category_Desc
                    };

                    dc.Categories.Add(entry);
                    return dc.SaveChanges();
                }
            }
            catch (Exception e) { throw e; }
        }
    }
}
