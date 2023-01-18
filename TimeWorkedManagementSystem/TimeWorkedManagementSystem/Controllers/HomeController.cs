using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using TimeWorkedManagementSystem.Contexts;
using TimeWorkedManagementSystem.Models;

namespace TimeWorkedManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserDbContext _dbContext;

        public HomeController(ILogger<HomeController> logger, UserDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            return View(await _dbContext.Shifts.Include(s => s.Company).Include(s => s.Breaks).ToListAsync());
        }

        [Authorize]
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