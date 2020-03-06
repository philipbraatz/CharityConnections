using CC.Connections.BL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CC.Connections.API.Controllers
{
    public class CategoryController : ApiController
    {
        // GET: api/Category
        public CategoryCollection Get()
        {
            CategoryCollection Categorys = new CategoryCollection();
            Categorys.LoadAll();
            return Categorys;
        }

        // GET: api/Category/5
        public Category Get(Guid id)
        {
            Category Category = new Category { ID = id };
            Category.LoadId(id);
            return Category;
        }

        // POST: api/Category
        public void Post([FromBody]Category Category)
        {
            Category.Insert();
        }

        // PUT: api/Category/5
        public void Put(Guid id, [FromBody]Category Category)
        {
            Category.Update();
        }

        // DELETE: api/Category/5
        public void Delete(Guid id)
        {
            Category Category = new Category { ID = id };
            Category.Delete();
        }
    }
}
