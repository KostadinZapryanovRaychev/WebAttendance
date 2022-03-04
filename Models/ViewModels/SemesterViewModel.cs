﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebAttendance.Models.ViewModels
{
    public class SemesterViewModel
    {
        public Semester Semester { get; set; }
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime startDate { get; set; }

        public DateTime endDate { get; set; }

    }
}
