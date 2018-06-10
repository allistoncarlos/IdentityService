using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using IdentityService.Domain;

namespace IdentityService.Repository
{
    public interface IRepository<in TPrimary, T> : IDisposable
        where TPrimary : class
        where T : IEntity
    {
        Task<T> Get(TPrimary id);

        Task<T> Get(Expression<Func<T, bool>> predicate);

        Task<IQueryable<T>> Load(Expression<Func<T, bool>> predicate);

        Task Delete(TPrimary id);

        Task<T> Update(T entity);

        Task<T> Save(T entity);
    }
}
