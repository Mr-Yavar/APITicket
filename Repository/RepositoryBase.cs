using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.IRepository;
using Entities;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected DBContextProvider _dbContextProvider;
        public RepositoryBase(DBContextProvider repositoryContext)
        {
            _dbContextProvider = repositoryContext;
        }

        public IQueryable<T> FindAll(bool trackChanges)
        {
            return !trackChanges ? _dbContextProvider.Set<T>().AsNoTracking() :
                _dbContextProvider.Set<T>();
        }
        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges)
        {
            return !trackChanges ? _dbContextProvider.Set<T>().Where(expression).AsNoTracking() :
                _dbContextProvider.Set<T>().Where(expression);
        }
        public void Create(T entity)
        {
            try
            {
                _dbContextProvider.Set<T>().Add(entity);
            }
            catch (Exception e)
            {

            }
        }
        public void Update(T entity)
        {
            try
            {
                _dbContextProvider.Set<T>().Update(entity);
            }
            catch (Exception e)
            {

            }
        }
        public void Delete(T entity)
        {
            try
            {
                _dbContextProvider.Set<T>().Remove(entity);
            }
            catch (Exception e)
            {

            }
        }
    }
}
