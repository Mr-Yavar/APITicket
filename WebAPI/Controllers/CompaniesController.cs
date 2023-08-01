using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {

        private readonly ILoggerManager _logger;
        private readonly ICompanyService _companySerivce;
        public CompaniesController(ILoggerManager logger, ICompanyService companySerivce)
        {
            _companySerivce = companySerivce;
            _logger = logger;
        
        }


        [HttpGet]
        public IActionResult GetCompanies()
        {
            var result = _companySerivce.GetAllCompanies();
            return Ok(result);

        }

        [HttpGet("{CompanyId}", Name = "CompanyById")]

        public IActionResult Company(Guid CompanyId)
        {
            var result = _companySerivce.GetCompany(CompanyId);
            if (result == null)
                return NotFound();
            return Ok(result);

        }

    }
}
