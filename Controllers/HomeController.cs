using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAttendance.Data;
using WebAttendance.Models;

namespace WebAttendance.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private IAttendanceRepository Repo { get; }
        public List<Semester> GetAllSemesterDisplay()
        {
            List<Semester> semNames = Repo.GetAllSemesters().ToList();
            return semNames;
        }

        public HomeController(ILogger<HomeController> logger, IAttendanceRepository repo)
        {
            _logger = logger;
            Repo = repo;
        }

        [Authorize]
        public IActionResult Index(string value)
        {
            ViewBag.semNames = new SelectList(GetAllSemesterDisplay(), "Id", "Name");
            TempData["name"] = value;

            return View();
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
