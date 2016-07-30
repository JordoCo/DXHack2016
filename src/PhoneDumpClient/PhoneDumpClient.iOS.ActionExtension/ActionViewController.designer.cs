// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace PhoneDumpClient.iOS.ActionExtension
{
    [Register ("ActionViewController")]
    partial class ActionViewController
    {
        [Outlet]
        UIKit.UIImageView imageView { get; set; }


        [Outlet]
        UIKit.UILabel textLabel { get; set; }


        [Outlet]
        UIKit.UILabel TextLabel { get; set; }


        [Outlet]
        UIKit.UIView view { get; set; }


        [Action ("DoneClicked:")]
        partial void DoneClicked (Foundation.NSObject sender);

        void ReleaseDesignerOutlets ()
        {
            if (imageView != null) {
                imageView.Dispose ();
                imageView = null;
            }

            if (view != null) {
                view.Dispose ();
                view = null;
            }
        }
    }
}