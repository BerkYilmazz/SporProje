using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SporProje.Models;
using System.Diagnostics;

namespace SporProje.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly SporDbContext context;
        public HomeController()
        {
            context = new SporDbContext();
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var topic = await context.Topics.Include(u => u.User).ToListAsync();

            return View(topic);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
