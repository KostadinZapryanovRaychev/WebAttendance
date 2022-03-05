﻿using Microsoft.AspNetCore.Authorization;
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
    public class SemesterController : Controller
    {
        private ISemesterRepository Repo { get; }

        private IAttendanceRepository _Repo { get; }


        public SemesterController(ISemesterRepository repo, IAttendanceRepository repository)
        {
            Repo = repo;
            _Repo = repository;

        }
        public IActionResult Index()
        {
            var semesters = Repo.GetAllAvailableSemesters();
            var result = semesters.OrderByDescending(x => x.Id);
            return View(result);
        }


        [HttpGet]
        public async Task<IActionResult> EditSemester(int SemesterId)

        {
            var existingSemester = await Repo.GetSemester(SemesterId);
            var viewModel = new SemesterViewModel { Semester = existingSemester };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> EditSemester(SemesterViewModel modifiedSemester)

        {
            if (ModelState.IsValid)
            {
                var existingSemester = modifiedSemester.Semester;
                await Repo.EditSemester(existingSemester);
            }
            return View(modifiedSemester);
        }


        [HttpPost]
        public async Task<IActionResult> DeleteSemester(int SemesterId)
        {
            var deleteMessage = await Repo.DeleteSemesterById(SemesterId);
            return Content(deleteMessage);
        }


        [HttpGet]
        public IActionResult CreateSemester()
        {
            var viewModel = new SemesterViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSemester(SemesterViewModel semesterViewModel)
        {
            if (ModelState.IsValid)
            {
                await Repo.AddSemester(semesterViewModel.Semester);
                return RedirectToAction("CreateSemester");
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult GetSemester(int id)
        {
            var sem = Repo.GetSemester(id);

            if (sem != null)
            {
                return View(sem);
            }
            return NotFound();
        }

    }
}
