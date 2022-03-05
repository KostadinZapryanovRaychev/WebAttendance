using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAttendance.Models;

namespace WebAttendance.Data
{
    public interface IAttendanceRepository
    {
        Task<string> AddAttendance(Attendance attendance);

        Task<string> CoppyAttendance(Attendance attendance);

        Task<string> DeleteAttendance(int id);

        Task<string> DeleteAttendance(Attendance attendance);

        Task<Attendance> EditAttendance(Attendance attendance);

        IEnumerable<Attendance> GetAttendances();

        IEnumerable<Attendance> FindAttendanceByUserId(int ApplicationUserId, string Mode = null);

        IEnumerable<Attendance> FindAttendanceBySemesterId(int SemesterId);

        IEnumerable<Attendance> FindAttendanceBySemesterIdandUserId(int ApplicationUserId, int SemesterId, string Mode = null);

        Task<Attendance> GetAttendanceByUserId(int ApplicationUserId);

        Task<Attendance> GetAttendanceByMode(string Mode);

        Task<Attendance> GetAttendanceBySemesterId(int ApplicationUserId, int SemesterId);

        Task<Attendance> GetAttendance(int id);

        void ReDetached(Attendance attendanceEntity);

        void Detached(Attendance attendanceEntity);

        Task<string> AddAttendanceWithoutHolidays(Attendance attendance);

        IList<Programs> LetGetPrograms();

        IList<Degrees> LetGetDegrees();

        IList<Programs> GetProgramsByDegreesId(int DegreesId);

        IList<Semester> GetAllSemesters();

        DateTime SemesterEndDateById(int id);

        Semester GetCurrentSemester();

        Semester GetCurrentSemesterId();
    }
}
