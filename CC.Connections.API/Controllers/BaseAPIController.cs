using CC.Abstract;
using CC.Connections.BL;
using System;
using System.Data.SqlClient;
using System.Net;
using System.Web.Http;

namespace CC.Connections.API.Controllers
{
    public class BaseAPIController<TList,TEntity,Tdb> : ApiController 
        where TList : BaseList<TEntity,Tdb>
        where TEntity : BaseModel<Tdb>
        where Tdb : class
    {
        // GET: api/{TEntity}/all
        /// <summary>
        /// Gets all TEntity
        /// </summary>
        /// <param></param>
        /// <returns>TList</returns>
        [HttpGet]
        [ActionName("All")]
        public TList Get()
        {
            TList tinstance = (TList)Activator.CreateInstance(typeof(TList), new object[] { });
            try
            {
            tinstance.GetType().GetMethod("LoadAll",new Type[] { }).Invoke(tinstance,null);
            }
            catch (System.Reflection.TargetInvocationException e)
            {
                if (e.InnerException.GetType() == typeof(System.Data.Entity.Core.EntityException))
                    throw e.InnerException.InnerException;
                else
                    throw e.InnerException;

            }
            catch (SqlException e)
            {
                throw e.InnerException;
            }
            return tinstance;
        }

        // GET: api/{TEntity}/get/id
        /// <summary>
        /// Gets a TEntity by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TEntity</returns>
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
        /// <summary>
        /// Get Tenitity by email
        /// </summary>
        /// <param name="id"></param>
        /// <returns>TEntity</returns>
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
        /// <summary>
        /// Insert new TEntity
        /// </summary>
        /// <param name="tinstance"></param>
        public void Post([FromBody]TEntity tinstance)
        {
            tinstance.GetType().GetMethod("Insert", new Type[] { }).Invoke(tinstance, null);
        }
        // POST: api/{TEntity}
        /// <summary>
        /// Insert new TEntity with password
        /// </summary>
        /// <param name="tinstance"></param>
        /// <param name="password"></param>
        public void Post([FromBody]TEntity tinstance,Password password)
        {
            tinstance.GetType().GetMethod("Insert", new Type[] { typeof(Password)}).Invoke(tinstance,new object[] { 
                password//parameters
            });
        }
        
        // PUT: api/{TEntity}/5
        /// <summary>
        /// Update Existing TEntity from existing instance
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tinstance"></param>
        public void Put(Guid id, [FromBody]TEntity tinstance)
        {
            tinstance.GetType().GetMethod("Update", new Type[] { }).Invoke(tinstance, null);
        }
        // PUT: api/{TEntity}/5
        /// <summary>
        /// Update Existing TEntity from existing instance requiring password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tinstance"></param>
        /// <param name="password"></param>
        public void Put(Guid id, [FromBody]TEntity tinstance, Password password)
       {
           tinstance.GetType().GetMethod("Update", new Type[] { typeof(Password) }).Invoke(tinstance, new object[] {
               password//parameters
           });
       }
        
        // DELETE: api/{TEntity}/5
        /// <summary>
        /// Delete Existing TEntity from id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(object id)
        {
            TEntity tinstance = (TEntity)Activator.CreateInstance(id.GetType(), new object[] { });
            tinstance.GetType().GetMethod("LoadId", new Type[] { id.GetType() }).Invoke(tinstance, new object[] { id });
            tinstance.GetType().GetMethod("Delete", new Type[] { }).Invoke(tinstance, null);
        }
        
        /// <summary>
        /// Delete Existing TEntity from id requiring password
        /// </summary>
        /// <param name="id"></param>
        /// <param name="password"></param>
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
