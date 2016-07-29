using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;
using PhoneDumpClient.Services;
using Xamarin.Forms;
using System.Diagnostics;
using PhoneDump.Entity.Dumps;

namespace PhoneDumpClient.View
{
    public class HomeViewModel : XViewModel
    {
        public string MainText { get; set; }

        public HomeViewModel(IFilePickerService filePickerService)
        {
            MainText = "Jordan";
            Device.BeginInvokeOnMainThread(async () =>
            {
                var str = await filePickerService.GetFileStringAsync();
                var entity = new DumpWireEntity
                {
                    Id = Guid.NewGuid(),
                    EncodedData = str,
                    MediaType = "something"
                };
            });
        }
    }
}
