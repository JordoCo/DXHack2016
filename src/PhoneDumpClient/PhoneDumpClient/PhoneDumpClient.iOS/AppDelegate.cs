using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using Foundation;
using PhoneDumpClient.iOS.Glue;
using PhoneDumpClient.View.Root;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamlingCore.iOS.Unified.Root;
using XamlingCore.Portable.Service.Localisation;

namespace PhoneDumpClient.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            XLocale.CultureInfo = new CultureInfo(NSLocale.PreferredLanguages[0]);

            Forms.Init();

            _configurePretties();

            var xapp = new App();
            xapp.Init<RootMasterDetailViewModel, ProjectGlue>();
            LoadApplication(xapp);
           

            return base.FinishedLaunching(app, options);
        }

        async void _configurePretties()
        {
            UINavigationBar.Appearance.BackgroundColor = UIColor.FromRGBA(0, 0, 0, 0);
            UINavigationBar.Appearance.TintColor = Color.Blue.ToUIColor();
            UINavigationBar.Appearance.BarTintColor = UIColor.White;
            UINavigationBar.Appearance.SetTitleTextAttributes(new UITextAttributes()
            {
                TextColor = UIColor.Black
            });

            await System.Threading.Tasks.Task.Delay(1000);

            var rootView = UIApplication.SharedApplication.KeyWindow.RootViewController;
            XiOSRoot.RootViewController = rootView;
            XiOSRoot.RootWindow = UIApplication.SharedApplication.KeyWindow;
        }
    }
}
