using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using LoggerService;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/companies/{companyId}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IEmployeeService _employeeService;
        public EmployeesController(ILoggerManager logger, IEmployeeService EmployeeService)
        {
            _employeeService = EmployeeService;
            _logger = logger;

        }

        [HttpGet]
        public IActionResult GetEmployeesForCompany(Guid companyId)
        {
            var result = _employeeService.GetEmployees(companyId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{Id}", Name="GetEmployeeForCompany")]
        public IActionResult GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var result = _employeeService.GetEmployees(companyId).FirstOrDefault(x=>x.Id==id);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult CreateEmployeeForCompany(Guid companyId, [FromBody] EmployeeForCreationDto employee)
        {
            if (employee == null)
            {
                _logger.LogError("EmployeeForCreationDto object sent from client is null.");
                return BadRequest("EmployeeForCreationDto object is null");
            }

            EmployeeDto Dto = null;
            var result = _employeeService.CreateEmployeeForCompany(companyId, employee);
            if (result==null)
                return BadRequest("somethins went wrong!");
            else
                return CreatedAtRoute("GetEmployeeForCompany",new {companyId=result.CompanyId, id = result.Id }, Dto);
            
            
        }


        [HttpDelete("{id}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            var response = _employeeService.DeleteEmployeeForCompany(companyId, id);
            if (response == null)
            {
                _logger.LogError("The Company or the spcific employee Not Found!");
                return NotFound();
            }
            return NoContent();
        }

    }
}
