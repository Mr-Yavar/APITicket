using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IService
{
    public interface IEmployeeService : IService
    {
         List<EmployeeDto> GetEmployees(Guid companyId);
         EmployeeModel CreateEmployeeForCompany(Guid companyId,EmployeeForCreationDto Emplyee);
        EmployeeModel DeleteEmployeeForCompany(Guid companyId, Guid EmplyeeId);
    }
}
