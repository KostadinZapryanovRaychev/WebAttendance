using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAttendance.Areas.Identity.Data;
using WebAttendance.Models;
using WebAttendance.Models.ViewModels;

namespace WebAttendance.Data
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private WebAttendanceDbContext Context { get; }
        public IConfiguration Config { get; }
        private string DeleteMassage { get; set; }
        private string ExceptionMessage { get; set; }

        public AttendanceRepository(WebAttendanceDbContext context, IConfiguration config, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            Context = context;
            Config = config;
            ExceptionMessage = config.GetSection("RepositoryExceptionMassage").Value;

        }
        public async Task<string> AddAttendance(Attendance attendance)
        {

            Context.Attendances.Add(attendance);
            await Context.SaveChangesAsync();
            return "Inserted";
        }

        public async Task<string> CoppyAttendance(Attendance attendance)
        {
            var originalEntity = Context.Attendances.AsNoTracking().FirstOrDefault(e => e.Id == attendance.Id);
            Context.Attendances.Add(originalEntity);
            await Context.SaveChangesAsync();
            return "Inserted";
        }

        public async Task<string> DeleteAttendance(int id)
        {
            Attendance deletedAttendance = await Context.Attendances.FindAsync(id);
            if (deletedAttendance != null)
            {
                Context.Attendances.Remove(deletedAttendance);
                await Context.SaveChangesAsync();
                DeleteMassage = "Изтриването успешно";
            }

            else
            {
                DeleteMassage = "Грешка";
            }

            return DeleteMassage;

        }

        public async Task<string> DeleteAttendance(Attendance attendance)
        {
            return await DeleteAttendance(attendance.Id);
        }

        public async Task<Attendance> EditAttendance(Attendance attendance)
        {
            var existingAttend = await Context.Attendances.FindAsync(attendance.Id);

            if (existingAttend != null)
            {
                existingAttend.Date = attendance.Date;
                existingAttend.Hours = attendance.Hours;
                existingAttend.Groupe = attendance.Groupe;
                existingAttend.Type = attendance.Type;
                existingAttend.Mode = attendance.Mode;
                existingAttend.Timeframe = attendance.Timeframe;
                existingAttend.Subjects = attendance.Subjects;
                existingAttend.Auditorium = attendance.Auditorium;
                existingAttend.Degree = attendance.Degree;
                existingAttend.Programs = attendance.Programs;
                existingAttend.Note = attendance.Note;
                existingAttend.Course = attendance.Course;


                Context.Attendances.Update(existingAttend);
                await Context.SaveChangesAsync();
            }
            return attendance;

        }

        public IEnumerable<Attendance> GetAttendances()
        {
            return Context.Attendances.ToList();
        }

        public IEnumerable<Attendance> FindAttendanceByUserId(int ApplicationUserId, string Mode = null)
        {

            if (Mode == null || Mode == "")
            {
                return Context.Attendances.Where(a => a.ApplicationUserId == ApplicationUserId);
            }
            else
            {
                return Context.Attendances.Where(a => a.ApplicationUserId == ApplicationUserId && a.Mode == Mode);
            }
        }

        public IEnumerable<Attendance> FindAttendanceBySemesterId(int SemesterId)
        {
            return Context.Attendances.Where(a => a.SemesterId == SemesterId);
        }

        public IEnumerable<Attendance> FindAttendanceBySemesterIdandUserId(int ApplicationUserId, int SemesterId, string Mode = null)
        {
            if (Mode == null || Mode == "")
            {
                return Context.Attendances.Where(a => a.ApplicationUserId == ApplicationUserId && a.SemesterId == SemesterId);
            }
            else
            {
                return Context.Attendances.Where(a => a.ApplicationUserId == ApplicationUserId && a.SemesterId == SemesterId && a.Mode == Mode);

            }
        }

        public async Task<Attendance> GetAttendanceByUserId(int ApplicationUserId)
        {
            var attend = await Context.Attendances.FindAsync(ApplicationUserId);
            return attend;
        }

        public async Task<Attendance> GetAttendanceByMode(string Mode)
        {

            var attendanceByMode = from attendance in Context.Attendances where attendance.Mode == Mode select attendance;
            var attendancesMode = await Context.Attendances.FindAsync(Mode);
            return attendancesMode;
        }

        public async Task<Attendance> GetAttendanceBySemesterId(int ApplicationUserId, int SemesterId)
        {
            var attend = await Context.Attendances.FindAsync(ApplicationUserId, SemesterId);
            return attend;
        }

        public async Task<Attendance> GetAttendance(int id)
        {
            var attend = await Context.Attendances.FindAsync(id);
            return attend;
        }

        public void Detached(Attendance attendanceEntity)
        {
            Context.Entry(attendanceEntity).State = EntityState.Detached;
            attendanceEntity.Id = 0;
        }

        public void ReDetached(Attendance attendanceEntity)
        {
            Context.Entry(attendanceEntity).State = EntityState.Detached;
            attendanceEntity.Id = 0;
        }

        public async Task<string> AddAttendanceWithoutHolidays(Attendance attendance)
        {

            var result = Context.NonWorkingDays.ToList();
            var holidays = new List<DateTime>();

            foreach (var holiday in result)
            {

                holidays.Add(holiday.Holiday);
            };

            if (!holidays.Contains(attendance.Date))
            {
                Context.Attendances.Add(attendance);
                await Context.SaveChangesAsync();
                return "Inserted";
            }
            return "Success";

        }


        public IList<Programs> LetGetPrograms()
        {
            var programsList = from Programs in Context.Programs select Programs;
            var programsNames = programsList.ToList<Programs>();
            return programsNames;
        }

        public IList<Degrees> LetGetDegrees()
        {
            var degreeslist = from Degrees in Context.Degrees select Degrees;
            var degreenames = degreeslist.ToList<Degrees>();
            return degreenames;
        }


        public IList<Programs> GetProgramsByDegreesId(int DegreesId)
        {
            var programsList = from programs in Context.Programs where programs.DegreesId == DegreesId select programs;
            var programsNames = programsList.ToList<Programs>();
            var onlyNames = new List<string>();

            foreach (var onlyName in programsNames)
            {
                onlyNames.Add(onlyName.Name);
            };

            return programsNames;
        }


        public IList<Semester> GetAllSemesters()
        {
            var allSemesters = from semester in Context.Semesters select semester;
            var allSemestersNames = allSemesters.ToList<Semester>();
            return allSemestersNames;
        }

        public DateTime SemesterEndDateById(int id)
        {
            var semester = Context.Semesters.FirstOrDefault(x => x.Id == id);
            return semester.endDate;
        }

        public Semester GetCurrentSemester()
        {
            DateTime now = DateTime.Now;
            return Context.Semesters.FirstOrDefault(x => x.startDate.AddDays(-3) <= now && x.endDate.AddDays(10) >= now);
        }

        public Semester GetCurrentSemesterId()
        {
            DateTime now = DateTime.Now;
            var currentSem = Context.Semesters.FirstOrDefault(x => x.startDate <= now && x.endDate.AddDays(10) >= now);
            return currentSem;
        }

    }
}
