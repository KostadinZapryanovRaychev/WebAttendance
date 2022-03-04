using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAttendance.Areas.Identity.Data;
using WebAttendance.Models;

namespace WebAttendance.Data
{
    public class NWDRepository:INWDRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private WebAttendanceDbContext Context { get; }
        public IConfiguration Config { get; }
        private string DeleteMassage { get; set; }
        private string ExceptionMessage { get; set; }
        public NWDRepository(WebAttendanceDbContext context, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            Context = context;
            Config = config;
            ExceptionMessage = config.GetSection("RepositoryExceptionMassage").Value;
        }


        public async Task<string> AddNWD(NonWorkingDays nonWorkingDays)
        {
            Context.NonWorkingDays.Add(nonWorkingDays);
            await Context.SaveChangesAsync();
            return "Inserted";
        }

        public async Task<string> DeleteNWD(int id)
        {
            NonWorkingDays deletedNWD = await Context.NonWorkingDays.FindAsync(id);
            if (deletedNWD != null)
            {
                Context.NonWorkingDays.Remove(deletedNWD);
                await Context.SaveChangesAsync();
                DeleteMassage = "Removed succesfully";
            }

            else
            {
                DeleteMassage = "Something went wrong , try again";
            }

            return DeleteMassage;

        }


        public async Task<string> DeleteNWD(NonWorkingDays nonWorkingDays)
        {
            return await DeleteNWD(nonWorkingDays.Id);
        }


        public async Task<NonWorkingDays> EditNWD(NonWorkingDays nonWorkingDays)
        {
            var existingNWD = await Context.NonWorkingDays.FindAsync(nonWorkingDays.Id);
            if (existingNWD != null)
            {
                existingNWD.Holiday = nonWorkingDays.Holiday;
                Context.NonWorkingDays.Update(existingNWD);
                await Context.SaveChangesAsync();
            }
            return nonWorkingDays;
        }


        public async Task<NonWorkingDays> GetNWD(int id)
        {
            var nonWorkingDays = await Context.NonWorkingDays.FindAsync(id);
            return nonWorkingDays;
        }

        public IEnumerable<NonWorkingDays> GetAllAvailableNWD()
        {
            return Context.NonWorkingDays.ToList();
        }

        public async void Save()
        {
            await Context.SaveChangesAsync();
        }
    }
}
