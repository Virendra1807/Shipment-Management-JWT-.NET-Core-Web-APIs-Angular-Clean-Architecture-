using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace ShipmentManagement.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task AddAsync(T entity);

        Task<T?> GetAsync(Expression<Func<T, bool>> predicateFilters, params Expression<Func<T, bool>>[] predicateInclude);

        void Update(T entity);

        void Detach(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicateFilters = null, params Expression<Func<T, bool>>[] predicateInclude);
        Task SaveChangesAsync();
        Task<(SqlParameter[] OutputParameters, DataTable ResultSet, List<string> Messages)?> ExecuteStoredProcedureInTransactionAsync(string storedProcedure, SqlParameter[]? parameters = null);

        //void TrimEntityProperties(T entity);
        }
    }