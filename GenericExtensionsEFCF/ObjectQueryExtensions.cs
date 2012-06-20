using System;
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

            foreach (var dbDataRecord in dbDataRecords)
            {
                var entity = MapDataRecordToProperties<TProjected>(dbDataRecord, properties);
                yield return entity;
            }
        }

        private static TProjected MapDataRecordToProperties<TProjected>(IDataRecord dbDataRecord, IEnumerable<PropertyInfo> properties)
            where TProjected : new()
        {
            var projectedEntity = new TProjected();
            foreach (var propertyInfo in properties)
            {
                var value = dbDataRecord[propertyInfo.Name];

                if (value is DBNull)
                {
                    value = null;
                }

                var propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;
                var newValue = value == null ? null : Convert.ChangeType(value, propertyType);

                propertyInfo.SetValue(projectedEntity, newValue, new object[] { });
            }
            return projectedEntity;
        }
    }
}
