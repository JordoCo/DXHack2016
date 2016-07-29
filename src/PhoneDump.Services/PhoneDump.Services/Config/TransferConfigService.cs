using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneDump.Contract.Services;
using XamlingCore.Portable.Contract.Config;
using XamlingCore.Portable.Contract.Downloaders;
using XamlingCore.Portable.Net.DownloadConfig;
using XamlingCore.Portable.Net.Service;

namespace PhoneDump.Services.Config
{
    public class TransferConfig : HttpTransferConfigServiceBase
    {
        private readonly IConfig _config;
        private readonly ITokenService _tokenService;

        public TransferConfig(IConfig config, ITokenService tokenService)
        {
            _config = config;
            _tokenService = tokenService;
        }

#pragma warning disable 1998 // Overridden method
        public override async Task<IHttpTransferConfig> GetConfig(string url, string verb)
#pragma warning restore 1998
        {
            var config = new StandardHttpConfig
            {
                Accept = "application/json",
                IsValid = true,
                Url = url,
                BaseUrl = url,
                Verb = verb,
                Headers = new Dictionary<string, string>(),
                Retries = 2,
                ContentEncoding = "application/json"
                //ContentEncoding = verb == "POST" ? "application/x-www-form-urlencoded" : "text/plain"
            };

            config.Headers.Add("Authorization", "Bearer " + _tokenService.Token);

            return config;
        }
    }
}
