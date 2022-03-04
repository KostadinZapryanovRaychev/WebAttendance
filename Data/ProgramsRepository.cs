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
    public class ProgramsRepository : IProgramsRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private WebAttendanceDbContext Context { get; }
        public IConfiguration Config { get; }
        private string DeleteMassage { get; set; }
        private string ExceptionMessage { get; set; }
        public ProgramsRepository(WebAttendanceDbContext context, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            Context = context;
            Config = config;
            ExceptionMessage = config.GetSection("RepositoryExceptionMassage").Value;
        }

        public async Task<Programs> GetProgram(int id)
        {
            var program = await Context.Programs.FindAsync(id);
            return program;
        }

        public IEnumerable<Programs> GetAllAvailablePrograms()
        {
            return Context.Programs.ToList();
        }

        public IEnumerable<Programs> GetProgramByDegreesId(int DegreesId)
        {
            return Context.Programs.Where(a => a.DegreesId == DegreesId);
        }

        public async Task<string> AddProgram(Programs programs)
        {
            Context.Programs.Add(programs);
            await Context.SaveChangesAsync();
            return "Inserted";
        }

        public async Task<string> DeleteProgramById(int id)
        {
            Programs deletedProgram = await Context.Programs.FindAsync(id);
            if (deletedProgram != null)
            {
                Context.Programs.Remove(deletedProgram);
                await Context.SaveChangesAsync();
                DeleteMassage = "Removed succesfully";
            }

            else
            {
                DeleteMassage = "Something went wrong , try again";
            }

            return DeleteMassage;
        }

        public async Task<string> DeleteProgram(Programs programs)
        {
            return await DeleteProgramById(programs.Id);
        }

        public async Task<Programs> EditProgram(Programs programs)
        {
            var existingProgram = await Context.Programs.FindAsync(programs.Id);
            if (existingProgram != null)
            {
                existingProgram.Name = programs.Name;
                existingProgram.DegreesId = programs.DegreesId;
                Context.Programs.Update(existingProgram);
                await Context.SaveChangesAsync();
            }
            return programs;
        }

        public async void Save()
        {
            await Context.SaveChangesAsync();
        }
    }
}
