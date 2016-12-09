#load "utils/current.csx"
#r "Microsoft.WindowsAzure.Storage"
using System;
using Microsoft.WindowsAzure.Storage.Table;

public static string accountUriTemplate = "https://datapipe.azurewebsites.net/api/accounts/{0}";

public static string eventUriTemplate = "https://datapipe.azurewebsites.net/api/events/{0}";

public class BalanceChangedEvent : TableEntity
{
    public BalanceChangedEvent()
    {
        Timestamp = Current.Offset;
        RowKey = Current.Guid.ToString();
        AccountId = "123";
        Amount = 3m;
        PartitionKey = "test";
    }

    public string AccountId { get; set; }
    public decimal Amount { get; set; }
}

public class BalanceChangedEventMessage
{
    public string EventUri { get; set; }
}


public class BalanceChangeEventResource
{
    public string AccountUri { get; set; }
    public BalanceChangedEvent Event { get; set; }
}

public class AccountResource
{
    public string Self { get; set; }
    public decimal Balance { get; set; }
}

public class DetailedBusinessEvent : TableEntity
{
    public string AccountId { get; set; }
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
    public string SourceEventUri { get; set; }
}

public class StagedDetailedBusinessEvent : DetailedBusinessEvent
{
    public StagedDetailedBusinessEvent(DetailedBusinessEvent eve)
    {
        Timestamp = eve.Timestamp;
        AccountId = eve.AccountId;
        Amount = eve.Amount;
        Balance = eve.Balance;
        SourceEventUri = eve.SourceEventUri;
    }

    //public string PartitionKey { get; set; }
    //public string RowKey { get; set; }
}