using System.Threading.Tasks;

namespace PhoneDump.Contract.Services
{
    public interface ILauncherService
    {
        Task Launch(string fileName);
    }
}