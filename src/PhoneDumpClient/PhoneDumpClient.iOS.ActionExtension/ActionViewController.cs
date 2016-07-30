using System;

using MobileCoreServices;
using Foundation;
using UIKit;
using PhoneDumpClient.iOS.ActionExtension.Glue;
using PhoneDump.Contract.Services;
using Autofac;
using PhoneDump.Entity.Dumps;
using System.Diagnostics;
using Newtonsoft.Json;

namespace PhoneDumpClient.iOS.ActionExtension
{
	public partial class ActionViewController : UIViewController
	{
		private string _encodedData;
		private string _rawData;

		protected ActionViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.
		}

		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();

			// Release any cached data, images, etc that aren't in use.
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Get the item[s] we're handling from the extension context.

			// For example, look for an image and place it into an image view.
			// Replace this with something appropriate for the type[s] your extension supports.
			bool imageFound = false;

			foreach (var item in ExtensionContext.InputItems)
			{
				foreach (var itemProvider in item.Attachments)
				{
					if (itemProvider.HasItemConformingTo(UTType.Image))
					{
						// This is an image. We'll load it, then place it in our image view.
						itemProvider.LoadItem(UTType.Image, null, delegate (NSObject image, NSError error)
						{
							var url = image as NSUrl;
							if (url != null)
							{
								NSOperationQueue.MainQueue.AddOperation(delegate
								{
									// Get encoded data.
									var data = UIImage.LoadFromData(NSData.FromUrl(url)).AsJPEG(1.0f);
									var str = data.GetBase64EncodedString(NSDataBase64EncodingOptions.None);
									_encodedData = str;

									SendDataAsync();
								});
							}
						});

						imageFound = true;
						break;
					}
				}

				if (imageFound)
				{
					// We only handle one image, so stop looking for more.
					break;
				}
			}
		}

		private async void SendDataAsync()
		{
			var glue = new ProjectGlue();
			glue.Init();

			var discoveryService = glue.Container.Resolve<IDiscoveryService>();
			await discoveryService.PerformRealDiscovery();

			var sendDumpService = glue.Container.Resolve<ISendDumpService>();
			var entity = new DumpWireEntity
			{
				Id = Guid.NewGuid(),
				EncodedData = _encodedData,
				RawData = _rawData,
				MediaType = "image/jpeg"
			};

			await sendDumpService.SendDump(entity);

			// Return any edited content to the host app.
			// This template doesn't do anything, so we just echo the passed-in items.
			ExtensionContext.CompleteRequest(ExtensionContext.InputItems, null);
		}

		partial void DoneClicked(NSObject sender)
		{
			// Return any edited content to the host app.
			// This template doesn't do anything, so we just echo the passed-in items.
			ExtensionContext.CompleteRequest(ExtensionContext.InputItems, null);
		}
	}
}

