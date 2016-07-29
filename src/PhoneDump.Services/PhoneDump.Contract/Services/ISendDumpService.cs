using System.Threading.Tasks;
using PhoneDump.Entity.Dumps;
using XamlingCore.Portable.Model.Response;

namespace PhoneDump.Contract.Services
{
    public interface ISendDumpService
    {
        Task<XResult<bool>> SendDump(DumpWireEntity entity);
    }
}