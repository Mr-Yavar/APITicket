using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IService
{
    public interface ICompanyService : IService
    { 
        List<CompanyDto> GetAllCompanies();

        CompanyDto GetCompany(Guid CompanyID);
        CompanyDto CreateCompany(CompanyForCreationDto Company);
    }
}
