using Doorfail.Connections.PL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace Doorfail.DataConnection
{
    public class CrudModelList<Tcrud, TEntity> :
        CrudModelCollection<Tcrud, TEntity, List<TEntity>>
        where TEntity : class
        where Tcrud : CrudModel_Json<TEntity>
    {

        public CrudModelList(){}
        public CrudModelList(TEntity[] entities) :base(entities){ }
        public CrudModelList(Tcrud[] entities) : base(entities) { }
    }

    public class CrudModel_DbSet<Tcrud, TEntity> :
    CrudModelCollection<Tcrud, TEntity, DbSet<TEntity>>
    where TEntity : class
    where Tcrud : CrudModel_Sql<TEntity>
    { }

    public class CrudModelCollection<Tcrud, TEntity, TTable> : List<Tcrud>
        where TEntity : class//database table
        where TTable : IEnumerable<TEntity>//Dbset or List
        where Tcrud : CrudModel<TEntity, TTable>//BL version
    {
        protected PropertyInfo[] properties { get; set; }

        public CrudModelCollection()
        {
            Type type = typeof(TEntity);
            properties = type.GetProperties();
        }

        public CrudModelCollection(Tcrud[] list)
        {
            Type type = typeof(TEntity);
            properties = type.GetProperties();

            this.AddRange(list);
        }
        public CrudModelCollection(TEntity[] list)
        {
            Type type = typeof(TEntity);
            properties = type.GetProperties();

            this.AddRange(list.Cast<Tcrud>());
        }
        //public AbsList(bool loadAll) {
        //    Type type = typeof(TEntity);
        //    properties = type.GetProperties();
        //}

        //to be used with LoadAll to convert from api EntityArray back to Collection
        public static explicit operator CrudModelCollection<Tcrud, TEntity, TTable>(Tcrud[] t)
        {
            if (t is null)
                return null;

            CrudModelCollection<Tcrud, TEntity, TTable> ret = new CrudModelCollection<Tcrud, TEntity, TTable>();
            ret.AddRange(t);
            return ret;
        }

        public Tcrud[] LoadAll(TTable table)
        {
            this.Clear();
            if (table.ToList().Count != 0)
                foreach (var c in table.ToList())
                {
                    Tcrud tinstance = (Tcrud)Activator.CreateInstance(typeof(Tcrud), new object[] { });
                    typeof(Tcrud).GetConstructor(new Type[] { typeof(TEntity) }).Invoke(tinstance, new object[] { c });//create a BL version of the PL class
                    base.Add(tinstance);//converting problem
                }
            return this.ToArray();
        }
    }

    public class SingleSortList<Tcrud, TEntity> : CrudModelList<Tcrud, TEntity>
        where TEntity : class
        where Tcrud : CrudModel_Json<TEntity>
    {
        private object sortID { get; set; }
        private int sortCol { get; set; }

        //permently delete entries
        public void DeleteAllMatching(DbSet<TEntity> table) { }
        //keep all that match filter
        public void KeepMatching(DbSet<TEntity> table, object matchingID)
        { }
        //remove all that match filter
        public void RemoveMatching(DbSet<TEntity> table, object matchingID) { }
        public void Add(CCEntities dc, DbSet<TEntity> table, TEntity entry) { }
        public void Remove(CCEntities dc, DbSet<TEntity> table, TEntity entry) { }
    }

    public class AbsListJoin<Tcrud, TEntity, TEntityJoin> : CrudModelList<Tcrud, TEntity>
        where TEntity : class
        where TEntityJoin : class
        where Tcrud : CrudModel_Json<TEntity>
    {
        protected object joinGroupingID;
        private string joinForeignIDname;
        private string joinGroupingIDname;
        private PropertyInfo[] joinTable_Properties { get; set; }

        //does not load list without calling load
        public AbsListJoin(string GroupingIDname, object GroupingID, string foreignIDname)
        {
            Type type = typeof(Tcrud);
            properties = type.GetProperties();

            Type typejoin = typeof(TEntityJoin);
            joinTable_Properties = typejoin.GetProperties();
            joinGroupingID = GroupingID;
            joinForeignIDname = foreignIDname;
            joinGroupingIDname = GroupingIDname;
        }
        public new void Clear()
        {
            joinGroupingID = null;
            base.Clear();
        }

        //TODO reimpliment LoadWithJoin using template below
        //PRES
        public void LoadWithJoin(DbSet<TEntity> entities, DbSet<TEntityJoin> joinTable,
                                 object joinid)
        {
            ///This is what this code is doing
            ///memberID = member_id;
            ///if (dc.MemberActions.ToList().Count != 0)
            ///    foreach (var col in dc.MemberActions)
            ///        if (col.MemberActionMember_ID == memberID)
            ///            foreach (var entry in dc.HelpingActions)
            ///                if (entry.HelpingActionID == col.MemberActionActionID)
            ///                    base.Add(new AbsHelpingAction(entry));

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
        public void DeleteAllPreferences(CCEntities dc, DbSet<TEntityJoin> joinTable)
        {
            foreach (var col in joinTable)
                if (joinGroupingID.Equals(PropertyHelper.getValue(col, joinGroupingIDname)))
                    joinTable.Remove(col);
            if (!Properties.Settings.Default.USE_JSON_TEMP)
                dc.SaveChanges();
            else
                JsonDatabase.SaveChanges();
            this.Clear();
        }
        public void DeleteAllPreferences(List<TEntityJoin> joinTable)
        {
            foreach (var col in joinTable)
                if (joinGroupingID.Equals(PropertyHelper.getValue(col, joinGroupingIDname)))
                    joinTable.Remove(col);
            JsonDatabase.SaveChanges();
            this.Clear();
        }

        public void Add(CCEntities dc, DbSet<TEntityJoin> joinTable, TEntityJoin joinInstance, Tcrud entry)
        {

            //Set new ID based on type
            if (joinTable_Properties[0].GetValue(joinInstance) is Guid)
                PropertyHelper.setValue(joinInstance, joinTable_Properties[0].Name, Guid.NewGuid());//instance_PK = newID using ("join_Property_ID")
            else if (joinTable_Properties[0].GetValue(joinInstance) is int)//gets type int
            {
                int newID = 0;

                //get new id if int
                if (joinTable.ToList().Count != 0)
                {
                    int max = 0;
                    foreach (var t in joinTable)
                    {
                        int comp = (int)PropertyHelper.getValue(t, joinTable_Properties[0].Name);
                        if (comp > max)
                            max = comp;
                    }
                    newID = max + 1;
                }
                //set primary key
                PropertyHelper.setValue(joinInstance, joinTable_Properties[0].Name, newID);//instance_PK = newID using ("join_Property_ID")
            }
            //if its not a guid or int the ID has to be pre-set before adding

            //set ID for group
            PropertyHelper.setValue(joinInstance, joinGroupingIDname, joinGroupingID);//instance_Grouping_ID = join_Grouping_ID using ("join_Grouping_ID")
            //set ID of entry being added
            PropertyHelper.setValue(joinInstance, joinForeignIDname,           //instance_FK using("join_FK") = 
                            PropertyHelper.getValue(entry, properties[0].Name)); // entry_ID using("entry_ID")

            joinTable.Add(joinInstance);//add
            dc.SaveChanges();
            base.Add(entry);
        }

        public void Add(List<TEntityJoin> joinTable, TEntityJoin joinInstance, Tcrud entry)
        {

            //Set new ID based on type
            if (joinTable_Properties[0].GetValue(joinInstance) is Guid)
                PropertyHelper.setValue(joinInstance, joinTable_Properties[0].Name, Guid.NewGuid());//instance_PK = newID using ("join_Property_ID")
            else if (joinTable_Properties[0].GetValue(joinInstance) is int)//gets type int
            {
                int newID = 0;

                //get new id if int
                if (joinTable.ToList().Count != 0)
                {
                    int max = 0;
                    foreach (var t in joinTable)
                    {
                        int comp = (int)PropertyHelper.getValue(t, joinTable_Properties[0].Name);
                        if (comp > max)
                            max = comp;
                    }
                    newID = max + 1;
                }
                //set primary key
                PropertyHelper.setValue(joinInstance, joinTable_Properties[0].Name, newID);//instance_PK = newID using ("join_Property_ID")
            }
            //if its not a guid or int the ID has to be pre-set before adding

            //set ID for group
            PropertyHelper.setValue(joinInstance, joinGroupingIDname, joinGroupingID);//instance_Grouping_ID = join_Grouping_ID using ("join_Grouping_ID")
            //set ID of entry being added
            PropertyHelper.setValue(joinInstance, joinForeignIDname,           //instance_FK using("join_FK") = 
                            PropertyHelper.getValue(entry, properties[0].Name)); // entry_ID using("entry_ID")

            joinTable.Add(joinInstance);//add
            JsonDatabase.SaveChanges();
            base.Add(entry);
        }

        public void Remove(CCEntities dc, DbSet<TEntityJoin> joinTable, Tcrud entry)
        {
            foreach (var join in joinTable)
            {
                if (joinGroupingID.Equals(PropertyHelper.getValue(join, joinGroupingIDname)) &&//ID for this group matches
                    PropertyHelper.getValue(entry, properties[0].Name).Equals(
                                   PropertyHelper.getValue(join, joinForeignIDname))
                    )//ID for this entry matches
                    joinTable.Remove(join);
            }
            dc.SaveChanges();
            base.Remove(entry);
        }
    }
}
