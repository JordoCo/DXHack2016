#r "Newtonsoft.Json"

using System;
using System.Net;
using Newtonsoft.Json;

public static object Run(HttpRequestMessage req, 
out object document,  out IDictionary<string, string> notification, TraceWriter log)
{
    log.Info($"Webhook was triggered!");

    notification = null;
    document = null;

    string jsonContent = req.Content.ReadAsStringAsync().Result;
    dynamic data = JsonConvert.DeserializeObject(jsonContent);

    if (data.first == null || data.last == null) {
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Please pass first/last properties in the input object"
        });
    }
    
    document = new {
        first = data.first,
        last = data.last
    };
    

    return req.CreateResponse(HttpStatusCode.OK, new {
        greeting = $"Hello {data.first} {data.last}!"
    });
}

private static IDictionary<string, string> GetTemplateProperties(string message)
{
    Dictionary<string, string> templateProperties = new Dictionary<string, string>();
    templateProperties["message"] = message;
    return templateProperties;
}

