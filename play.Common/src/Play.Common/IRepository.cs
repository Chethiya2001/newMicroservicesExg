

using System.Linq.Expressions;

namespace Play.Common
{
    public interface IRepository<T> where T : IEntity
    {
        Task CreateAsync(T item);
        Task<IReadOnlyCollection<T>> GetAllAsync();
        Task<IReadOnlyCollection<T>> GetAllAsync(Expression<Func<T, bool>> filter);
        Task<T> GetItems(Guid id);
        Task<T> GetItems(Expression<Func<T, bool>> filter);
        Task RemoveAync(Guid id);
        Task UpdateAsync(T item);
    }
}