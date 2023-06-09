﻿namespace Fresh.DataAccess.Interfaces.Repositories
{
    public interface IGenericRepository<T>
    {
        Task<bool> CreateAsync(T item);
        Task<bool> UpdateAsync(int id, T entity);
        Task<bool> DeleteAsync(int id);
        Task<T> GetByIdAsync(int id);
        Task<IList<T>> GetAllLimit(int skip, int take);
        Task<IList<T>> GetAllAsync();
    }
}
