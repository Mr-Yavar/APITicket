using Asp.Versioning;
using Azure;
using Contracts.IService;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.X86;
using System;
using LoggerService;

namespace WebAPI.Controllers
{

        [ApiVersion("2.0")]
        [Route("api/companies")]
        [ApiController]
        public class CompaniesV2Controller : ControllerBase
        {
            private readonly ILoggerManager _logger;
            private readonly ICompanyService _companySerivce;
            public CompaniesV2Controller(ILoggerManager logger, ICompanyService companySerivce)
            {
                _companySerivce = companySerivce;
                _logger = logger;

            }
            [HttpGet]
            public async Task<IActionResult> GetCompanies()
            {
                
            var companies = _companySerivce.GetAllCompanies();
                var companiesV2 = companies.Select(x => $"{x.Name} V2").ToList();
                return Ok(companiesV2);
            }
        


    }
}