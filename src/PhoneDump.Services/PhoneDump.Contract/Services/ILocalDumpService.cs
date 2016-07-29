using System.Threading.Tasks;
using PhoneDump.Entity.Dumps;

namespace PhoneDump.Contract.Services
{
    public interface ILocalDumpService
    {
        Task Init();
        Task AddDump(DumpWireEntity dump);
    }
}