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
        private IPromoterService _promoterService;
        private IPlaceService _placeService;
        private IHallService _hallService;
        private IDynParameterService _dynParameterService;

        public BaseInfoController(ICategoryService categoryService,
                                  IPromoterService promoterService,
                                  IPlaceService placeService,
                                  IHallService hallService
            , IDynParameterService dynParameterService
                                  ) : base(){
           
            _categoryService = categoryService;
            _promoterService = promoterService;
            _placeService = placeService;   
            _hallService = hallService;
            _dynParameterService = dynParameterService;
        }


        [HttpGet("[action]")]
        public async  Task<IActionResult> GetCategories()
        {
           var res = await _categoryService.GetCategories();
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPlaces()
        {
            var res = await _placeService.GetPlaces();
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetHallsByPlaceId(Guid Id)
        {
            var res = await _hallService.GetHallsByPlaceId(Id);
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetHallsAndFloorsByPlaceId(Guid Id)
        {
            var res = await _hallService.GetHallsWithSeatsByPlaceId(Id);
            return Ok(res);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetPromoters()
        {
            var res = await _promoterService.GetPromoters();
            return Ok(res);
        }


        [HttpGet("[action]/{Key}")]
        public async Task<IActionResult> GetParametersByKey(string Key)
        {
            var res = await _dynParameterService.GetChildrenByParentKey(Key);
            return Ok(res);
        }
    }
}
