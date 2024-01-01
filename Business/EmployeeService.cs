using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
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

        public EmployeeModel CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto Emplyee)
        {
            try
            {
                var company = _repositoryManager.CompanyRepository.FindByCondition(x => x.Id == companyId, false).FirstOrDefault();
                EmployeeModel newEmployee = new EmployeeModel();
                newEmployee = _mapper.Map<EmployeeModel>(Emplyee);
                newEmployee.CompanyId = companyId;
                _repositoryManager.EmployeeRepository.Create(newEmployee);
                _repositoryManager.Save();
                return newEmployee;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }


        public EmployeeModel DeleteEmployeeForCompany(Guid companyId, Guid EmplyeeId)
        {
            try
            {
                var Employee = _repositoryManager.EmployeeRepository.FindByCondition(x => x.Id == companyId && x.Id==EmplyeeId, false).FirstOrDefault();

                if(Employee == null)
                        return null;

                _repositoryManager.EmployeeRepository.Delete(Employee);
                _repositoryManager.Save();

                return Employee;
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }
}
