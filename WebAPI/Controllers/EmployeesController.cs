using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using Features.RequestFeatures;
using LoggerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

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

   
        [HttpGet( Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, [FromQuery] EmployeeParameters employeeParameters)
        {
            var result = await _employeeService.GetEmployeesAsync(companyId , employeeParameters);
            if(result == (null,null))
                return NotFound();
            Response.Headers.Add("X-Pagination",JsonSerializer.Serialize(result.metaData));

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
            if (result == null)
                return BadRequest("somethins went wrong!");
            else
                return CreatedAtRoute("GetEmployeeForCompany", new { companyId = result.CompanyId, id = result.Id }, Dto);


        }


        [HttpDelete("{id}")]
        public IActionResult DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            var response = _employeeService.DeleteEmployeeForCompany(companyId, id);
            if (response == null)
            {
                _logger.LogError("The Company or the spcific employee Not Found!");
                return NotFound();//404
            }
            return NoContent();//204 
        }


        [HttpPut("{id}")]
        public IActionResult UpdateEmployeeForCompany(Guid companyId, Guid id, [FromBody] EmployeeForUpdateDto employee)
        {
            if (employee == null)
            {
                _logger.LogError("EmployeeForUpdateDto object sent from client is null.");
                return BadRequest("EmployeeForUpdateDto object is null");
            }

            if (_employeeService.UpdateEmployee(companyId, id, employee) == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
