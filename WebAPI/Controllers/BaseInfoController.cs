using Contracts.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseInfoController : ControllerBase
    {

        private ICategoryService _categoryService;

        public BaseInfoController(ICategoryService categoryService) : base(){
           
            _categoryService = categoryService;
        }


        [HttpGet("[action]")]
        public async  Task<IActionResult> GetCategories()
        {
           var res = await _categoryService.GetCategories();
            return Ok(res);
        }



    }
}
