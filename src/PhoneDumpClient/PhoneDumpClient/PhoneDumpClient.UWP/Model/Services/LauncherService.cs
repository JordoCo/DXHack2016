using PhoneDump.Contract.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneDumpClient.UWP.Model.Services
{
    public class LauncherService : ILauncherService
    {
        public async Task Launch(string fileName)
        {
            var file = await Windows.Storage.ApplicationData.Current.LocalFolder.GetFileAsync(fileName);

            var success = await Windows.System.Launcher.LaunchFileAsync(file);
        }
    }
}
