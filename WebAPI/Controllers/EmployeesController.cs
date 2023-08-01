using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
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
    }
}
