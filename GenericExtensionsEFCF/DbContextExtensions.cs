using System.Data.Entity;
using System.Linq;
using System.Reflection;

namespace GenericExtensionsEFCF
{
    public static class DbContextExtensions
    {
        public static DbSet<TEntity> GetDbSetReference<TEntity>(this DbContext dbContext) where TEntity : class
        {
            var dbContextType = dbContext.GetType();
            var properties = dbContextType.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var property = properties.FirstOrDefault(p => typeof(DbSet<TEntity>) == p.PropertyType);

            return property.GetValue(dbContext, new object[] { }) as DbSet<TEntity>;
        }
    }
}