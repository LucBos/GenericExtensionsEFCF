using System.Data.Entity;

namespace GenericExtensionsEFCF
{
    abstract class QueryValue<TValue, TParameter> : IQueryValue<TValue, TParameter>
    {
        protected DbContext DbContext;

        protected QueryValue(DbContext dbContext)
        {
            DbContext =  dbContext;
        }

        public abstract TValue Get();
    }
}