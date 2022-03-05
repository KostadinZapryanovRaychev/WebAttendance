using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAttendance.Data;
using WebAttendance.Models;
using WebAttendance.Models.ViewModels;

namespace WebAttendance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProgramsController : Controller
    {
        private IProgramsRepository Repo { get; }

        private IAttendanceRepository _Repo { get; }
        public ProgramsController(IProgramsRepository repo, IAttendanceRepository repository)
        {
            Repo = repo;
            _Repo = repository;
        }

        public List<Degrees> GetAllDegrees()
        {
            List<Degrees> degreesNames = _Repo.LetGetDegrees().ToList();
            return degreesNames;

        }
        public IActionResult Index(int DegreesId)
        {
            ViewBag.degreesNames = new SelectList(GetAllDegrees(), "Id", "Name");

            IEnumerable<Programs> programs;
            if (DegreesId > 0)
            {
                programs = Repo.GetProgramByDegreesId(DegreesId);
            }
            else
            {
                programs = Repo.GetAllAvailablePrograms();
            }

            return View(programs);
        }

        [HttpGet]
        public async Task<IActionResult> EditProgram(int ProgramsId)

        {
            ViewBag.degreesNames = new SelectList(GetAllDegrees(), "Id", "Name");
            var existingPrograms = await Repo.GetProgram(ProgramsId);
            var viewModel = new ProgramsViewModel { Programs = existingPrograms };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditProgram(ProgramsViewModel updatedProgram)

        {
            ViewBag.degreesNames = new SelectList(GetAllDegrees(), "Id", "Name");
            if (ModelState.IsValid)
            {
                var existingProgram = updatedProgram.Programs;
                await Repo.EditProgram(existingProgram);
            }
            return View(updatedProgram);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProgram(int ProgramsId)
        {
            var deleteMessage = await Repo.DeleteProgramById(ProgramsId);
            return Content(deleteMessage);
        }

        [HttpGet]
        public IActionResult CreateProgram()
        {
            ViewBag.degreesNames = new SelectList(GetAllDegrees(), "Id", "Name");
            var viewModel = new ProgramsViewModel();
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProgram(ProgramsViewModel programsViewModel)
        {
            ViewBag.degreesNames = new SelectList(GetAllDegrees(), "Id", "Name");
            if (ModelState.IsValid)
            {
                await Repo.AddProgram(programsViewModel.Programs);
                return RedirectToAction("CreateProgram");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetProgram(int id)
        {
            var prog = Repo.GetProgram(id);

            if (prog != null)
            {
                return View(prog);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetPrograms()
        {
            var programs = Repo.GetAllAvailablePrograms();
            return View(programs);
        }
    }
}
