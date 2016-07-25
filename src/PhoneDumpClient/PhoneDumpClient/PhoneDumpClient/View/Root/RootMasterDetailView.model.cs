using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamlingCore.XamarinThings.Content.MasterDetail;
using XamlingCore.XamarinThings.Contract;

namespace PhoneDumpClient.View.Root
{
    public class RootMasterDetailViewModel : XMasterDetailViewModel

    {
        public RootMasterDetailViewModel(IViewResolver viewResolver) : base(viewResolver)
        {
        }

        public override void OnInitialise()
        {
            NavigationTint = Color.Silver;

            //add a couple of pages.

            AddPackage<HomeViewModel>();

            SetMaster(CreateContentModel<MasterDetailMenuPageViewModel>());

            Build();

            base.OnInitialise();
        }
    }
}
