using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Devkoes.Restup.WebServer.File;
using Devkoes.Restup.WebServer.Http;
using Devkoes.Restup.WebServer.Rest;

using PhoneDumpClient.UWP.Model.Controllers;
using PhoneDump.Contract.Services;

namespace PhoneDumpClient.UWP.Model.Services
{
    public class WebService : IWebService
    {
        private HttpServer _httpServer;
        public async Task InitServer()
        {
            _httpServer = new HttpServer(8800);

            var restRouteHandler = new RestRouteHandler();

            restRouteHandler.RegisterController<DumpInContentController>();

            _httpServer.RegisterRoute("api", restRouteHandler);

        //    _httpServer.RegisterRoute(new StaticFileRouteHandler(@"DemoStaticFiles\Web"));
            await _httpServer.StartServerAsync();
        }
    }
}
