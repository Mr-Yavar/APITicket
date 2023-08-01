using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EmployeeService : ServiceBase, IEmployeeService
    {
        public EmployeeService(IRepositoryManager repositoryManager, IMapper mapper) : base(repositoryManager, mapper)
        {

        }

        public List<EmployeeDto> GetEmployees(Guid companyId)
        {
            var company = _repositoryManager.CompanyRepository.FindByCondition(x => x.Id == companyId, trackChanges: false);
            if (company == null)
            {
                return null;
            }
            var employeesFromDb = _repositoryManager.EmployeeRepository.FindByCondition(x => x.CompanyId == companyId,
           trackChanges: false).ToList();
            var employeesDtos = _mapper.Map<List<EmployeeDto>>(employeesFromDb);
            return employeesDtos;

        }
    }
}
