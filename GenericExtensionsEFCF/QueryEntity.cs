using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using EF.Extensions;

namespace GenericExtensionsEFCF
{
    public abstract class QueryEntity<TEntity, TParameter> : IQueryEntity<TEntity, TParameter>
        where TEntity : class
    {
        protected Expression<Func<TEntity, bool>> Expression;
        protected DbSet<TEntity> DbSet;

        public TParameter Parameter { get; set; }

        protected QueryEntity(DbContext dbContext)
        {
            DbSet = dbContext.GetDbSetReference<TEntity>();
        }

        public TEntity GetSingle()
        {
            return DbSet.FirstOrDefault(Expression);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet.Where(Expression);
        }
    }
}