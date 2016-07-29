using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Servers;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.MasterDetail;
using XamlingCore.XamarinThings.Contract;

namespace PhoneDumpClient.View.Root
{
    public class RootMasterDetailViewModel : XMasterDetailViewModel

    {
        private readonly IWebService _webService;

        public RootMasterDetailViewModel(IViewResolver viewResolver, IWebService webService) : base(viewResolver)
        {
            _webService = webService;
        }

        public override void OnInitialise()
        {
            NavigationTint = Color.Silver;

            //add a couple of pages.

            AddPackage<HomeViewModel>();

            SetMaster(CreateContentModel<MasterDetailMenuPageViewModel>());

            Build();

            _webService.InitServer();

            base.OnInitialise();
        }
    }
}
