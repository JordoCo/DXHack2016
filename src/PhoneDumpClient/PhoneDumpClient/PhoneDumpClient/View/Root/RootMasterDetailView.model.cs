using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Services;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.MasterDetail;
using XamlingCore.XamarinThings.Contract;

namespace PhoneDumpClient.View.Root
{
    public class RootMasterDetailViewModel : XMasterDetailViewModel

    {
        private readonly IWebService _webService;
        private readonly ILocalDumpService _localDumps;

        public RootMasterDetailViewModel(IViewResolver viewResolver, 
            IWebService webService,
            ILocalDumpService localDumps) : base(viewResolver)
        {
            _webService = webService;
            _localDumps = localDumps;
        }

        public override void OnInitialise()
        {
            NavigationTint = Color.Silver;

            //add a couple of pages.

            AddPackage<HomeViewModel>();

            SetMaster(CreateContentModel<MasterDetailMenuPageViewModel>());

            Build();

            _webService.InitServer();

            _initAsyncs();


            base.OnInitialise();
        }

        async void _initAsyncs()
        {
            await _localDumps.Init();
        }
    }
}
