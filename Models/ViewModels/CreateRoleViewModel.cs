using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAttendance.Models.ViewModels
{
    public class CreateRoleViewModel
    {
        [Display(Name = "Име")]
        public string Name { get; set; }
        
    }
}
