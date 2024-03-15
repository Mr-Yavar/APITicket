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
        private ICompanyRepository _companyRepository;
        private IEmployeeRepository _employeeRepository;
        public RepositoryManager(DBContextProvider Context)
        {
            _context = Context;
        }
        public ICompanyRepository CompanyRepository
        {
            get
            {
                if (_companyRepository == null)
                    _companyRepository = new CompanyRepository(_context);

                return _companyRepository;
            }
        }
        public IEmployeeRepository EmployeeRepository
        {
            get
            {
                if (_employeeRepository == null)
                    _employeeRepository = new EmployeeRepository(_context);
              
                return _employeeRepository;
            }
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

