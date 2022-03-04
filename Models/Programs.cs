using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAttendance.Models
{
    public class Programs
    {
        public int Id { get; set; }

        [Display(Name = "Име")]
        public string Name { get; set; }

        [Display(Name = "Образователна степен")]
        public int DegreesId { get; set; }
        public Degrees Degrees { get; set; }
    }
}
