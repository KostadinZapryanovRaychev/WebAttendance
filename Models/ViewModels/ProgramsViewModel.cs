using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace WebAttendance.Models.ViewModels
{
    public class ProgramsViewModel
    {
        public Programs Programs { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }

        public int DegreesId { get; set; }

    }
}
