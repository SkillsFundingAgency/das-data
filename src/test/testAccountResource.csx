#load "prelude.csx"
#load "../wwwroot/AccountResource/run.csx"

using System.Collections;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Web.Http.Hosting;

// Arrange
var storage = new List<BalanceChangedEvent>();
var table = storage.AsQueryable();

var req = new HttpRequestMessage();
req.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

accountUriTemplate = "http://account/{0}";

// Action
var res = Run(req, "123", table, log).Result;

// Assert
fa.Should(res.StatusCode.ToString()).Be(HttpStatusCode.NotFound.ToString());

//Arrange
storage.Add(new BalanceChangedEvent());
storage.Add(new BalanceChangedEvent());

// Action
res = Run(req, "123", table, log).Result;

// Assert
fa.Should(res.StatusCode.ToString()).Be(HttpStatusCode.OK.ToString());
fa.Should(res.Content.ReadAsStringAsync().Result)
    .Be("{\"Self\":\"http://account/123\",\"Balance\":6.0}");