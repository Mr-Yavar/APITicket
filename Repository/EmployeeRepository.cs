using Contracts.IRepository;
using Entities;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories
{
    public class EmployeeRepository: RepositoryBase<EmployeeModel>,IEmployeeRepository
    {
        public EmployeeRepository(DBContextProvider dBContextProvider):base(dBContextProvider)
        { }
    }
}
