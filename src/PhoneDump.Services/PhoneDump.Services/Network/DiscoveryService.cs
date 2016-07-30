using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Services;

namespace PhoneDump.Services.Network
{
    public class DiscoveryService : IDiscoveryService
    {
        public string LocalUrl { get; set; }

        public async Task PerformDiscovery()
        {
            LocalUrl = "http://10.83.234.14:8800/api/data";
        }
    }
}
