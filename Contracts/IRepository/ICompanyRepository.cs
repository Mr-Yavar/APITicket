using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Contracts.IRepository
{
    public interface ICompanyRepository: IRepositoryBase<CompanyModel>
    {
        
    }
}
