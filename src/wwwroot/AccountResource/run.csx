#r "Microsoft.WindowsAzure.Storage"
#load "../shared.csx"
#load "../utils/current.csx"

using Microsoft.WindowsAzure.Storage.Table;
using System.Net;
using System.Linq;

public static async Task<HttpResponseMessage> Run(
    HttpRequestMessage req,
    string accountid, 
    IQueryable<BalanceChangedEvent> inTable, 
    TraceWriter log)
{
    log.Info($"C# HTTP trigger function processed a request. RequestUri={req.RequestUri}");

    //fetch the event
    var events = inTable
        .Where(e => e.PartitionKey == "test" && e.AccountId == accountid)
        .ToList();
    if (!events.Any())
        return await Task.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound));

    var acc = new AccountResource
    {
        Self = string.Format(accountUriTemplate, accountid),
        Balance = events.Sum(e => e.Amount)
    };

    var res = req.CreateResponse(HttpStatusCode.OK, acc);
    return await Task.FromResult(res);
}
