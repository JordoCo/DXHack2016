using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneDumpClient.Services
{
    public interface IFilePickerService
    {
        Task<Stream> GetFileStreamAsync();
        Task<string> GetFileStringAsync();

    }
}
