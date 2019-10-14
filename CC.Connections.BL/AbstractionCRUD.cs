using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;
using System.Reflection;

namespace CC.Connections.BL
{
    public static class Utils
    {
        public static bool HasProperty(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName) != null;
        }

        //gets and varibles values
        public static object getValue<TEntity>(TEntity prop, string propertyName)
        {
            Type type = typeof(TEntity);//maybe make property
            PropertyInfo[] props = type.GetProperties();
            PropertyInfo ret = props.Where(c => c.Name == propertyName).FirstOrDefault();
            if (ret != null)
                return ret.GetValue(prop);
            else
                throw new Exception(typeof(TEntity)+" does not have the property '"+propertyName+"'");
        }
        public static void setValue<TEntity>(TEntity prop, string propertyName, object value)
        {
            Type type = typeof(TEntity);//maybe make property
            PropertyInfo[] props = type.GetProperties();
            PropertyInfo setter = props.Where(c => c.Name == propertyName).FirstOrDefault();
            if(setter != null)
                setter.SetValue(prop, value);
            else
                throw new Exception(typeof(TEntity) + " does not have the property '" + propertyName + "'");
        }
        //public static TCrud toBL<TCrud,TEntity>(TEntity entity) 
        //    where TCrud : ColumnEntry<TEntity>
        //    where TEntity : class
        //{
        //    return (TCrud)entity;
        //}

