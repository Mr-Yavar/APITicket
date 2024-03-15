using AutoMapper;
using Contracts.IRepository;
using Contracts.IService;
using Entities.DataTransferObjects;
using LoggerService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using System;
using System.Collections.Generic;
using WebAPI.ModelBinders;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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
        // [ResponseCache(Duration = 180)]
        [OutputCache(Duration = 60)]

        public IActionResult GetCompanies()
        {
            var result = _companySerivce.GetAllCompanies();
            return Ok(result);

        }


        //Caching OutPut by Key

        //[HttpGet("output-varybykey")] 
        //[OutputCache(VaryByQueryKeys = new[] { nameof(firstKey) })]
        //public IActionResult VaryByKey(string firstKey, string secondKey)
        //{
        //    return Ok($"{firstKey} {secondKey} - retrieved at {DateTime.Now}");
        //}

        [HttpGet("{CompanyId}", Name = "CompanyById")]

        public IActionResult Company(Guid CompanyId)
        {
            var result = _companySerivce.GetCompany(CompanyId);
            if (result == null)
                return NotFound();
            return Ok(result);

        }

        [HttpPost(Name = "CreateCompany")]
        public IActionResult CreateCompany([FromBody] CompanyForCreationDto Company)
        {
            if (Company == null)
            {
                return BadRequest("Company could not be empty!");
            }
            var result = _companySerivce.CreateCompany(Company);
            return CreatedAtRoute("CompanyById", new { id = result.Id }, result);

        }

        [HttpGet("collection/({ids})", Name = "CompanyCollection")]
        public IActionResult GetCompanyCollection([ModelBinder(BinderType = typeof(ArrayModelBinder))] IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }

            var companyEntities = _companySerivce.GetCompanyCollectionByIds(ids);
            if (ids.Count() != companyEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }

            return Ok(companyEntities);
        }

        [HttpPost("collection")]
        public IActionResult CreateCompanyCollection([FromBody] IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection == null)
            {
                _logger.LogError("Company collection sent from client is null.");
                return BadRequest("Company collection is null");
            }

            var CompaniestoReturn = _companySerivce.CreateCompanyCollection(companyCollection);
            var ids = string.Join(",", CompaniestoReturn.Select(c => c.Id));
            return CreatedAtRoute("CompanyCollection", new { ids }, CompaniestoReturn);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteCompany(Guid id)
        {


            if (!_companySerivce.DeleteCompany(id))
                return NotFound();
            return NoContent();
        }
    }
}
