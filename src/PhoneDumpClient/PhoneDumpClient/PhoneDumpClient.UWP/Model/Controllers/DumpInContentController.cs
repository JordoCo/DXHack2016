using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Devkoes.Restup.WebServer.Attributes;
using Devkoes.Restup.WebServer.Models.Schemas;
using Devkoes.Restup.WebServer.Rest.Models.Contracts;
using PhoneDump.Entity.Dumps;

namespace PhoneDumpClient.UWP.Model.Controllers
{
    [RestController(InstanceCreationType.PerCall)]
    public sealed class DumpInContentController
    {
        //[UriFormat("/fromcontent")]
        //public IPutResponse UpdateSomething([FromContent] FromContentData data)
        //{
        //    Debug.WriteLine($"Received counter value of {data.Counter}");
        //    return new PutResponse(PutResponse.ResponseStatus.OK);
        //}

        //[UriFormat("/fromcontent")]
        //public IPostResponse CreateSomething([FromContent] FromContentData data)
        //{


        //    Debug.WriteLine($"Received counter value of {data.Counter}");
        //    return new PostResponse(PostResponse.ResponseStatus.Created, $"fromcontent/{data.Counter}");
        //}


        [UriFormat("/async")]
        public IAsyncOperation<IGetResponse> GetSomethingAsync()
        {
            return Task.FromResult<IGetResponse>(new GetResponse(GetResponse.ResponseStatus.OK, "asyncvalue")).AsAsyncOperation();
        }

        [UriFormat("/data")]
        public IPostResponse PostSomething([FromContent] DumpWireEntity data)
        {
            var img = data.EncodedData;

            var b = Convert.FromBase64String(img);
           

            Debug.WriteLine($"Received length: {b.Length}");
            return new PostResponse(PostResponse.ResponseStatus.Created, $"data/{b.Length}");
        }

        //[UriFormat("/stringencoding")]
        //public IGetResponse EncodingDecodingTest([FromContent] MoreComplexData data)
        //{

        //    var img = data.Text;

        //    var b = Convert.FromBase64String(img);



        //    Debug.WriteLine($"Received complex data with text {data.Text}");
        //    return new GetResponse(GetResponse.ResponseStatus.OK, new MoreComplexData() { Text = data.Text });
        //}
    }
}
