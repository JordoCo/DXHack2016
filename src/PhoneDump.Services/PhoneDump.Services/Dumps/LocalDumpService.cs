using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Services;
using PhoneDump.Entity;
using PhoneDump.Entity.Dumps;
using XamlingCore.Portable.Contract.Entities;

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
        }

        public async Task Init()
        {
            var dumps = await _cache.GetEntity<ObservableCollection<DumpWireEntity>>(DumpConstants.LocalDumpFileName);
            if (dumps != null)
            {
                _dumps = dumps;
            }
        }

        public async Task AddDump(DumpWireEntity dump)
        {
            _dumps.Add(dump);

            await _cache.SetEntity(DumpConstants.LocalDumpFileName, _dumps);
        }
    }
}
