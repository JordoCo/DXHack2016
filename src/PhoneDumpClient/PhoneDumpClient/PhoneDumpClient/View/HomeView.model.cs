using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;

namespace PhoneDumpClient.View
{
    public class HomeViewModel : XViewModel
    {
        public string MainText { get; set; }

        public HomeViewModel()
        {
            MainText = "Jordan";
        }
    }
}
