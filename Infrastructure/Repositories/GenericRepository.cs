using Infrastructure.AppDBContext;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using ShipmentManagement.Application.Interfaces;

using System.Data;
using System.Linq.Expressions;

namespace ShipmentManagement.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly DbSet<T> _dbset;
        public GenericRepository(ApplicationDbContext appDbContext)
        {
            _dbContext = appDbContext;
            _dbset = appDbContext.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            //TrimEntityProperties(entity);
            await _dbset.AddAsync(entity);
        }

        public async Task<T?> GetAsync(Expression<Func<T, bool>> predicateFilters, params Expression<Func<T, bool>>[] predicateInclude)
        {
            IQueryable<T> query = _dbset.AsNoTracking();
            query = query.Where(predicateFilters);
            var sql = query.ToQueryString();
            query = predicateInclude.Aggregate(query, (current, include) => current.Include(include));
            var entity = await query.FirstOrDefaultAsync();
            //if (entity != null) TrimEntityProperties(entity);
            return entity;

        }
        public void Update(T entity)
        {
            //TrimEntityProperties(entity);
            _dbset.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Detach(T entity)
        {
            _dbContext.Entry(entity).State = EntityState.Detached;
        }
        public async Task<bool> DeleteAsync(T entity)
        {
            _dbContext.Remove(entity);
            return true;
        }


        public async Task<IList<T>> GetAllAsync(Expression<Func<T, bool>>? predicateFilters = null, params Expression<Func<T, bool>>[] predicateInclude)
        {
            IQueryable<T> query = _dbset.AsNoTracking();
            query = query.Where(predicateFilters);
            var sql = query.ToQueryString();
            query = predicateInclude.Aggregate(query, (current, include) => current.Include(include));
            List<T> entities = await query.ToListAsync();
            return entities;
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task<(SqlParameter[] OutputParameters, DataTable ResultSet, List<string> Messages)?> ExecuteStoredProcedureInTransactionAsync(string storedProcedure, SqlParameter[]? parameters = null)
        {
            var resultSet = new DataTable();
            var messages = new List<string>();
            var outputParameters = new List<SqlParameter>();

            using (var connection = new SqlConnection(_dbContext.Database.GetDbConnection().ConnectionString))
            {
                using (var command = new SqlCommand(storedProcedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            if (param.Value is string stringValue)
                            {
                                param.Value = stringValue.Trim();
                            }
                        }
                        command.Parameters.AddRange(parameters);
                    }

                    // Event handler for SQL Server messages (e.g., PRINT statements)
                    connection.InfoMessage += (sender, e) =>
                    {
                        messages.AddRange(e.Errors.Cast<SqlError>().Select(err => err.Message));
                    };

                    try
                    {
                        await connection.OpenAsync();

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                resultSet.Load(reader);
                            }
                        }

                        // Extract output parameters
                        //outputParameters.AddRange(parameters.Where(p => p.Direction == ParameterDirection.Output));
                        if (parameters != null)
                        {
                            outputParameters.AddRange(parameters.Where(p => p.Direction == ParameterDirection.Output));
                        }

                        return (outputParameters.ToArray(), resultSet, messages);
                    }
                    catch (SqlException ex)
                    {
                        // Handle SQL exceptions here or rethrow as needed
                        throw new ApplicationException(ex.Message);
                    }
                    finally
                    {
                        await connection.CloseAsync();
                    }
                }
            }
        }


        //void IGenericRepository<T>.TrimEntityProperties(T entity)
        //{
        //    if (entity == null) return;

        //    var stringProperties = typeof(T).GetProperties()
        //                                     .Where(p => p.PropertyType == typeof(string) && p.CanRead && p.CanWrite);

        //    foreach (var property in stringProperties)
        //    {
        //        var currentValue = property.GetValue(entity) as string;
        //        if (!string.IsNullOrEmpty(currentValue))
        //        {
        //            var trimmedValue = currentValue.Trim();
        //            property.SetValue(entity, trimmedValue);
        //        }
        //    }
        //}
    }
}