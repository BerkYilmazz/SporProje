using Microsoft.AspNetCore.Mvc;

namespace SporProje.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
