# SFA Digital Apprenticeship Service Data (BETA)

This solution represents the Reporting Data load code base for the Digital Apprenticeship Service.

### Build
![Build Status](https://sfa-gov-uk.visualstudio.com/_apis/public/build/definitions/c39e0c0b-7aff-4606-b160-3566f3bbce23/238/badge)

## Running locally

### Requirements

In order to run this solution locally you will need the following installed:

* [SQL Server LocalDB](https://www.microsoft.com/en-us/download/details.aspx?id=52679) - you only need the LocalDB component of SQL Server Express
* [Azure SDK 2.9 (for Visual Studio)](https://azure.microsoft.com/en-us/downloads/) - choose the SDK version appropriate to your version of Visual Studio

You should run Visual Studio as an Administrator.

### Setup

* Running the RunBuild.bat will expose any dependency issues
* Running the Database project (SFA.DAS.Data.Database) will provision the local DB instance with relevent seed data.
* Update the connection string in the JSON to point at your LocalDB instance (look around in Visual Studio's SQL Server Object Explorer)

### Running

Running the cloud service will start the data load application.