        ////not used
        //private static void createAbstractInstance(object instance, PropertyInfo prop)
        //{
        //    string name = prop.Name;
        //    dynamic propValue = prop.GetValue(instance);
        //
        //    Type genericClass = typeof(CellEntry<>);//class you want to create
        //    Type constructedClass = genericClass.MakeGenericType(prop.PropertyType);
        //    dynamic created = Activator.CreateInstance(constructedClass,
        //        name, propValue);//constructer values are optional
        //    if (created != null)
        //        columns.Add(created);
        //    else
        //        throw new Exception("ColumnEntry<> could not add " + name + "(" + prop.PropertyType + ") : " + propValue);
        //}
    }
    public class ColumnEntry<TEntity> where TEntity : class
    {
        //the ID is what ever parameter is first in the class
        //most fields are private/protected because this class should be inherited 
        //  and the functions should be created without DBconnection and object fields
        protected object ID
        {
            get { return properties[0].GetValue(instance); }
            set { properties[0].SetValue(instance, value); }
        }
        //holds actual entry class instance
        //also used to check type
        private TEntity instance;
        //public string name { get { return instance.GetType().Name; } }
        //private int id_col { get; set; }
        private PropertyInfo[] properties { get; set; }
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
                object ret = propinf.GetValue(instance);
                if (ret != null)
                    return ret;
                else
                    return default;
            }
            else
                throw new Exception(typeof(TEntity) + " does not have a " + propertyName + " property");
        }
        private void setValue(TEntity prop, string propertyName, object value)
        {
            Type type = typeof(TEntity);//maybe make property
            PropertyInfo propinf = type.GetProperties().Where(c => c.Name == propertyName).FirstOrDefault();
            if (propinf != null)
                propinf.SetValue(prop, value);
            else
                throw new Exception(typeof(TEntity) + " does not have a " + propertyName + " property");
        }

        //gets this instance
        protected object getProperty(string propertyName)
        {
            PropertyInfo prop = properties.Where(c => c.Name == propertyName).FirstOrDefault();
            if (prop != null)
            {
                object ret = prop.GetValue(instance);
                if (ret != null)
                    return ret;
                else
                    return default;
            }
            else
                throw new Exception(typeof(TEntity) + " does not have a " + propertyName + " property");
        }
        protected void setProperty(string propertyName, object value)
        {
            PropertyInfo propinf = properties.Where(c => c.Name == propertyName).FirstOrDefault();
            if (propinf != null)
                propinf.SetValue(instance, value);
            else
                throw new Exception(typeof(TEntity) + " does not have a " + propertyName + " property");
        }

        //public void LoadTable(DbSet<TEntity> tableUsed)
        //{
        //    table = tableUsed;
        //}
        public ColumnEntry(TEntity entry)
        {
            Type type = typeof(TEntity);//maybe make property
            properties = type.GetProperties();
            instance = entry;
        }
        public ColumnEntry(DbSet<TEntity> table, object id,string load_AlternativeField = "")
        {
            Type type = typeof(TEntity);//maybe make property
            properties = type.GetProperties();
            instance = table.FirstOrDefault();
            LoadId(table, id,load_AlternativeField);
        }

        //public static Tcrud ConvertToBL<Tcrud>(TEntity entity) 
        //    where Tcrud : ColumnEntry<TEntity>
        //{
        //    return new ColumnEntry<TEntity>(entity);
        //}
        //public static implicit operator ColumnEntry<TEntity>(TEntity entry)
        //{
        //    return new ColumnEntry<TEntity>(entry);
        //}
        //public extern static Tcrud ToBL<Tcrud>(TEntity entity) 
        //    where Tcrud :ColumnEntry<TEntity>, new();

        public void Clear()
        {
            foreach (var c in properties)
                setValue(instance, c.Name, default);
        }

        protected int Delete(DBconnections dc, DbSet<TEntity> table)
        {
            try
            {
                foreach (var col in table)
                    //instance = where( entry in the table == this ID)
                    if (ID.Equals(getValue(col, properties[0].Name)))
                        table.Remove(col);
                int changes = dc.SaveChanges();
                if (changes == 0)
                    throw new Exception("Could not delete value " + properties[0].Name + " = " + properties[0].GetValue(instance));
                else
                    return dc.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }

        protected bool Exists(DbSet<TEntity> table)
        {
            foreach (var col in table)
                if (ID.Equals(getValue(col, properties[0].Name)))
                    return true;
            return false;
        }

        protected int Insert(DBconnections dc, DbSet<TEntity> table)
        {
            try
            {
                List<TEntity> tlist = table.ToList();
                //get highest integer id
                if (properties[0].GetValue(instance) is int)//gets type int
                    if (table.ToList().Count > 0)
                    {
                        //only works when ID is int and database orders
                        TEntity entity = table.ToList().LastOrDefault();
                        PropertyInfo id_prop = entity.GetType().GetProperty(properties[0].Name, typeof(int));
                        ID = (int)id_prop.GetValue(entity) +1;

                        //int max = 0;
                        //foreach (var t in table)
                        //{
                        //    comp = (int)getValue(t, properties[0].Name);
                        //    if (comp > max)
                        //        max = comp;
                        //}
                        //ID = max + 1;
                    }
                    else
                        ID = 0;
                //check for valid id
                else if (properties[0].GetValue(instance) is string)// string type
                {
                    if ((string)ID == string.Empty)
                        throw new Exception("ID cannot be blank");
                }
                else
                    throw new Exception("ID must be int or string");

                table.Add(instance);

                //dc.Categories.Add(this);
                return dc.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }
        protected void LoadId(DbSet<TEntity> table, object id,string load_AlternativeField = "")
        {
            if (load_AlternativeField == string.Empty)
            {
                ID = id;
                LoadId(table, (string)properties[0].Name);
            }
            else
            {
                setProperty(load_AlternativeField,id);
                LoadId(table, load_AlternativeField);
            }
        }
        //any property that cant be null should be put in here
        //TODO make abstract list of nonnullable values
        private void CleanNulls()
        {
            foreach (var p in properties)
                if (p.GetValue(instance) == null)
                    switch (p.Name)
                    {
                        case "DateOfBirth":
                            p.SetValue(instance, new DateTime());
                            break;
                        default:
                            p.SetValue(instance, default);
                            break;
                    }
        }

        protected TEntity LoadId(DbSet<TEntity> table,string propName="_Default")
        {
            if (propName == "_Default")
                propName = (string)properties[0].Name;

            try
            {
                object tempID = getProperty(propName);
                bool found = false;
                //dont cound the first thing you find 
                //it always thinks the first id matches current id. Might break things
                bool fixFirstFound = false;
                List<TEntity> tDebugVarible = table.ToList();
                foreach (var col in table)
                {
                    if (fixFirstFound)
                    {
                        //instance = where( entry in the table == this ID)
                        if (tempID.Equals(getValue(col, propName)))
                        {
                            instance = col;//sets all properties
                            found = true;
                            break;
                        }
                    }
                    else
                        fixFirstFound = true;
                }
                if (!found)
                    throw new Exception(instance.GetType().Name + " could not be found with ID = " + ID);

                CleanNulls();
                return instance;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        protected int Update(DBconnections dc, DbSet<TEntity> table)
        {
            try
            {
                if (!Exists(table))
                    throw new Exception("ID does not exist in table");

                foreach (var col in table)
                    if (ID.Equals(getValue(col, properties[0].Name)))
                    {
                        foreach (var c in properties)
                            setValue(col, c.Name,            //set table column
                                getValue(instance, c.Name)  //to current instance
                            );
                    }
                return dc.SaveChanges();
            }
            catch (Exception e) { throw e; }
        }
    }

    public class AbsList<Tcrud, TEntity> : List<Tcrud>
        where TEntity : class//database table
        where Tcrud : ColumnEntry<TEntity>//BL version
    {
        protected PropertyInfo[] properties { get; set; }

        public AbsList()
        {
            Type type = typeof(TEntity);
            properties = type.GetProperties();
        }
        //public AbsList(bool loadAll) {
        //    Type type = typeof(TEntity);
        //    properties = type.GetProperties();
        //}

            //TODO reimpliment
        public void LoadAll(DbSet<TEntity> table)
        {
           //this.Clear();
           //if (table.ToList().Count != 0)
           //    foreach (var c in table.ToList())
           //        base.Add(Tcrud.ToBL<Tcrud>(c));//converting problem
        }
    }

    public class AbsListJoin<Tcrud, TEntity, TEntityJoin> : AbsList<Tcrud, TEntity>
        where Tcrud : ColumnEntry<TEntity>
        where TEntity : class
        where TEntityJoin : class
    {
        protected object joinGrouping_ID;
        private string joinForeign_ID_name;
        private string joinGrouping_ID_name;
        private PropertyInfo[] joinTable_Properties { get; set; }

        //does not load list without calling load
        public AbsListJoin(string Grouping_ID_name, object Grouping_ID, string foreign_ID_name)
        {
            Type type = typeof(Tcrud);
            properties = type.GetProperties();

            Type typejoin = typeof(TEntityJoin);
            joinTable_Properties = typejoin.GetProperties();
            joinGrouping_ID = Grouping_ID;
            joinForeign_ID_name = foreign_ID_name;
            joinGrouping_ID_name = Grouping_ID_name;
        }
        public new void Clear()
        {
            joinGrouping_ID = null;
            base.Clear();
        }

        //TODO reimpliment
        public void LoadWithJoin(DbSet<TEntity> entities, DbSet<TEntityJoin> join_table,
                                 object join_id)
        {
            ///This is what this code is doing
            ///memberID = member_id;
            ///if (dc.Member_Action.ToList().Count != 0)
            ///    foreach (var col in dc.Member_Action)
            ///        if (col.MemberActionMember_ID == memberID)
            ///            foreach (var entry in dc.Helping_Action)
            ///                if (entry.Helping_Action_ID == col.MemberActionAction_ID)
            ///                    base.Add(new AbsHelping_Action(entry));
            
            ///Actual implimentation
            //this.Clear();
            //joinTable_ID = join_id;
            //if (entities.ToList().Count != 0)
            //    foreach (var col in join_table)
            //    {
            //        //instance = where( entry in the table == this ID)
            //        object colJoin_ID = Utils.getValue(col, joinTable_ID_Prop_Name);
            //        object colentity_ID = Utils.getValue(col, joinProp_name);
            //        if (joinTable_ID.Equals(colJoin_ID))
            //        {
            //            foreach (var entry in entities)
            //            {
            //                try
            //                {
            //                    //Converting problem
            //                    Tcrud crud = ColumnEntry<TEntity>.ConvertToBL<Tcrud>(entry);//from Tentity to Tcrud
            //                    if (colentity_ID.Equals(Utils.getValue(entry, properties[0].Name)))
            //                        base.Add(crud);
            //
            //
            //                }
            //                catch (Exception e)
            //                {
            //
            //                    throw new Exception("AbstractionCRUD LoadWithJoin Adding "+
            //                        entry.GetType().Name+" as ("+typeof(Tcrud)+"): "+e.Message);
            //                }
            //            }
            //        }
            //    }
        }
        public void DeleteAllPreferences(DBconnections dc, DbSet<TEntityJoin> join_table)
        {
            foreach (var col in join_table)
                if (joinGrouping_ID.Equals(Utils.getValue(col, joinGrouping_ID_name)))
                    join_table.Remove(col);
            dc.SaveChanges();
            this.Clear();
        }

        public void Add(DBconnections dc, DbSet<TEntityJoin> joinTable, TEntityJoin joinInstance, Tcrud entry)
        {
            if (joinTable_Properties[0].GetValue(joinInstance) is int)//gets type int
            {
                //get new id if int
                int newID = 0;
                if (joinTable.ToList().Count != 0)
                {
                    int max = 0;
                    foreach (var t in joinTable)
                    {
                        int comp = (int)Utils.getValue(t, joinTable_Properties[0].Name);
                        if (comp > max)
                            max = comp;
                    }
                    newID = max + 1;
                }
                //set primary key
                Utils.setValue(joinInstance, joinTable_Properties[0].Name, newID);//instance_PK = newID using ("join_Property_ID")
            }
            //set ID for group
            Utils.setValue(joinInstance, joinGrouping_ID_name,joinGrouping_ID);//instance_Grouping_ID = join_Grouping_ID using ("join_Grouping_ID")
            //set ID of entry being added
            Utils.setValue(joinInstance, joinForeign_ID_name,           //instance_FK using("join_FK") = 
                            Utils.getValue(entry, properties[0].Name)); // entry_ID using("entry_ID")

            joinTable.Add(joinInstance);//add
            dc.SaveChanges();
            base.Add(entry);
        }
        public void Remove(DBconnections dc, DbSet<TEntityJoin> joinTable, Tcrud entry)
        {
            foreach (var join in joinTable)
            {
                if (joinGrouping_ID.Equals(Utils.getValue(join, joinGrouping_ID_name)) &&//ID for this group matches
                    Utils.getValue(entry, properties[0].Name).Equals(
                                   Utils.getValue(join, joinForeign_ID_name))
                    )//ID for this entry matches
                    joinTable.Remove(join);
            }
            dc.SaveChanges();
            base.Remove(entry);
        }
    }
}
