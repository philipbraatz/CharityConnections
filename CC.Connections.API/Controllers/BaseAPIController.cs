using CC.Abstract;
using CC.Connections.BL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CC.Connections.API.Controllers
{
    public class BaseAPIController<TList,TEntity,Tdb> : ApiController 
        where TList : BaseList<TEntity,Tdb>
        where TEntity : BaseModel<Tdb>
        where Tdb : class
    {
        // GET: api/{TEntity}/all
        [HttpGet]
        [ActionName("All")]
        public TList Get()
        {
            TList tinstance = (TList)Activator.CreateInstance(typeof(TList), new object[] { });
            try
            {
            tinstance.GetType().GetMethod("LoadAll",new Type[] { }).Invoke(tinstance,null);
            }
            catch (SqlException e)
            {
                throw e.InnerException;
            }
            return tinstance;
        }

        // GET: api/{TEntity}/get/id
        [HttpGet]
        public TEntity Get([FromUri]Guid id)
        {
            try
            {
                TEntity tinstance = (TEntity)Activator.CreateInstance(typeof(TEntity), new object[] { });
                var loadConstructor = typeof(TEntity).GetConstructor(new Type[] { typeof(Guid),typeof(bool) });
                loadConstructor.Invoke(tinstance,new object[] { id,true});
                return tinstance;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // GET: api/{TEntity}/get/id
        [HttpGet]
        [ActionName("GetEmail")]
        public TEntity Get([FromUri]String id)
        {
            id = id.Replace('-','.');
            try
            {
                TEntity tinstance = (TEntity)Activator.CreateInstance(typeof(TEntity), new object[] { });
                var loadConstructor = typeof(TEntity).GetConstructor(new Type[] { typeof(String), typeof(bool) });
                loadConstructor.Invoke(tinstance, new object[] { id, true });
                return tinstance;
            }
            catch (Exception)
            {

                throw;
            }
        }

        // POST: api/{TEntity}
        public void Post([FromBody]TEntity tinstance)
        {
            tinstance.GetType().GetMethod("Insert", new Type[] { }).Invoke(tinstance, null);
        }
        // POST: api/{TEntity}
        public void Post([FromBody]TEntity tinstance,Password password)
        {
            tinstance.GetType().GetMethod("Insert", new Type[] { typeof(Password)}).Invoke(tinstance,new object[] { 
                password//parameters
            });
        }
        
        // PUT: api/{TEntity}/5
        public void Put(Guid id, [FromBody]TEntity tinstance)
        {
            tinstance.GetType().GetMethod("Update", new Type[] { }).Invoke(tinstance, null);
        }
       // PUT: api/{TEntity}/5
       public void Put(Guid id, [FromBody]TEntity tinstance, Password password)
       {
           tinstance.GetType().GetMethod("Update", new Type[] { typeof(Password) }).Invoke(tinstance, new object[] {
               password//parameters
           });
       }
        
        // DELETE: api/{TEntity}/5
        public void Delete(object id)
        {
            TEntity tinstance = (TEntity)Activator.CreateInstance(id.GetType(), new object[] { });
            tinstance.GetType().GetMethod("LoadId", new Type[] { id.GetType() }).Invoke(tinstance, new object[] { id });
            tinstance.GetType().GetMethod("Delete", new Type[] { }).Invoke(tinstance, null);
        }
        
        public void Delete(object id, Password password)
        {
            TEntity tinstance = (TEntity)Activator.CreateInstance(id.GetType(), new object[] { });
            tinstance.GetType().GetMethod("LoadId", new Type[] { id.GetType() }).Invoke(tinstance, new object[] { id });
            tinstance.GetType().GetMethod("Delete", new Type[] { typeof(Password) }).Invoke(tinstance, new object[] {
                password//parameters
            });
        }
    }
}
