using System.Threading.Tasks;

namespace PhoneDumpClient.Services
{
    public interface IFilePickerService
    {
        Task<string> GetFileStringAsync();
    }
}
