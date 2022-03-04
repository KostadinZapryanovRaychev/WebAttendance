﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAttendance.Models
{
    public class Semester
    {
        public int Id { get; set; }

        [Display(Name = "Семестър")]

        public string  Name { get; set; }

        [Display(Name = "Начална дата за редовно обучение")]
        [Column(TypeName = "date")]
        public DateTime startDate { get; set; }

        [Display(Name = "Крайна дата за редовно обучение")]
        [Column(TypeName = "date")]
        public DateTime endDate { get; set; }

    }
}
