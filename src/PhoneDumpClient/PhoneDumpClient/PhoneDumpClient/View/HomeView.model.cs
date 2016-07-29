using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Servers;
using XamlingCore.Portable.View.ViewModel;

namespace PhoneDumpClient.View
{
    public class HomeViewModel : XViewModel
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenTestService _testService;
        public string MainText { get; set; }

        public HomeViewModel(ITokenService tokenService, ITokenTestService testService)
        {
            _tokenService = tokenService;
            _testService = testService;
            MainText = "Jordan";
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
