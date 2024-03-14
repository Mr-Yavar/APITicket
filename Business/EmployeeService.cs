using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Entities.Models;
using LoggerService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EmployeeService : ServiceBase, IEmployeeService
    {

        ILoggerManager _logger;

        public EmployeeService(IRepositoryManager repositoryManager, IMapper mapper,ILoggerManager logger) : base(repositoryManager, mapper)
        {
            _logger = logger;
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


        public EmployeeModel UpdateEmployee(Guid companyId, Guid id, EmployeeForUpdateDto employee)
        {
         
            var company = _repositoryManager.CompanyRepository.FindByCondition(x=>x.Id==companyId, trackChanges: true).FirstOrDefault();
            if (company == null)
            {
                _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
                return null;
            }
            var employeeEntity = _repositoryManager.EmployeeRepository.FindByCondition(x=>x.CompanyId==companyId && x.Id==id, trackChanges: true).FirstOrDefault();
            if (employeeEntity == null)
            {
                _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
                return null;
            }
            _mapper.Map(employee, employeeEntity);
            _repositoryManager.Save();

            return employeeEntity;
        }
    }
}
