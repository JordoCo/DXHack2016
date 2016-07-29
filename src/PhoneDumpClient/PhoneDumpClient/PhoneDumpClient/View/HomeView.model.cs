﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamlingCore.Portable.View.ViewModel;
using PhoneDumpClient.Services;
using Xamarin.Forms;
using System.Diagnostics;
using System.Windows.Input;
using PhoneDump.Contract.Services;
using PhoneDump.Entity.Dumps;
using XamlingCore.Portable.View;
using XamlingCore.Portable.Messages.XamlingMessenger;
using PhoneDump.Services.Messages;
using System.IO;

namespace PhoneDumpClient.View
{
    public class HomeViewModel : XViewModel
    {
        private readonly ITokenService _tokenService;
        private readonly ITokenTestService _testService;
        private readonly IFilePickerService _filePickerService;
        private readonly ISendDumpService _sendDumpService;

        public string _mainText;

        public ICommand TestButtonCommand { get; set; }

        private ImageSource _dumpSource;
        public ImageSource DumpSource { get { return _dumpSource; }
            set { _dumpSource = value;  OnPropertyChanged();  } }

        public HomeViewModel(ITokenService tokenService, ITokenTestService testService,
            IFilePickerService filePickerService,
            ISendDumpService sendDumpService)
        {

            this.Register<NewDumpMessage>(_onNewDumpMessage);

            TestButtonCommand = new XCommand(_onTestButton);

            _tokenService = tokenService;
            _testService = testService;
            _filePickerService = filePickerService;
            _sendDumpService = sendDumpService;

            _timer();

        }

        async void _timer()
        {
            while (true)
            {
                MainText = DateTime.Now.ToString();
                await Task.Delay(1000);
            }
        }

        void _onNewDumpMessage(object message)
        {
            var m = message as NewDumpMessage;

            if (m?.Entity == null)
            {
                return;
            }

            _processImage(m.Entity);
        }

        void _processImage(DumpWireEntity dump)
        {
            // Overtake string-data from object in variable
            string cFotoBase64 = dump.EncodedData; // Overtake string-data from object in variable
                                                    // Convert in Byte-Array with encoding
            Byte[] ImageFotoBase64 = System.Convert.FromBase64String(cFotoBase64);
            // Create Image and set stream from converted Byte-Array as source
            DumpSource = ImageSource.FromStream(() => new MemoryStream(ImageFotoBase64));//, WidthRequest = 200, HeightRequest = 200, BackgroundColor = Color.Aqua, };
        }

        async void _onTestButton()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var str = await _filePickerService.GetFileStringAsync();
                var entity = new DumpWireEntity
                {
                    Id = Guid.NewGuid(),
                    EncodedData = str,
                    MediaType = "something",
                    RawData = "https://www.youtube.com/watch?v=dQw4w9WgXcQ"
                };
                await _sendDumpService.SendDump(entity);
            });
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

        public string MainText
        {
            get { return _mainText; }
            set
            {
                _mainText = value; 
                OnPropertyChanged();
            }
        }

    }
}
