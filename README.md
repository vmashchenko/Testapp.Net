# Task description
There are hundreds of flat files with raw data are being posted to the particular folders every day. 

Each data provider has own specification doc, that provides the following info: 
-field name
- beginning
- field
- length 
- and file type (fixed length or delimited, quoted text or not,
various delimiters for 1) fields, 2) records) 

The raw data from the said files needs to be uploaded to SQL Server, 
parsed according to the spec 
and stored in the tables that would allow to query the raw data in the most efficient way. 

The solution should be aimed at the efficiency and performance.

#### Requirements
* VS 2019
* .NET Core SDK 3.1
* MS SQL Server 2016 or later


#### Techonologies
* ASP.NET Core
* Entity Framework Core 3.0
* Swagger
* RESTful API
* SeriLog
* AutoMapper

