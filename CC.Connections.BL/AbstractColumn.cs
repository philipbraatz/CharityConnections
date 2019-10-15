using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CC.Connections.PL;

namespace CC.Connections.BL
{
    //Entry
    public interface ICrud<TEntity> where TEntity : class
    {
        void Clear();
        bool Exists(DbSet<TEntity> table);
        void LoadId(DbSet<TEntity> table);
        void LoadId(DbSet<TEntity> table, int id);
        int Insert(DBconnections dc, DbSet<TEntity> table);
        int Delete(DBconnections dc, DbSet<TEntity> table);
        int Update(DBconnections dc, DbSet<TEntity> table);
    }

    //Table or Junction table join
    public interface ICrudList<TEntity> where TEntity : class
    {
        void Clear();
        void LoadAll(DbSet<TEntity> table);
        void DeleteAllMatching(DbSet<TEntity> table);
        void LoadMatching(DbSet<TEntity> table, object matching_ID);
        void RemoveMatching(DbSet<TEntity> table, object matching_ID);
        void Add(DBconnections dc, DbSet<TEntity> table, TEntity entry);
        void Remove(DBconnections dc, DbSet<TEntity> table, TEntity entry);
    }

    /// <summary>
    /// Example class of multityped list
    /// unused
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    ////could be an interface
    ////abstract is faster but less robust
    //public abstract class CellEntry
    //{
    //    public abstract string Name();
    //    public abstract void Clear();//sets the value to empty
    //
    //    public abstract CellEntry setCell(PropertyInfo v);
    //}
    //// extend abstract ColumnEntry class
    //public class CellEntry<DataType> : CellEntry where DataType : struct
    //{
    //    private DataType mDataType;//is any datatype!!!
    //    private string name;
    //
    //    public CellEntry(string var_name)
    //    {
    //        name = var_name;
    //    }
    //    public CellEntry(string var_name, DataType value)
    //    {
    //        name = var_name;
    //        mDataType = value;
    //    }
    //
    //    public override void Clear()
    //    {
    //        mDataType = new DataType();
    //    }
    //
    //    public override CellEntry setCell(PropertyInfo v)
    //    {
    //        var instance = v.GetType();
    //        Type type = typeof(User);
    //        return new CellEntry<DataType>(v.Name, v.GetValue(v));
    //    }
    //
    //    public override string Name(){ return name; }
    //}
}
