using Entities.DataTransferObjects;
using Entities.Models;
using Features.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.IService
{
    public interface IEmployeeService : IService
    {
        Task<List<EmployeeDto>> GetEmployeesAsync(Guid companyId);
        Task<(IEnumerable<EmployeeDto> employees, MetaData metaData)> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters);
        EmployeeModel CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto Emplyee);
        EmployeeModel DeleteEmployeeForCompany(Guid companyId, Guid EmplyeeId);

        EmployeeModel UpdateEmployee(Guid companyId, Guid id, EmployeeForUpdateDto employee);
    }
}
