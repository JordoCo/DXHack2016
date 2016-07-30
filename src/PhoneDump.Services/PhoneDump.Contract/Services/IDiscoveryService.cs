using System.Threading.Tasks;

namespace PhoneDump.Contract.Services
{
    public interface IDiscoveryService
    {
        string LocalUrl { get; set; }
        Task PerformDiscovery();
        Task PerformRealDiscovery();
    }
}