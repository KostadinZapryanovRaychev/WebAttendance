using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAttendance.Models;

namespace WebAttendance.Data
{
    public interface IProgramsRepository
    {
        void Save();

        Task<Programs> GetProgram(int id);

        IEnumerable<Programs> GetAllAvailablePrograms();

        IEnumerable<Programs> GetProgramByDegreesId(int DegreesId);

        Task<string> AddProgram(Programs programs);

        Task<string> DeleteProgramById(int id);

        Task<string> DeleteProgram(Programs programs);

        Task<Programs> EditProgram(Programs programs);
    }
}
