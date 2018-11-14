# SFA Digital Apprenticeship Service Data (BETA)

This solution represents the Reporting Data load code base for the Digital Apprenticeship Service.

### Build
![Build Status](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/238/badge)

## Running locally
Create a local.settings.json file with the following schema, values will need to be populated
```
{
  "IsEncrypted": false,
  "Values": {
    "AzureWebJobsStorage": "UseDevelopmentStorage=true",
    "AzureWebJobsDashboard": "UseDevelopmentStorage=true",
    "CronSchedule": "0 */1 * * * *",
    "ConfigurationStorageConnectionString": "UseDevelopmentStorage=true",
    "MessageBusConnectionString": "",
    "EnvironmentName": "LOCAL",
    "ServiceName": "SFA.DAS.Data"
  }
}
```
