using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Servers;
using XamlingCore.Portable.View.ViewModel;
using PhoneDumpClient.Services;
using Xamarin.Forms;
using System.Diagnostics;
using PhoneDump.Entity.Dumps;

namespace PhoneDumpClient.View
{
    public class HomeViewModel : XViewModel
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenTestService _testService;
        public string MainText { get; set; }

        public HomeViewModel()
        {
            _tokenService = tokenService;
            _testService = testService;
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

        public override void OnInitialise()
        {
            _test();
            base.OnInitialise();
        }

        async void _test()
        {
            var resutl = await _testService.TestToken(_tokenService.Token);
        }
    }
}
