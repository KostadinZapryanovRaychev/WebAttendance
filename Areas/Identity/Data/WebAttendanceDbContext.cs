using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebAttendance.Areas.Identity.Data;
using WebAttendance.Data.Models;
using WebAttendance.Models;

namespace WebAttendance.Areas.Identity.Data
{
    public class WebAttendanceDbContext : IdentityDbContext<ApplicationUser, ApplicationUserRole, int>
    {
        public WebAttendanceDbContext(DbContextOptions<WebAttendanceDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }

        public DbSet<Programs> Programs { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<NonWorkingDays> NonWorkingDays { get; set; }
        public DbSet<Degrees> Degrees { get; set; }
        public DbSet<Semester> Semesters { get; set; }
    }
}
