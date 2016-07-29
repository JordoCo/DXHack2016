using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Services;
using PhoneDump.Entity.Dumps;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Model.Response;

namespace PhoneDump.Services.Dumps
{
    public class SendDumpService : ISendDumpService
    {
        private readonly IXWebRepo<DumpPostResult> _dumpLocalNetworkRepo;
        private readonly IDiscoveryService _discoveryService;
        private readonly IXWebRepo<DumpPostResult> _localNetworkRepo;

        public SendDumpService(IXWebRepo<DumpPostResult> dumpLocalNetworkRepo,
             IDiscoveryService discoveryService,
            IXWebRepo<DumpPostResult> localNetworkRepo)
        {
            _dumpLocalNetworkRepo = dumpLocalNetworkRepo;
            _discoveryService = discoveryService;
            _localNetworkRepo = localNetworkRepo;
        }
        public async Task<XResult<bool>> SendDump(DumpWireEntity entity)
        {
            var dumpUrl = _discoveryService.LocalUrl;

            _localNetworkRepo.SetEndPoint(dumpUrl);

            var result = await _localNetworkRepo.Post(entity);

            Debug.WriteLine($"Dump result: {result?.Object?.Code}");

            return result.Copy<bool>();
        }
    }
}
