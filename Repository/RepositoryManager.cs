using Contracts.IRepository;
using Entities;
using Repositories.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private DBContextProvider _context;
        private ICategoryRepository _categoryRepository;
      
        public RepositoryManager(DBContextProvider Context)
        {
            _context = Context;
        }
        public ICategoryRepository CompanyRepository
        {
            get
            {
                if (_categoryRepository == null)
                    _categoryRepository = new CategoryRepository(_context);

                return _categoryRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

