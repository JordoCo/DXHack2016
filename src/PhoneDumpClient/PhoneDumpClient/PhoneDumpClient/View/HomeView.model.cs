using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;
using PhoneDumpClient.Services;
using Xamarin.Forms;
using System.Diagnostics;
using System.Windows.Input;
using PhoneDump.Contract.Services;
using PhoneDump.Entity.Dumps;
using XamlingCore.Portable.View;

namespace PhoneDumpClient.View
{
    public class HomeViewModel : XViewModel
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenTestService _testService;
        private readonly IFilePickerService _filePickerService;
        private readonly ISendDumpService _sendDumpService;
        public string MainText { get; set; }

        public ICommand TestButtonCommand { get; set; }

        public HomeViewModel(ITokenService tokenService, ITokenTestService testService,
            IFilePickerService filePickerService,
            ISendDumpService sendDumpService)
        {

            TestButtonCommand = new XCommand(_onTestButton);

            _tokenService = tokenService;
            _testService = testService;
            _filePickerService = filePickerService;
            _sendDumpService = sendDumpService;
            MainText = "Jordan";
            
        }

        async void _onTestButton()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var str = await _filePickerService.GetFileStringAsync();
                var entity = new DumpWireEntity
                {
                    Id = Guid.NewGuid(),
                    EncodedData = str,
                    MediaType = "something",
                    RawData = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
                };
                await _sendDumpService.SendDump(entity);
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
