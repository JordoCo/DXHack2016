using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Services;

using PhoneDump.Entity.Config;
using XamlingCore.Portable.Contract.Config;
using XamlingCore.Portable.Contract.Repos.Base;
using XamlingCore.Portable.Contract.Serialise;

namespace PhoneDump.Services.Network
{
    public class DiscoveryService : IDiscoveryService
    {
        private readonly IXWebRepo<IPSettings> _settingsRepo;
        private readonly IEntitySerialiser _ser;
        public string LocalUrl { get; set; }

        public DiscoveryService(IXWebRepo<IPSettings> settingsRepo, IEntitySerialiser ser)
        {
            _settingsRepo = settingsRepo;
            _ser = ser;
        }

        public async Task PerformDiscovery()
        {
			LocalUrl = "http://10.106.20.76:8800/api/data";
		}

        public async Task PerformRealDiscovery()
        {
            await PerformDiscovery();
            return;
            _settingsRepo.SetEndPoint("http://testapiforlogics.azurewebsites.net/ip.txt");

            var result = await _settingsRepo.GetResult();

            var s = result.Result;

            LocalUrl = _ser.Deserialise<IPSettings>(s)?.IPAddress;
        }
    }
}
