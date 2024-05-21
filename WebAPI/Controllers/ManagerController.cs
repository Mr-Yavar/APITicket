using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class ManagerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
