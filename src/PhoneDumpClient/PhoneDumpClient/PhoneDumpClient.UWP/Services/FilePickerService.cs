using System;
using System.IO;
using System.Threading.Tasks;
using PhoneDumpClient.Services;
using PhoneDumpClient.Extensions;

namespace PhoneDumpClient.UWP.Services
{
    public class FilePickerService : IFilePickerService
    {
        public async Task<Stream> GetFileStreamAsync()
        {
            // Create the picker.
            var picker = new Windows.Storage.Pickers.FileOpenPicker();
            picker.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            picker.SuggestedStartLocation =
                Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;

            // Add image file type filters.
            picker.FileTypeFilter.Add(".jpg");
            picker.FileTypeFilter.Add(".jpeg");
            picker.FileTypeFilter.Add(".png");
            // TODO: Add more file type filters...

            // Get the file and stream.
            var file = await picker.PickSingleFileAsync();
            return await file.OpenStreamForReadAsync();
        }

        public async Task<string> GetFileStringAsync()
        {
            using (var stream = await GetFileStreamAsync())
            {
                var bytes = stream.GetBytes();
                var str = Convert.ToBase64String(bytes);
                return str;
            }
            return null;
        }


    }
}
