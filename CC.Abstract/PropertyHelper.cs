using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Doorfail.DataConnection
{
    public static class PropertyHelper
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
                throw new Exception(typeof(TEntity) + " does not have the property '" + propertyName + "'");
        }
        public static void setValue<TEntity>(TEntity prop, string propertyName, object value)
        {
            Type type = typeof(TEntity);//maybe make property
            PropertyInfo[] props = type.GetProperties();
            PropertyInfo setter = props.Where(c => c.Name == propertyName).FirstOrDefault();
            if (setter != null)
                setter.SetValue(prop, value);
            else
                throw new Exception(typeof(TEntity) + " does not have the property '" + propertyName + "'");
        }

        // get MaxLength as an extension method to the DbContext
        public static int? GetMaxLength<T>(this DbContext context, Expression<Func<T, string>> column)
        {
            return (int?)context.GetFacets<T>(column)["MaxLength"].Value;
        }

        // get MaxLength as an extension method to the Facets (I think the extension belongs here)
        public static int? GetMaxLength(this ReadOnlyMetadataCollection<Facet> facets)
        {
            return (int?)facets["MaxLength"].Value;
        }

        // just for fun: get all the facet values as a Dictionary 
        public static Dictionary<string, object> AsDictionary(this ReadOnlyMetadataCollection<Facet> facets)
        {
            return facets.ToDictionary(o => o.Name, o => o.Value);
        }


        public static ReadOnlyMetadataCollection<Facet> GetFacets<T>(this DbContext context, Expression<Func<T, string>> column)
        {
            ReadOnlyMetadataCollection<Facet> result = null;

            var entType = typeof(T);
            var columnName = ((MemberExpression)column.Body).Member.Name;

            var objectContext = ((IObjectContextAdapter)context).ObjectContext;
            var test = objectContext.MetadataWorkspace.GetItems(DataSpace.CSpace);

            if (test == null)
                return null;

            var q = test
                .Where(m => m.BuiltInTypeKind == BuiltInTypeKind.EntityType)
                .SelectMany(meta => ((EntityType)meta).Properties
                .Where(p => p.Name == columnName && p.TypeUsage.EdmType.Name == "String"));

            var queryResult = q.Where(p =>
            {
                var match = p.DeclaringType.Name == entType.Name;
                if (!match)
                    match = entType.Name == p.DeclaringType.Name;

                return match;

            })
                .Select(sel => sel)
                .FirstOrDefault();

            result = queryResult.TypeUsage.Facets;

            return result;

        }
    }
}
