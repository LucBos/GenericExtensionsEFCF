using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using GenericExtensionsEFCF;

namespace EF.Extensions
{
    /// <summary>
    /// http://www.lucbos.net/2012/03/calling-stored-procedure-with-entity.html
    /// </summary>
    public static class DatabaseExtensions
    {
        private const bool RetrieveNameFromResultDto = false;

        public static IEnumerable<TResult> ExecuteStoredProcedure<TResult>(this Database database, IStoredProcedure<TResult> procedure)
        {
            var parameters = CreateSqlParametersFromProperties(procedure);

            var sqlCommand = CreateSPCommand<TResult>(procedure.GetType().Name, parameters);

            return database.SqlQuery<TResult>(sqlCommand, parameters.Cast<object>().ToArray());
        }

        private static List<SqlParameter> CreateSqlParametersFromProperties<TResult>(IStoredProcedure<TResult> procedure)
        {
            var procedureType = procedure.GetType();
            var propertiesOfProcedure = procedureType.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var parameters =
                propertiesOfProcedure.Select(propertyInfo => ConvertPropertyToSpParameter(procedure, propertyInfo))
                    .ToList();
            return parameters;
        }

        private static SqlParameter ConvertPropertyToSpParameter<TResult>(IStoredProcedure<TResult> procedure, PropertyInfo propertyInfo)
        {
            return new SqlParameter(string.Format("@{0}", (object)propertyInfo.Name),
                                    propertyInfo.GetValue(procedure, new object[] { }));
        }

        private static string CreateSPCommand<TResult>(string storedProcedureName, List<SqlParameter> parameters)
        {
            string queryString;

            queryString = RetrieveNameFromResultDto ? CreateStoredProcedureName<TResult>() : string.Format("sp_{0}", storedProcedureName);
            parameters.ForEach(x => queryString = string.Format("{0} {1},", queryString, x.ParameterName));

            return queryString.TrimEnd(',');
        }

        private static string CreateStoredProcedureName<TResult>()
        {
            var name = typeof(TResult).Name;

            if (name.EndsWith("Result"))
            {
                name = name.Substring(0, name.LastIndexOf("Result"));
            }

            string storedProcedureName = string.Format("sp_{0}", name);
            return storedProcedureName;
        }
    }
}