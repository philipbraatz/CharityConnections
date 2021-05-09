using Doorfail.Connections.PL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace Doorfail.DataConnection
{

    //PropertyInfo with custom info
    //Max
    internal class PropertyDB_Info<TEntity>
    {
        internal int max { get; private set; }
        internal PropertyInfo p;
        public PropertyDB_Info(PropertyInfo info, DbContext context, TEntity entity)
        {
            p = info;
            loadPropertyMax(context, entity);
        }
        public PropertyDB_Info(PropertyInfo info, TEntity entity)
        {
            if (!Properties.Settings.Default.USE_JSON_TEMP)
                throw new InvalidOperationException("Cannot use JSON while USE_JSON_TEMP = false");
            else if (!JsonDatabase.inintalized)
            {
                JsonDatabase.LoadDatabase();
            }
            p = info;
            loadPropertyMax(entity);
        }
        public PropertyDB_Info(PropertyInfo info) => p = info;

        private void loadPropertyMax(DbContext context, TEntity entity)
        {
            if (p == null || p.Name != "String")
                max = -1;
            else
            {
                int? nmax = PropertyHelper.GetMaxLength<TEntity>(context, (x) => (string)p.GetValue(entity));
                max = nmax != null ? (int)nmax : -1;
            }
        }
        private void loadPropertyMax(TEntity entity)
        {
            max = -1;
        }

    }
    internal class PropertyException : Exception
    {
        public PropertyException() : base() { }
        public PropertyException(Type tEntity, string propertyName) : base(tEntity + " does not have the property " + propertyName)
        { }

        public PropertyException(Type tEntity, string propertyName, Exception innerException) : base(tEntity + " does not have the property " + propertyName, innerException)
        { }

        protected PropertyException(SerializationInfo info, StreamingContext context) : base(info, context)
        { }
    }

    public class CrudModel_Json<TEntity> : CrudModel<TEntity, List<TEntity>>
        where TEntity : class
    {
        public CrudModel_Json() => createInstance();
        //create from exist instance
        public CrudModel_Json(TEntity entry) => createInstance(entry);
        //load from database
        public CrudModel_Json(List<TEntity> table, object id, bool preload = false, string load_AlternativeField = "")
        {
            try
            {
                createInstance();
            }
            catch (Exception e)
            {
                if (e.Message != "The underlying provider failed on Open.")
                    throw e;
                else
                    throw new Exception("Could not connect to database: " + e.InnerException.Message);
            }
            if (!preload)
                LoadId(table, id, load_AlternativeField);
        }
    }
    public class CrudModel_Sql<TEntity> : CrudModel<TEntity, DbSet<TEntity>>
    where TEntity : class
    {
        public CrudModel_Sql() => createInstance();
        //create from exist instance
        public CrudModel_Sql(TEntity entry) => createInstance(entry);
        //load from database
        public CrudModel_Sql(DbSet<TEntity> table, object id, bool preload = false, string load_AlternativeField = "")
        {
            try
            {
                createInstance();
            }
            catch (Exception e)
            {
                if (e.Message != "The underlying provider failed on Open.")
                    throw e;
                else
                    throw new Exception("Could not connect to database: " + e.InnerException.Message);
            }
            if (!preload)
                LoadId(table, id, load_AlternativeField);
        }
    }

    public class CrudModel<TEntity, TTable>
        where TEntity : class
        where TTable : IEnumerable<TEntity>
    {
        //the ID is what ever parameter is first in the class
        //most fields are private/protected because this class should be inherited 
        //  and the functions should be created without DBconnection and object fields
        protected object ID
        {
            get { return properties[0].p.GetValue(instance); }
            set { properties[0].p.SetValue(instance, value); }
        }
        //holds actual entry class instance
        //also used to check type
        private TEntity instance;
        //public string name { get { return instance.GetType().Name; } }
        //private int id_col { get; set; }
        private List<PropertyDB_Info<TEntity>> properties { get; set; }
        //usefull functions
        //Name              - parameter name in code
        //GetValue(dummy)   - actual value
        //PropertyType      - actual values type

        //gets and varibles values
        private object getValue(TEntity prop, string propertyName)
        {
            Type type = typeof(TEntity);//maybe make property
            PropertyInfo propinf = type.GetProperties().Where(c => c.Name == propertyName).FirstOrDefault();
            if (propinf != null)
            {
                object ret = propinf.GetValue(prop);
                if (ret != null)
                    return ret;
                else
                    return default;
            }
            else
                throw new PropertyException(typeof(TEntity), propertyName);
        }
        private void setValue(TEntity prop, string propertyName, object value)
        {
            Type type = typeof(TEntity);//maybe make property
            PropertyInfo propinf = type.GetProperties().Where(c => c.Name == propertyName).FirstOrDefault();
            if (propinf != null)
                propinf.SetValue(prop, value);
            else
                throw new PropertyException(typeof(TEntity), propertyName);
        }

        //gets this instance
        public object getProperty(string propertyName)
        {
            PropertyDB_Info<TEntity> prop = properties.Where(c => c.p.Name == propertyName).FirstOrDefault();
            if (prop != null)
            {
                object ret = prop.p.GetValue(instance);
                if (ret != null)
                    return ret;
                else
                    return default;
            }
            else
                throw new PropertyException(typeof(TEntity), propertyName);
        }
        protected void setProperty(string propertyName, object value, bool forceInt = true)
        {
            setProperty(propertyName, (int)value);
        }
        public void setProperty(string propertyName, object value)
        {

            try
            {
                PropertyDB_Info<TEntity> propinf = properties.Where(c => c.p.Name == propertyName).FirstOrDefault();
                Type propType = value?.GetType() ?? propinf.p.PropertyType;
                if (propinf == null)
                    throw new PropertyException(typeof(TEntity), propertyName);



                if (propinf != null)
                {
                    //propinf.p.SetValue(instance,value);
                    if (value == null)
                        propinf.p.SetValue(instance, null);//deal with null right away
                    else switch (propType.Name)
                        {
                            case "Nullable`1"://some weird type, TODO research nullable propType
                                Type[] propGeneric = propType.GenericTypeArguments;

                                if (propGeneric.Length > 0)
                                {

                                    if (propGeneric[0].Name == "DateTime")
                                        propinf.p.SetValue(instance, (DateTime)value);
                                    else if (propGeneric[0].Name == "Guid")
                                        propinf.p.SetValue(instance, (Guid)value);
                                    else
                                        propinf.p.SetValue(instance, value);
                                }

                                else
                                    propinf.p.SetValue(instance, value);
                                break;
                            case "String":
                                if (((string)value).Length > propinf.max)//get property index equal to current property to compare sized
                                {
                                    propinf.p.SetValue(instance, ((string)value));//.Substring(0, propinf.max - 1));//cut of larger values (zero based)
                                    break;//only break if proper length string
                                }
                                break;
                            case "Uri":
                                propinf.p.SetValue(instance, ((Uri)value).OriginalString);
                                break;
                            default:
                                propinf.p.SetValue(instance, value);//yolo and hope it works
                                break;
                        }
                }
                else
                    throw new PropertyException(typeof(TEntity), propertyName);
            }
            catch (Exception e)
            {
                if (true)
                    throw new Exception(typeof(TEntity) + ": " + e.Message);
            }
        }

        protected void setProperties(Object destination)
        {
            if (destination is null)
                throw new ArgumentNullException(nameof(destination));

            // Iterate the Properties of the destination instance and  
            // populate them from their source counterparts  
            PropertyInfo[] destProp = destination.GetType().GetProperties();
            foreach (PropertyInfo property in destProp)
                if (property.CanWrite)
                    property.SetValue(this, property.GetValue(destination, null), null);
        }

        protected CrudModel<TEntity, TTable> getInstance() => this;

        private bool isDbSet() => typeof(TTable) == typeof(DbSet<TEntity>);
        private bool isList() => typeof(TTable) == typeof(List<TEntity>);

        private void ThrowInvalidTTable()
            => throw new NotImplementedException("CrudModel does not support " + typeof(TTable));

        protected void createInstance() =>
            createInstance((TEntity)Activator.CreateInstance(typeof(TEntity), new object[] { }));
        protected void createInstance(TEntity entry)
        {
            Type type = typeof(TEntity);//maybe make property
            instance = entry;
            properties = new List<PropertyDB_Info<TEntity>>();
            if (isDbSet())
                using (CCEntities dc = new CCEntities())
                {
                    type.GetProperties().ToList().ForEach(c =>
                    {
                        PropertyDB_Info<TEntity> test = new PropertyDB_Info<TEntity>(c, dc, instance);
                        properties.Add(test);
                    }
                    );
                }
            else if (isList())
            {
                type.GetProperties().ToList().ForEach(c =>
                {
                    PropertyDB_Info<TEntity> test = new PropertyDB_Info<TEntity>(c, instance);
                    properties.Add(test);
                });
            }
            else
                ThrowInvalidTTable();
        }
        //new instance
        public CrudModel() => createInstance();
        //create from exist instance
        public CrudModel(TEntity entry) => createInstance(entry);
        //load from database
        public CrudModel(TTable table, object id, bool preload = false, string load_AlternativeField = "")
        {
            try
            {
                createInstance();
            }
            catch (Exception e)
            {
                if (e.Message != "The underlying provider failed on Open.")
                    throw e;
                else
                    throw new Exception("Could not connect to database: " + e.InnerException.Message);
            }
            if (!preload)
                LoadId(table, id, load_AlternativeField);
        }

        public void Clear()
        {
            foreach (var c in properties)
                setValue(instance, c.p.Name, default);
        }

        protected int Delete(CCEntities dc, DbSet<TEntity> table)
        {
            try
            {
                foreach (var col in table)
                {
                    //instance = where( entry in the table == this ID)
                    if (ID.Equals(getValue(col, properties[0].p.Name)))
                        table.Remove(col);
                }

                int changes = dc.SaveChanges();
                if (changes == 0)
                    throw new Exception("Could not delete value " + properties[0].p.Name + " = " + properties[0].p.GetValue(instance));
                else
                    return dc.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }
        protected int Delete(List<TEntity> table)
        {
            try
            {
                foreach (var col in table)
                    //instance = where( entry in the table == this ID)
                    if (ID.Equals(getValue(col, properties[0].p.Name)))
                        table.Remove(col);
                JsonDatabase.SaveChanges(table);
                return 1;
            }
            catch (Exception e) { throw e; }
        }

        protected static bool Exists(TTable table, CrudModel<TEntity, TTable> instance)
        {
            foreach (var col in table)
                if (instance.ID.Equals(instance.getValue(col, instance.properties[0].p.Name)))
                    return true;
            return false;
        }

        protected int Insert(CCEntities dc, DbSet<TEntity> table)
        {
            try
            {
                //List<TEntity> tlist = table.ToList();

                //Set new ID based off of id type
                if (properties[0].p.GetValue(instance) is Guid)
                    ID = Guid.NewGuid();
                else if (properties[0].p.GetValue(instance) is int)//gets type int
                    if (table.ToList().Count > 0)
                    {
                        //only works when ID is int and database orders
                        TEntity entity = table.ToList().LastOrDefault();
                        PropertyInfo id_prop = entity.GetType().GetProperty(properties[0].p.Name, typeof(int));
                        ID = (int)id_prop.GetValue(entity) + 1;
                    }
                    else
                        ID = 0;
                //check for valid id
                else if (properties[0].p.GetValue(instance) is string)// string type
                {
                    if ((string)ID == string.Empty)
                        throw new Exception("ID cannot be blank");
                }
                else
                    throw new Exception("ID of type " + properties[0].p.GetValue(instance).GetType().Name + " is not supported");

                if (isDbSet())
                    ((DbSet<TEntity>)(Object)table).Add(instance);

                //dc.Categories.Add(this);
                return dc.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }

        protected int Insert(List<TEntity> table)
        {
            try
            {
                //List<TEntity> tlist = table.ToList();

                //Set new ID based off of id type
                if (properties[0].p.GetValue(instance) is Guid)
                    ID = Guid.NewGuid();
                else if (properties[0].p.GetValue(instance) is int)//gets type int
                    if (table.ToList().Count > 0)
                    {
                        //only works when ID is int and database orders
                        TEntity entity = table.ToList().LastOrDefault();
                        PropertyInfo id_prop = entity.GetType().GetProperty(properties[0].p.Name, typeof(int));
                        ID = (int)id_prop.GetValue(entity) + 1;
                    }
                    else
                        ID = 0;
                //check for valid id
                else if (properties[0].p.GetValue(instance) is string)// string type
                {
                    if ((string)ID == string.Empty)
                        throw new Exception("ID cannot be blank");
                }
                else
                    throw new Exception("ID of type " + properties[0].p.GetValue(instance).GetType().Name + " is not supported");

                ((List<TEntity>)(Object)table).Add(instance);

                //dc.Categories.Add(this);
                JsonDatabase.SaveChanges(table);
                return 1;
            }
            catch (Exception e) { throw e; }
        }

        protected void LoadId(TTable table, object id, string load_AlternativeField = "")
        {
            //Set property then call self again
            if (load_AlternativeField == string.Empty)
            {
                ID = id;
                LoadId(table, (string)properties[0].p.Name);
            }
            else
            {
                setProperty(load_AlternativeField, id);
                LoadId(table, load_AlternativeField);
            }
        }

        private void CleanNulls()
        {
            foreach (var p in properties)
                if (p.p.GetValue(instance) == null)
                    switch (p.p.Name)
                    {
                        case "DateOfBirth":
                            p.p.SetValue(instance, new DateTime());
                            break;
                        default:
                            p.p.SetValue(instance, default);//PRES C# 7.1 Default literal
                            break;
                    }
        }

        protected TEntity LoadId(TTable table, string propName = "_Default")
        {
            if (propName == "_Default")
                propName = (string)properties[0].p.Name;

            try
            {
                object tempID = getProperty(propName);
                bool found = false;

                //dont cound the first thing you find 
                //it always thinks the first id matches current id.
                //FIRST entry in database is skipped
                bool fixFirstFound = false;
                TEntity firstObject = null;
                //List<TEntity> tDebugVarible = table.ToList();
                foreach (var col in table)
                {
                    if (fixFirstFound)
                    {
                        //instance = where( entry in the table == this ID)
                        if ((getValue(col, propName).ToString()) == (tempID.ToString()))
                        {
                            instance = col;//sets all properties
                            found = true;
                            break;
                        }
                    }
                    else//keep first entry to check later
                    {
                        firstObject = col;
                        fixFirstFound = true;
                    }
                }
                //check first entry
                if (firstObject != null && (getValue(firstObject, propName).ToString()) == (tempID.ToString()))
                {
                    instance = firstObject;
                    found = true;
                }

                if (!found)
                    throw new Exception(instance.GetType().Name + " could not be found with ID = " + ID);

                CleanNulls();
                return instance;
            }
            catch (Exception e)
            {
                //throw e;
                return table.First();//bad way to do things
            }
        }

        protected int Update(CCEntities dc, DbSet<TEntity> table)
        {
            try
            {
                if (!Exists((TTable)(Object)table, this))
                    throw new Exception("ID does not exist in table");

                foreach (var col in table)
                    if (ID.Equals(getValue(col, properties[0].p.Name)))
                    {
                        foreach (var c in properties)
                            setValue(col, c.p.Name,            //set table column
                                getValue(instance, c.p.Name)  //to current instance
                            );
                    }
                return dc.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }

        protected int Update(List<TEntity> table)
        {
            try
            {
                if (!Exists((TTable)(Object)table, this))
                    throw new Exception("ID: "+this.ID+" does not exist in table: "+typeof(TTable));

                foreach (var col in table)
                    if (ID.Equals(getValue(col, properties[0].p.Name)))
                    {
                        foreach (var c in properties)
                            setValue(col, c.p.Name,            //set table column
                                getValue(instance, c.p.Name)  //to current instance
                            );
                    }
                JsonDatabase.SaveChanges(table);
                return 1;
            }
            catch (Exception e) { throw e; }
        }
    }
}
