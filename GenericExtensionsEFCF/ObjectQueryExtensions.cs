using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Linq;
using System.Reflection;

namespace GenericExtensionsEFCF
{
    public static class ObjectQueryExtensions
    {
        public static IEnumerable<TProjected> Map<TProjected>(this ObjectQuery<DbDataRecord> dbDataRecords)
            where TProjected : new()
        {
            var projectedType = typeof(TProjected);
            var properties = projectedType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            return dbDataRecords.Select(dbDataRecord => MapDataRecordToProperties<TProjected>(dbDataRecord, properties)).ToList();
        }

        private static TProjected MapDataRecordToProperties<TProjected>(IDataRecord dbDataRecord, IEnumerable<PropertyInfo> properties)
            where TProjected : new()
        {
            var projectedEntity = new TProjected();
            foreach (var propertyInfo in properties)
            {
                propertyInfo.SetValue(projectedEntity, dbDataRecord[propertyInfo.Name], new object[] { });
            }
            return projectedEntity;
        }
    }
}
