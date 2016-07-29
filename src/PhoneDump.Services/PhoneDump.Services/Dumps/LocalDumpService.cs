using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Services;
using PhoneDump.Entity;
using PhoneDump.Entity.Dumps;
using PhoneDump.Services.Messages;
using XamlingCore.Portable.Contract.Entities;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Messages.XamlingMessenger;

namespace PhoneDump.Services.Dumps
{
    public class LocalDumpService : ILocalDumpService
    {
        private readonly IEntityCache _cache;

        private ObservableCollection<DumpWireEntity> _dumps 
            = new ObservableCollection<DumpWireEntity>();

        public LocalDumpService(IEntityCache cache)
        {
            _cache = cache;

            this.Register<DumpReceivedMessage>(_onReceived);
        }

        public async Task Init()
        {
            var dumps = await _cache.GetEntity<ObservableCollection<DumpWireEntity>>(DumpConstants.LocalDumpFileName);
            if (dumps != null)
            {
                _dumps = dumps;
            }
        }

        async void _onReceived(object message)
        {
            var m = message as DumpReceivedMessage;

            await Received(m.Entity);
        }

        public async Task Received(DumpWireEntity dump)
        {
            await AddDump(dump);

            new NewDumpMessage(dump).Send();
        }

        public async Task AddDump(DumpWireEntity dump)
        {
            _dumps.Add(dump);

            await _cache.SetEntity(DumpConstants.LocalDumpFileName, _dumps);

        }
    }
}
