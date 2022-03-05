using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAttendance.Data;
using WebAttendance.Models.ViewModels;

namespace WebAttendance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class NonWorkingDaysController : Controller
    {
        private IProgramsRepository Repo { get; }

        private IAttendanceRepository _Repo { get; }

        private INWDRepository nWDRepo { get; }
        public NonWorkingDaysController(IProgramsRepository repo, IAttendanceRepository repository, INWDRepository nWDRepository)
        {
            Repo = repo;
            _Repo = repository;
            nWDRepo = nWDRepository;
        }
        public IActionResult Index()
        {
            var nonworkingdays = nWDRepo.GetAllAvailableNWD();
            return View(nonworkingdays);
        }

        [HttpGet]
        public async Task<IActionResult> EditNonWorkingDays(int NWDId)

        {
            var existingNWD = await nWDRepo.GetNWD(NWDId);
            var viewModel = new NonWorkingDaysViewModel { NonWorkingDays = existingNWD };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditNonWorkingDays(NonWorkingDaysViewModel nonWorkingDaysViewModel)

        {
            if (ModelState.IsValid)
            {
                var existingNWD = nonWorkingDaysViewModel.NonWorkingDays;
                await nWDRepo.EditNWD(existingNWD);
            }
            return View(nonWorkingDaysViewModel);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteNWD(int NWDId)
        {
            var deleteMessage = await nWDRepo.DeleteNWD(NWDId);
            return Content(deleteMessage);
        }


        [HttpGet]

        public IActionResult CreateNonWorkingDays()
        {
            var viewModel = new NonWorkingDaysViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNonWorkingDays(NonWorkingDaysViewModel nonWorkingDaysViewModel)
        {
            if (ModelState.IsValid)
            {
                await nWDRepo.AddNWD(nonWorkingDaysViewModel.NonWorkingDays);
                return RedirectToAction("CreateNonWorkingDays");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetNWD(int id)
        {
            var dis = nWDRepo.GetNWD(id);

            if (dis != null)
            {
                return View(dis);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetNWD()
        {
            var nonworkingdays = nWDRepo.GetAllAvailableNWD();
            return View(nonworkingdays);
        }

    }
}
