using Foundation;
using PhoneDumpClient.Services;
using System;
using System.Threading.Tasks;
using UIKit;
using PhoneDumpClient.Extensions;
using System.Linq;

namespace PhoneDumpClient.iOS.Services
{
    public class FilePickerService : IFilePickerService
    {
        public Task<string> GetFileStringAsync()
        {
            var taskCompletionSource = new TaskCompletionSource<string>();

            // Create the image picker.
            var imagePicker = new UIImagePickerController();
            imagePicker.SourceType = UIImagePickerControllerSourceType.PhotoLibrary;
            imagePicker.MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary);

            // Register event handlers.
            imagePicker.FinishedPickingMedia += (sender, e) =>
            {
                // Extract the file name.
                var referenceUrl = e.Info.Values.FirstOrDefault(v => v is NSUrl) as NSUrl;
                var assetsLibrary = new AssetsLibrary.ALAssetsLibrary();
                assetsLibrary.AssetForUrl(referenceUrl, (obj) =>
                {
                    // Get the file stream.
                    var stream = (e.EditedImage ?? e.OriginalImage).AsPNG().AsStream();
                    var bytes = stream.GetBytes();
                    var str = Convert.ToBase64String(bytes);

                    // Complete the task.
                    taskCompletionSource.SetResult(str);

                    // Dismiss the image picker.
                    imagePicker.DismissViewController(true, null);
                }, (obj) =>
                {
                    taskCompletionSource.SetResult(null);

                    // Dismiss the image picker.
                    imagePicker.DismissViewController(true, null);
                });
            };
            imagePicker.Canceled += (sender, e) =>
            {
                taskCompletionSource.SetResult(null);

                // Dismiss the image picker.
                imagePicker.DismissViewController(true, null);
            };


            // Show the image picker.
            var window = UIApplication.SharedApplication.KeyWindow;
            var viewController = window.RootViewController;
            viewController.PresentViewController(imagePicker, true, null);

            return taskCompletionSource.Task;
        }
    }
}
