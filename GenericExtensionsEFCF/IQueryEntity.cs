using System.Collections.Generic;

namespace GenericExtensionsEFCF
{
    public interface IQueryEntity<out TEntity, TParameter>
    {
        TParameter Parameter { get; set; }
        TEntity GetSingle();
        IEnumerable<TEntity> GetAll();
    }
}